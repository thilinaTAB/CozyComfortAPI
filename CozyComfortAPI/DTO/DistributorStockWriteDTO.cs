using System.ComponentModel.DataAnnotations;

namespace CozyComfortAPI.DTO
{
    public class DistributorStockWriteDTO
    {
        [Required]
        [Range(0, 999999, ErrorMessage = "Stock value must be non negative")]
        public int Inventory { get; set; }

        [Required]
        public int DistributorID { get; set; }

        [Required]
        public int ModelID { get; set; }    
    }
}
