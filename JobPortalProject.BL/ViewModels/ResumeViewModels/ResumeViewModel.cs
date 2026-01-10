using JobPortalProject.BL.ViewModels.EducationViewModels;
using JobPortalProject.BL.ViewModels.ExperienceViewModels;
using JobPortalProject.BL.ViewModels.PersonalInfoViewModels;
using JobPortalProject.DA.DataContext.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortalProject.BL.ViewModels.ResumeViewModels
{
    public class ResumeViewModel
    {
        public int Id {  get; set; }
        public int CandidateId {  get; set; }
        public string About { get; set; } = null!;
        public List<string> Skills { get; set; } = [];
        public List<string> Languages { get; set; } = [];
        public PersonalInfoViewModel PersonalInfo { get; set; } = null!;
        public List<ExperienceViewModel> Experiences { get; set; } = [];
        public List<EducationViewModel> Educations { get; set; } = [];
    }

    public class ResumeCreateViewModel
    {
        public int CandidateId { get; set; }
    }

    public class ResumeUpdateViewModel { }

    public class ResumeTranslationViewModel
    {

        public int ResumeId { get; set; }
        public int LanguageId { get; set; }
        public string? About { get; set; }
        public List<string> Languages { get; set; } = [];
        public List<string> Skills { get; set; } = [];
        public bool IsCompleted { get; set; }
    }

    public class ResumeTranslationCreateViewModel
    {
        public string? About { get; set; }
        public string? Skills { get; set; }
        public string? Languages { get; set; }
        public int ResumeId { get; set; }
        public int LanguageId { get; set; }
    }

    public class ResumeTranslationUpdateViewModel
    {
        public int Id { get; set; }
        public string About { get; set; } = null!;
        public string Skills { get; set; }=null!;
        public string Languages { get; set; } = null!;
        public int ResumeId { get; set; }
        public int LanguageId { get; set; }
    }
}
