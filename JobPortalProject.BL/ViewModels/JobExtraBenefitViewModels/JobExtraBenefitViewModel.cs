using JobPortalProject.BL.ViewModels.JobResponsibilityViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortalProject.BL.ViewModels.JobExtraBenefitViewModels
{
    public class JobExtraBenefitViewModel
    {
        public int Id { get; set; }
        public int JobId { get; set; }
    }
    public class JobExtraBenefitCreateViewModel
    {
        public int JobId { get; set; }
        public List<JobExtraBenefitTranslationCreateViewModel> JobExtraBenefitTranslations { get; set; } = [];
    }
    public class JobExtraBenefitUpdateViewModel
    {
        public int Id { get; set; }
        public List<JobExtraBenefitTranslationUpdateViewModel> JobExtraBenefitTranslations { get; set; } = [];
    }
    public class JobExtraBenefitTranslationViewModel
    {
    }
    public class JobExtraBenefitTranslationCreateViewModel
    {
        public int JobExtraBenefitId { get; set; }
        public int LanguageId { get; set; }
        public string Value { get; set; } = null!;
    }
    public class JobExtraBenefitTranslationUpdateViewModel
    {
        public int Id { get; set; }
        public int LanguageId { get; set; }
        public string? Value {  get; set; }
    }
}
