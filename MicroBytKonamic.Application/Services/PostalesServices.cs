using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;

namespace MicroBytKonamic.Application.Services;

internal class PostalesServices : IPostalesServices
{
    private readonly MicrobytkonamicContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IResourcesServices _resourcesServices;
    private readonly IConfiguration _configuration;
    private readonly IStringLocalizer _localizer;

    public PostalesServices(MicrobytkonamicContext dbContext, IMapper mapper, IResourcesServices resourcesServices, IConfiguration configuration, IStringLocalizer<SharedResource> localizer)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _resourcesServices = resourcesServices;
        _configuration = configuration;
        _localizer = localizer;
    }

    public int CalcAnyo(DateTime date)
    {
        var month = date.Month;
        var year = month switch { 12 => date.Year, _ => date.Year - 1 };

        return year;
    }

    public bool EsNavidad(DateTime date)
    {
        string? cfgStr = _configuration["ShowFrmNuevaPostal"];

        if (cfgStr != null && bool.TryParse(cfgStr, out var showFrmNuevaPostal))
            return showFrmNuevaPostal;

        var month = date.Month;
        var day = date.Day;

        return month switch
        {
            1 => day <= 7,
            12 => day >= 20,
            _ => false
        };
    }

    public async Task<GetFelicitacionResult> GetFelicitacionAsync(GetFelicitacionIn input)
    //=> await Task.Run(() => new GetFelicitacionResult
    //{
    //    Intervals = new(),
    //    FelicitacionDto = new()
    //    {
    //        Nick = "dd",
    //        Fecha = DateTime.Now,
    //        Texto = _localizer["NickRequired"]
    //    }
    //});
    {
        FelicitacionDto? dto = null;
        IQueryable<Postale> query = from p in _dbContext.Postales where p.Anyo == input.Anyo orderby p.IdPostales descending select p;
        var intervals = input.Intervals;
        var postal0 = await query.FirstOrDefaultAsync();

        if (postal0 != null)
        {
            AddNotIntervals(ref query, input.Intervals);

            var postal = await query.FirstOrDefaultAsync();

            if (postal == null)
            {
                postal = postal0;
                intervals.Clear();
            }

            dto = _mapper.Map<FelicitacionDto>(postal);
            intervals.Add(postal.IdPostales);
        }

        return new GetFelicitacionResult { FelicitacionDto = dto, Intervals = intervals };
    }

    public async Task<IntegerIntervals> AltaFelicitacionAsync(AltaFelicitacionIn input, CancellationToken cancellationToken = default)
    {
        if (!input.FelicitacionDto.Fecha.HasValue)
            throw new ArgumentNullException("Fecha is null");

        if (string.IsNullOrWhiteSpace(input.FelicitacionDto.Nick))
            throw new MBException("el nick es obligatorio");

        if (string.IsNullOrWhiteSpace(input.FelicitacionDto.Texto))
            throw new MBException("el texto es obligatorio");

        var postal = _mapper.Map<Postale>(input.FelicitacionDto);

        postal.Anyo = input.Anyo;
        await _dbContext.Postales.AddAsync(postal, cancellationToken);
        cancellationToken.ThrowIfCancellationRequested();
        await _dbContext.SaveChangesAsync(cancellationToken);
        cancellationToken.ThrowIfCancellationRequested();

        return new IntegerIntervals(postal.IdPostales, postal.IdPostales);
    }

    public async Task<byte[]> ReadMP3NavidadAsync()
    {
        var file = _resourcesServices.GetRandomNavidadMp3();
        var data = await File.ReadAllBytesAsync(file);

        return data;
    }

    internal void AddNotIntervals(ref IQueryable<Postale> query, IntegerIntervals intervals)
    {
        /*
             [a b] [c d]
             (id>=a && id<=b) || (id>=c && id<=d)
             !((id>=a && id<=b) || (id>=c && id<=d)) =
             !(id>=a && id<=b) && !(id>=c && id<=d) =
             (id<a || id>b) && (id<c || id>d)
         */
        foreach (var interval in intervals.Intervals)
        {
            query = query.Where(p => p.IdPostales < interval.Start || p.IdPostales > interval.End);
        }
    }
}
