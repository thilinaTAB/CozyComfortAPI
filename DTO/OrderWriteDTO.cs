using System.ComponentModel.DataAnnotations;

namespace CozyComfortAPI.DTO
{
    public class OrderWriteDTO
    {
        [Required]
        public int ModelID { get; set; }
        [Required]
        public int Quantity { get; set; }
    }
}
