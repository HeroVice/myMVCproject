using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMVCProject.Models
{
    public class Company
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        [Required]
        public string CompanyName { get; set; }
    }
}
