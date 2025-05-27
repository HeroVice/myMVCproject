using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace MyMVCProject.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        [Range(1, 120, ErrorMessage = "List Price must be between 1 and 120.")]
        public decimal Price { get; set; }
        [Required]
        [Range(1, 120, ErrorMessage = "List Price must be between 1 and 120.")]
        public decimal ListPrice { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        [ValidateNever]
        public Category Category { get; set; }
        public string Publisher { get; set; }
        public int PublishDate { get; set; }
        [DisplayName("image")]
        [ValidateNever]
        public List<ProductImage> ProductImages { get; set; }

        [Range(0, 100)]
        public int? DiscountPercent { get; set; } // İndirim yüzdesi, nullable çünkü indirim olmayabilir

        [Range(0, 5.0)]
        public decimal? Rating { get; set; } // 0-5 arası puan, nullable olabilir

    }
}
