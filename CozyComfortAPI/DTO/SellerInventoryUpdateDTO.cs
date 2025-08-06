// File: SellerInventoryUpdateDTO.cs
using System.ComponentModel.DataAnnotations;

namespace CozyComfortAPI.DTO
{
    public class SellerInventoryUpdateDTO
    {
        [Required]
        public int SellerId { get; set; }

        [Required]
        public int BlanketModelId { get; set; }

        [Required]
        [Range(0, 99999, ErrorMessage = "Quantity must be non-negative.")]
        public int Quantity { get; set; }
    }
}