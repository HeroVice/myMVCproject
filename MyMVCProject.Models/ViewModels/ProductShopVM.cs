using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMVCProject.Models.ViewModels
{
    public class ProductShopVM
    {
        public Product Product { get; set; }
        public ShoppingCart ShoppingCart { get; set; }
        public bool IsInBasket { get; set; }
        public bool IsInLibrary { get; set; }
        public Review ExistingReview { get; set; }

    }
}
