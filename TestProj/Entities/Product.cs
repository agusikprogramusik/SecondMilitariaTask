namespace TestProj.Entities
{
    public class Product
    {
        public string Id { get; set; }
        public string? ProductCode { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public List<string>? Photos { get; set; } = new List<string>();
        public decimal? Price { get; set; }
        public int? StockQuantity { get; set; }
        public bool IsFlagged { get; set; } = false;
    }
}
