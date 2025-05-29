using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyMVCProject.DataAccess.Data;
using MyMVCProject.DataAccess.Repository.IRepository;
using MyMVCProject.Models;

namespace MyMVCProject.DataAccess.Repository
{
    public class WishlistRepository : Repository<Wishlist>, IWishlistRepository
    {
        private ApplicationDbContext _db;
        public WishlistRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Wishlist obj)
        {
            _db.Wishlists.Update(obj);
        }
    }
}
