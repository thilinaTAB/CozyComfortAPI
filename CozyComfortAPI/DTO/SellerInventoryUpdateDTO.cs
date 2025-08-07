// Inside CozyComfortAPI/DTO/SellerInventoryUpdateDTO.cs
using System.ComponentModel.DataAnnotations;

namespace CozyComfortAPI.DTO
{
    public class SellerInventoryUpdateDTO
    {
        // Add this property for the PUT and GET requests
        public int InventoryId { get; set; }

        [Required]
        public int SellerId { get; set; }

        [Required]
        public int BlanketModelId { get; set; }

        [Required]
        [Range(0, 99999, ErrorMessage = "Quantity must be non-negative.")]
        public int Quantity { get; set; }

        // Add these new properties for displaying data
        public string ModelName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string MaterialName { get; set; } = string.Empty;
        public string MaterialDescription { get; set; } = string.Empty;
    }
}