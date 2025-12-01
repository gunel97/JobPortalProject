using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortalProject.BL.ViewModels.JobCategoryViewModels
{
    public class JobCategoryViewModel
    {
        public int Id { get; set; }
        public string? Name {  get; set; }
        public string? ImagePublicId { get; set; }
        public string? ImageUrl { get; set; }
        public List<int> JobIds { get; set; } = [];
    }
    public class JobCategoryCreateViewModel { }

    public class JobCategoryUpdateViewModel { }

    public class JobCategoryTranslationViewModel { }

    public class JobCategoryTranslationCreateViewModel { }

    public class JobCategoryTranslationUpdateViewModel { }
}
