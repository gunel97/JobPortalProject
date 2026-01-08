using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortalProject.BL.ViewModels.JobResponsibilityViewModels
{
    public class JobResponsibilityViewModel
    {
        public int Id { get; set; }
        public int JobId { get; set; }
    }
    public class JobResponsibilityCreateViewModel
    {
        public int JobId { get; set; }
        public List<JobResponsibilityTranslationCreateViewModel> JobResponsibilityTranslations { get; set; } = [];
    }
    public class JobResponsibilityUpdateViewModel
    {
        public int Id { get; set; }
        public List<JobResponsibilityTranslationUpdateViewModel> JobResponsibilityTranslations { get; set; } = [];
    }

    public class JobResponsibilityTranslationViewModel
    {

    }
    public class JobResponsibilityTranslationCreateViewModel
    {
        public int JobResponsibilityId { get; set; }
        public int LanguageId { get; set; }
        [Required(ErrorMessage = "Username is required")]
        public string? Value { get; set; } 
        public string? ReadyValue { get; set; }
    }
    public class JobResponsibilityTranslationUpdateViewModel
    {
        public int Id { get; set; }
        public int LanguageId { get; set; }
        public string? Value { get; set; }
    }
}
