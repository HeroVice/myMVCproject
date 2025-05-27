using MyMVCProject.Models;

namespace MyMVCProject.DataAccess.Repository.IRepository
{
    public interface IProductRepository : IRepository<Product>
    {
        Product GetProductWithImages(int id);

        Product GetProductWithCategory(int productId);


        void Update(Product obj);
    }
}