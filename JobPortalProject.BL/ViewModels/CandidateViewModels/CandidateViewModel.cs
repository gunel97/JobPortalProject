using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortalProject.BL.ViewModels.CandidateViewModels
{
    public class CandidateViewModel
    {
        public int Id { get; set; }
        
    }

    public class CandidateCreateViewModel {
        public string AppUserId { get; set; } = null!;
    }

    public class CandidateUpdateViewModel { }

 
}
