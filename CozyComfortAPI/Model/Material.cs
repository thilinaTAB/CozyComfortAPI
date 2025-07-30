using System.ComponentModel.DataAnnotations;

namespace CozyComfortAPI.Model
{
    public class Material
    {
        [Key]
        public int MaterialID { get; set; }
        [Required]
        public string MaterialName { get; set; }
        public string Description { get; set; }
    }
}
