using System.ComponentModel.DataAnnotations;

namespace CozyComfortAPI.DTO
{
    public class SellerOrderWriteDTO
    {
        [Required]
        public int SellerId { get; set; }

        [Required]
        public int DistributorStockID { get; set; }

        [Required]
        [Range(1, 99999, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }
    }
}