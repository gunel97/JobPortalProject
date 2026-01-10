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

        public async Task<bool> Complete(int resumeId, int languageId)
        {
            var resumeTranslation = await Repository.GetAsync(predicate: x => x.ResumeId == resumeId && x.LanguageId == languageId);
            if (resumeTranslation == null)
                return false;
            resumeTranslation.IsCompleted = true;
           var result =  await Repository.UpdateAsync(resumeTranslation);
            if (result == null)
                return false;

            return true;
        }

        public async Task<bool> Update(ResumeTranslationUpdateViewModel model)
        {
            string[] languages = model.Languages.Split(',');
            string[] skills = model.Skills.Split(',');

            var resumeTranslation = await Repository.GetByIdAsync(model.Id);
            if (resumeTranslation == null)
                return false;

            resumeTranslation.Skills = skills.ToList();
            resumeTranslation.Languages=languages.ToList();
            resumeTranslation.About= model.About;

            var result = await Repository.UpdateAsync(resumeTranslation);
            if (result == null)
                return false;
            return true;
        }
    }
}
