using AutoMapper.QueryableExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroBytKonamic.Application.Services;

public class LanguagesServices(LanguagesContainer _container, MicrobytkonamicContext _dbContext, IMapper _mapper) : ILanguagesServices
{
    private readonly LanguagesContainer _container = _container;
    private readonly MicrobytkonamicContext _dbContext = _dbContext;
    private readonly IMapper _mapper = _mapper;

    public async Task<LanguageDto[]> GetSupportedLanguageDtos(CancellationToken cancellationToken)
        => await _dbContext.Languages.Where(l => _container.SupportedLanguagesName.Contains(l.Culture)).ProjectTo<LanguageDto>(_mapper.ConfigurationProvider).ToArrayAsync(cancellationToken);
}
