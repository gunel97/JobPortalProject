using JobPortalProject.BL.ViewModels.Pagination;
using JobPortalProject.DA.DataContext.Enums;

namespace JobPortalProject.BL.ViewModels.OrderViewModels
{

    public class OrderViewModel
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public PaymentStatus Status { get; set; } 
        public decimal Amount { get; set; }
    }

    public class OrderPagedViewModel
    {
        public OrderFilterViewModel? Filter { get; set; }
        public PagedResultModel<OrderViewModel> Orders { get; set; } = null!;
    }


    public class OrderFilterViewModel
    {
        public string? SearchTerm { get; set; }
        public string SortBy { get; set; } = "CreatedAt";
        public string SortOrder { get; set; } = "desc";
        public int Index { get; set; } = 0;
        public int Size { get; set; } = 10;
        public PaymentStatus? Status { get; set; }
    }

    public class OrderCreateViewModel { }

    public class OrderUpdateViewModel { }
}
