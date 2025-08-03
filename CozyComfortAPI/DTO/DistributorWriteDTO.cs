using System.ComponentModel.DataAnnotations;

namespace CozyComfortAPI.DTO
{
    public class DistributorWriteDTO
    {
        [Required]
        public string DistributorName { get; set; }
        [Required]
        public string Email { get; set; }
    }
}
