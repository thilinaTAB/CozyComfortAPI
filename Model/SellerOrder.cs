using CozyComfortAPI.Model;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CozyComfortAPI.Models
{
    public class SellerOrder
    {
        [Key]
        public int SellerOrderID { get; set; }

        [Required]
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        [Required]
        public int Quantity { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0, 999999, ErrorMessage = "Total cannot be negative.")]
        public decimal Total { get; set; }

        [Required]
        [MaxLength(50)]
        public string Status { get; set; } = "Pending";

        [Required]
        public int SellerID { get; set; }
        [ForeignKey("SellerID")]
        public Seller Seller { get; set; }

        // This links to DistributorStock instead of BlanketModel
        [Required]
        public int DistributorStockID { get; set; }
        [ForeignKey("DistributorStockID")]
        public DistributorStock DistributorStock { get; set; }
    }
}