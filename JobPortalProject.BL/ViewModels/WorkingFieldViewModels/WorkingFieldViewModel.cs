using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortalProject.BL.ViewModels.WorkingFieldViewModels
{
    public class WorkingFieldViewModel
    {
        public int Id { get; set; }
        public string? IconUrl { get; set; }
        public string? IconPublicId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    }

    public class WorkingFieldCreateViewModel
    {
        public int CompanyId { get; set; }
        public int SelectedUpdateLanguageId { get; set; }
        public IFormFile? IconFile { get; set; }
        public string? IconUrl { get; set; }
        public string? IconPublicId { get; set; }
        public WorkingFieldTranslationCreateViewModel? WorkingFieldTranslationCreateViewModel { get; set; }
    }

    public class WorkingFieldUpdateViewModel
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string? IconUrl { get; set; }
        public string? IconPublicId { get; set; }
        public IFormFile? IconFile { get; set; }
        public WorkingFieldTranslationUpdateViewModel? WorkingFieldTranslationUpdateViewModel { get; set; } 
    }

    public class WorkingFieldTranslationViewModel
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int LanguageId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    }

    public class WorkingFieldTranslationCreateViewModel
    {
        public int WorkingFieldId { get; set; }
        public int LanguageId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    }

    public class WorkingFieldTranslationUpdateViewModel
    {
        public int Id { get; set; }
        public int WorkingFieldId { get; set; }
        public int LanguageId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}
