using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace CozyComfortAPI.Models
{
    public class Seller
    {
        [Key]
        public int SellerId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string ContactEmail { get; set; }

        public List<SellerOrder> Orders { get; set; } = new List<SellerOrder>();
        public List<SellerInventory> Inventory { get; set; } = new List<SellerInventory>();

        public Seller(int id, string name, string contactEmail, string apiSecretKey)
        {
            SellerId = id;
            Name = name;
            ContactEmail = contactEmail;
        }

        public Seller()
        {
        }
    }
}