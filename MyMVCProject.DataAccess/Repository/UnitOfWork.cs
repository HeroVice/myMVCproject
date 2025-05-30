﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyMVCProject.DataAccess.Data;
using MyMVCProject.DataAccess.Repository.IRepository;

namespace MyMVCProject.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _db;
        public IProductRepository Product { get; private set; }
        public ICategoryRepository Category { get; private set; }
        public IProductImageRepository ProductImage { get; private set; }
        public IShoppingCartRepository ShoppingCart { get; private set; }
        public ICompanyRepository Company { get; private set; }
        public IOrderDetailRepository OrderDetail { get; private set; }
        public ILibraryRepository Library { get; private set; }
        public IReviewRepository Review { get; private set; }
        public IWishlistRepository Wishlist { get; private set; }

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Product = new ProductRepository(_db);
            Category = new CategoryRepository(_db);
            ProductImage = new ProductImageRepository(_db);
            ShoppingCart = new ShoppingCartRepository(_db);
            Company = new CompanyRepository(_db);
            OrderDetail = new OrderDetailRepository(_db);
            Library = new LibraryRepository(_db);
            Review = new ReviewRepository(_db);
            Wishlist = new WishlistRepository(_db);
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
