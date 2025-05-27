using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMVCProject.Models.ViewModels
{
    public class ProductCatalogVM
    {
        public IEnumerable<Product> Products { get; set; }
        public int? SelectedCategoryId { get; set; }
        public string Publisher { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public IEnumerable<Category> Categories { get; set; }

        public string? SortOrder { get; set; }
        public string SearchTerm { get; set; }
    }
}
