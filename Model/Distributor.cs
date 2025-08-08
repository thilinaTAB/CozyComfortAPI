using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace CozyComfortAPI.Model
{
    public class Distributor
    {
        [Key]
        public int DistributorID { get; set; }
        [Required]
        public string DistributorName { get; set; }
        [Required]
        public string Email { get; set; }

        public List<Order> Orders { get; set; } = new List<Order>();
        public List<DistributorStock> DistributorStocks { get; set; } = new List<DistributorStock>();

        public Distributor(int id, string name, string email)
        {
            DistributorID = id;
            DistributorName = name;
            Email = email;
        }

        public Distributor()
        {
        }
    }
}
