using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CozyComfortAPI.Model
{
    public class Order
    {
        [Key]
        public int OrderID { get; set; }

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
        public int DistributorID { get; set; }
        [ForeignKey("DistributorID")]
        public Distributor Distributor { get; set; }

        [Required]
        public int ModelID { get; set; }
        [ForeignKey("ModelID")]
        public BlanketModel BlanketModel { get; set; }
    }
}
