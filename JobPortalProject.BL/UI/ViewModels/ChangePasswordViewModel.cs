using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortalProject.BL.UI.ViewModels
{
    public class ChangePasswordViewModel
    {
        public required string CurrentPassword { get; set; }

        [DataType(DataType.Password)]
        public required string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare(nameof(NewPassword))]
        public required string ConfirmPassword { get; set; }
    }
}
