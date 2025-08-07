using System;

namespace CozyComfortAPI.DTO
{
    public class SellerOrderReadDTO
    {
        public int SellerOrderID { get; set; }
        public DateTime OrderDate { get; set; }
        public string ModelName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Total { get; set; }
        public string Status { get; set; }
    }
}