using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMVCProject.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IProductRepository Product { get; }
        ICategoryRepository Category { get; }
        IProductImageRepository ProductImage { get; }

        IShoppingCartRepository ShoppingCart { get; }

        void Save();
    }
}
