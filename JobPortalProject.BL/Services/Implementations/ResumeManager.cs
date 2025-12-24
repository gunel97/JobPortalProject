using AutoMapper;
using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.ViewModels.ResumeViewModels;
using JobPortalProject.DA.DataContext.Entities;
using JobPortalProject.DA.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace JobPortalProject.BL.Services.Implementations
{
    public class ResumeManager:CrudManager<Resume, ResumeViewModel, ResumeCreateViewModel, ResumeUpdateViewModel>
        , IResumeService
    {
        public ResumeManager(IRepositoryAsync<Resume> repository, IMapper mapper):base(repository, mapper) { }

        public async Task<Resume> GetResumeWithDetailsAsync(int resumeId, int languageId)
        {
            var resume =await
                Repository.GetAsync(predicate: x => x.Id == resumeId,
                include: x => x.Include(x => x.PersonalInfo)!.ThenInclude(x => x.Translations.Where(t => t.LanguageId == languageId))
                .Include(x => x.PersonalInfo).ThenInclude(x => x.Address).ThenInclude(x => x.AddressTranslations.Where(t => t.LanguageId == languageId))
                .Include(x => x.PersonalInfo).ThenInclude(x => x.Address).ThenInclude(x=>x.City).ThenInclude(x => x.CityTranslations.Where(t => t.LanguageId == languageId))
                .Include(x => x.PersonalInfo).ThenInclude(x => x.Address).ThenInclude(x=>x.City).ThenInclude(x=>x.Country).ThenInclude(x => x.Translations.Where(t => t.LanguageId == languageId))
                .Include(x => x.Translations.Where(t => t.LanguageId == languageId))
                .Include(x => x.Experiences).ThenInclude(x => x.Translations.Where(t => t.LanguageId == languageId))
                .Include(x => x.Educations).ThenInclude(x => x.Translations.Where(t => t.LanguageId==languageId))
                );

            if (resume == null)
                return null!;
            return resume;

        }
    }
}
