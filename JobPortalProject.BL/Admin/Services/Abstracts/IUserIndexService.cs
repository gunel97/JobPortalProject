using JobPortalProject.BL.Admin.ViewModels;
using JobPortalProject.BL.ViewModels.UserViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortalProject.BL.Admin.Services.Abstracts
{
    public interface IUserIndexService
    {
        public Task<UserPagedIndexViewModel> GetPagedUserIndexModel(UserFilterViewModel filter);
    }
}
