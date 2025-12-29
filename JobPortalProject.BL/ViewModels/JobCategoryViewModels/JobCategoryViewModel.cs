using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortalProject.BL.ViewModels.JobCategoryViewModels
{
    public class JobCategoryViewModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? ImagePublicId { get; set; }
        public string? ImageUrl { get; set; }
        public List<int> JobIds { get; set; } = [];
        public bool IsDeleted { get; set; }
    }

    public class JobCategoryCreateViewModel
    {
        public IFormFile ImageFile { get; set; } = null!;
        public string? ImagePublicId { get; set; }
        public string? ImageUrl { get; set; }
        public List<JobCategoryTranslationCreateViewModel> Translations { get; set; } = [];
    }

    public class JobCategoryUpdateViewModel {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        public IFormFile? ImageFile { get; set; } = null!;
        public string? ImagePublicId { get; set; }
        public string? ImageUrl { get; set; }
        public List<JobCategoryTranslationUpdateViewModel> Translations { get; set; } = [];
    }

    public class JobCategoryTranslationViewModel
    {
        public int Id { get; set; }
        public int LanguageId { get; set; }
        public string? Name { get; set; }
    }

    public class JobCategoryTranslationCreateViewModel
    {
        public int JobCategoryId { get; set; }
        public string Name { get; set; } = null!;
        public int LanguageId { get; set; }
    }

    public class JobCategoryTranslationUpdateViewModel
    {
        public int Id { get; set; }
        public int JobCategoryId { get; set; }
        public string Name { get; set; } = null!;
        public int LanguageId { get; set; }
        public string? LanguageIcon { get; set; }
    }
}
