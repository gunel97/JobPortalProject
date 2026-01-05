using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortalProject.BL.UI.ViewModels
{
    public class AccountSettingsViewModel
    {
        public ChangePasswordViewModel? ChangePasswordModel { get; set; }
        public ChangeEmailViewModel? ChangeEmailModel { get; set; }
        public DeleteAccountViewModel? DeleteAccount { get; set; }
    }

    public class DeleteAccountViewModel
    {
        [DataType(DataType.Password)]
        public required string CurrentPasswordForDelete { get; set; }
    }
}
