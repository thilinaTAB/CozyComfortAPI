namespace CozyComfortAPI.DTO
{
    public class OrderReadDTO
    {
        public int OrderID { get; set; }
        public DateTime OrderDate { get; set; }
        public string ModelName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Total { get; set; }
        public string Status { get; set; }  
    }
}
