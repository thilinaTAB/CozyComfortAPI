namespace CozyComfortAPI.DTO
{
    public class DistributorStockReadDTO
    {
        public int DistributorStockID { get; set; }
        public int Inventory { get; set; }

        public int DistributorID { get; set; }

        public int ModelID { get; set; }
        public ModelReadDTO BlanketModel { get; set; }
    }
}
