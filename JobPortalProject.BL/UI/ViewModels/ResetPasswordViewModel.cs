using System.ComponentModel.DataAnnotations;

namespace JobPortalProject.BL.UI.ViewModels
{
    public class ResetPasswordViewModel
    {
        [DataType(DataType.Password)]
        public required string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare(nameof(NewPassword))]
        public required string ConfirmPassword { get; set; }
        public required string Email { get; set; }
        public required string ResetToken { get; set; }
    }
}
