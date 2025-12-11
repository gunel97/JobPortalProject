using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortalProject.BL.ViewModels.JobResponsibilityViewModels
{
    public class JobResponsibilityViewModel
    {

    }
    public class JobResponsibilityCreateViewModel
    {
        public int JobId { get; set; }
        public List<JobResponsibilityTranslationCreateViewModel> Translations { get; set; } = [];
    }
    public class JobResponsibilityUpdateViewModel
    {

    }

    public class JobResponsibilityTranslationViewModel
    {

    }
    public class JobResponsibilityTranslationCreateViewModel
    {
        public int JobResponsibilityId { get; set; }
        public int LanguageId { get; set; }
        public string Value { get; set; } = null!;
    }
    public class JobResponsibilityTranslationUpdateViewModel
    {

    }
}
