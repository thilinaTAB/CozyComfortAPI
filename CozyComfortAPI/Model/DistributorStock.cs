using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CozyComfortAPI.Model
{
    public class DistributorStock
    {
        [Key]
        public int DistributorStockID { get; set; }

        [Required]
        [Range(0, 999999, ErrorMessage = "Stock value must be non-negative.")]
        public int Inventory { get; set; }


        [Required]
        public int DistributorID { get; set; }
        public Distributor Distributor { get; set; }


        [Required]
        public int ModelID { get; set; }
        public BlanketModel BlanketModel { get; set; }

        public DistributorStock() { }
    }
}
