using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMVCProject.Models
{
    public class CompanyRequest
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Email { get; set; }
        public DateTime RequestDate { get; set; }
        public bool IsApproved { get; set; }
    }
}
