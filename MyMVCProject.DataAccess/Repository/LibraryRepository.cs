using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MyMVCProject.DataAccess.Data;
using MyMVCProject.DataAccess.Repository;
using MyMVCProject.DataAccess.Repository.IRepository;
using MyMVCProject.Models;

namespace MyMVCProject.DataAccess.Repository
{
    public class LibraryRepository : Repository<Library>, ILibraryRepository
    {
        private ApplicationDbContext _db;
        public LibraryRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Library obj)
        {
            _db.Libraries.Update(obj);
        }
    }
}
