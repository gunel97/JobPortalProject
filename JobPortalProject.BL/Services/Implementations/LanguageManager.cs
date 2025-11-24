using AutoMapper;
using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.ViewModels.LanguageViewModels;
using JobPortalProject.DA.DataContext.Entities;
using JobPortalProject.DA.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortalProject.BL.Services.Implementations
{
    public class LanguageManager:CrudManager<Language, LanguageViewModel, LanguageCreateViewModel, LanguageUpdateViewModel>
        ,ILanguageService
    {
        public LanguageManager(IRepositoryAsync<Language> repository, IMapper mapper):base(repository, mapper) { }
    }
}
