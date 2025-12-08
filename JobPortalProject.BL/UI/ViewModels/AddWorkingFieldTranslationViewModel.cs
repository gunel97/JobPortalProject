using JobPortalProject.BL.ViewModels.WorkingFieldViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortalProject.BL.UI.ViewModels
{
    public class AddWorkingFieldTranslationViewModel
    {
        public int SelectedLanguageId { get; set; }
        public int WorkingFieldId { get; set; }
        public WorkingFieldTranslationCreateViewModel? TranslationCreateViewModel { get; set; }
        public List<SelectListItem>? WorkingFields { get; set; }
    }
}
