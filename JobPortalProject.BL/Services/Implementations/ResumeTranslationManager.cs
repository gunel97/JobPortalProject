using AutoMapper;
using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.ViewModels.ResumeViewModels;
using JobPortalProject.DA.DataContext.Entities;
using JobPortalProject.DA.Repositories.Contracts;
using System.Net.WebSockets;

namespace JobPortalProject.BL.Services.Implementations
{
    public class ResumeTranslationManager : CrudManager<ResumeTranslation, ResumeTranslationViewModel, ResumeTranslationCreateViewModel, ResumeTranslationUpdateViewModel>
        , IResumeTranslationService
    {
        public ResumeTranslationManager(IRepositoryAsync<ResumeTranslation> repository, IMapper mapper):base(repository, mapper) { }

        public async Task<ResumeTranslationViewModel> Create(ResumeTranslationCreateViewModel model, int resumeId)
        {
            string[] skills = model.Skills!.Split(',');
            string[] languages = model.Languages!.Split(",");

            var resumeTranslation = new ResumeTranslation
            {
                ResumeId = resumeId,
                LanguageId = model.LanguageId,
                About = model.About,
                Skills = skills.Select(x => x).ToList(),
                Languages=languages.Select(x=>x).ToList(),
            };

            var result = await Repository.AddAsync(resumeTranslation);

            var resumeTranslationViewModel = Mapper.Map<ResumeTranslationViewModel>(result);

            return resumeTranslationViewModel;
        }
    }
}
