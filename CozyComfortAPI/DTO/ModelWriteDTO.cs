using System.ComponentModel.DataAnnotations;
namespace CozyComfortAPI.DTO
{
    public class ModelWriteDTO
    {
        [Required]
        public string ModelName { get; set; }
        [Required]
        [Range(0.00, 99999999.00, ErrorMessage = "Price must be non negative")]
        public decimal Price { get; set; }
        public string Description { get; set; }
        [Required]
        [Range(0, 999999, ErrorMessage = "Stock value must be non negative")]
        public int Stock { get; set; }

        [Required]
        public int MaterialID { get; set; }
    }
}
