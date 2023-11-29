using MicroBytKonamic.Commom.Data;
using MicroBytKonamic.Commom.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroBytKonamic.Application.Services;

internal class PostalesServices : IPostalesServices
{
    private readonly MicrobytkonamicContext _dbContext;
    private readonly IMapper _mapper;

    public PostalesServices(MicrobytkonamicContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<GetFelicitacionResult> GetFelicitacion(IntegerIntervals intervals)
    {
        var query = _dbContext.Postales.OrderByDescending(p => p.IdPostales);
        var postal = await query.FirstOrDefaultAsync();
        var dto = _mapper.Map<FelicitacionDto>(postal);

        if (postal != null)
            intervals.Add(postal.IdPostales);

        return new GetFelicitacionResult { FelicitacionDto = dto, Intervals = intervals };
    }
}
