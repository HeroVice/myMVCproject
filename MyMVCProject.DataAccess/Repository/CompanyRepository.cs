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
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        private readonly ApplicationDbContext _db;
        public CompanyRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Company obj)
        {
            var objFromDb = _db.Companies.FirstOrDefault(u => u.Id == obj.Id);
            if (objFromDb != null)
            {
                objFromDb.CompanyName = obj.CompanyName;
                objFromDb.UserId = obj.UserId;
                objFromDb.Id = obj.Id;
            }
        }
    }
}
