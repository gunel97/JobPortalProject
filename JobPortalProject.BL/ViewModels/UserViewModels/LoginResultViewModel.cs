namespace JobPortalProject.BL.ViewModels.UserViewModels
{
    public class LoginResultViewModel
    {
        public bool IsSuccess { get; set; }
        public string? UserId { get; set; }
        public string? Username { get; set; }
        public string? Role { get; set; }
    }
}
