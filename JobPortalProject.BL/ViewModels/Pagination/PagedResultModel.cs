using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortalProject.BL.ViewModels.Pagination
{
    public interface IPagedResult
    {
        int Index { get; set; }
        int Size { get; set; }
        int Count { get; set; }
        int Pages { get; set; }
        bool HasPrevious { get; }
        bool HasNext { get; }
    }
    public class PagedResultBaseModel : IPagedResult
    {
        public int Index { get; set; }
        public int Size { get; set; }
        public int Count { get; set; }
        public int Pages { get; set; }

        public bool HasPrevious => Index > 0;
        public bool HasNext => Index + 1 < Pages;
    }

    public class PagedResultModel<T> : PagedResultBaseModel
    {
        public List<T> Items { get; set; }

        public PagedResultModel()
        {
            Items = new List<T>();
        }
    }
}
