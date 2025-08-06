using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CozyComfortAPI.Models
{
    public class SellerInventory
    {
        [Key]
        public int SellerInventoryId { get; set; }

        [Required]
        [ForeignKey("Seller")]
        public int SellerId { get; set; }

        [Required]
        public int BlanketModelId { get; set; }

        public int Quantity { get; set; }
    }
}