namespace CozyComfortAPI.DTO
{
    public class ModelReadDTO
    {
        public int ModelID { get; set; }
        public string ModelName { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public int Stock { get; set; }

        public int MaterialID { get; set; }
        public string MaterialName { get; set; }
        public string MaterialDescription { get; set; }
    }
}
