using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyMVCProject.Models;

namespace MyMVCProject.DataAccess.Repository.IRepository
{
    public interface IReviewRepository : IRepository<Review>
    {
        
        void Update(Review obj);
    }
}
