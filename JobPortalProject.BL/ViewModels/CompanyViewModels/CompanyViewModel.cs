using JobPortalProject.BL.ViewModels.AddressViewModels;
using JobPortalProject.BL.ViewModels.WorkingFieldViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortalProject.BL.ViewModels.CompanyViewModels
{
    public class CompanyViewModel
    {
        public string? Name { get; set; }
        public string? LogoUrl {  get; set; }
        public int CompanySize { get; set; }
        public AddressViewModel? MainAddress { get; set; }
        public List<AddressViewModel> CompanyAddresses { get; set; } = [];
        public List<WorkingFieldViewModel> WorkingFields { get; set; } = [];

        
    }

    public class CompanyCreateViewModel { }

    public class CompanyUpdateViewModel { }

    public class CompanyTranslationViewModel
    {
    }

    public class CompanyTranslationCreateViewModel { }

    public class CompanyTranslationUpdateViewModel { }


}
