using System.ComponentModel.DataAnnotations;

namespace JobPortalProject.BL.UI.ViewModels
{
    public class ChangeEmailViewModel
    {
        [Required]
        [EmailAddress]
        public string NewEmail { get; set; } = null!;
        [Required]
        [DataType(DataType.Password)]
        public string CurrentPasswordForEmail { get; set; } = null!;
    }
}
