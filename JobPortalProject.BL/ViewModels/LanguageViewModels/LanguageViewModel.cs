using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortalProject.BL.ViewModels.LanguageViewModels
{
    public class LanguageViewModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? IsoCode { get; set; }
        public string? IconPublicId { get; set; } 
        public string? IconUrl { get; set; }
    }

    public class LanguageCreateViewModel { }

    public class LanguageUpdateViewModel { }
}
