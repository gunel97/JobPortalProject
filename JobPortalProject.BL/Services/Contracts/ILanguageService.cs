using JobPortalProject.BL.ViewModels.LanguageViewModels;
using JobPortalProject.DA.DataContext.Entities;
using JobPortalProject.DA.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortalProject.BL.Services.Contracts
{
    public interface ILanguageService:ICrudService<Language, LanguageViewModel,LanguageCreateViewModel, LanguageUpdateViewModel>
    {
    }
}
