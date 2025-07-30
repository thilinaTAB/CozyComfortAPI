using System.ComponentModel.DataAnnotations;

namespace CozyComfortAPI.Model
{
    public class Order
    {
        [Key]
        public int OrderID { get; set; }
        [Required]
        public string Date { get; set; }
        [Range(0.00, 99999999.00, ErrorMessage = "Discount must be non negative")]
        public decimal Discount { get; set; }
        [Required]
        [Range(0.00, 99999999.00, ErrorMessage = "Total must be non negative")]
        public decimal Total { get; set; }

        public List<BlanketModel> BlanketModels { get; set; } = new List<BlanketModel>();
        public Distributor ByDistributor { get; set; } = new Distributor();
    }
}
