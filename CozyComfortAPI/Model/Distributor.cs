using System.ComponentModel.DataAnnotations;

namespace CozyComfortAPI.Model
{
    public class Distributor
    {
        [Key]
        public int DistributorID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]

        public List<Order> Orders { get; set; } = new List<Order>();

        public Distributor(int id, string name, string email)
        {
            DistributorID = id;
            Name = name;
            Email = email;
        }

        public Distributor()
        {

        }
    }
}
