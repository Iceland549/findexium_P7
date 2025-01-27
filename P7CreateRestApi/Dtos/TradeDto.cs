namespace P7CreateRestApi.Dtos
{
    public class TradeDto
    {
        public int TradeId { get; set; }
        public string? Account { get; set; }
        public string? AccountType { get; set; }
        public double? BuyQuantity { get; set; }
        public double? SellQuantity { get; set; }
        public double? BuyPrice { get; set; }
        public double? SellPrice { get; set; }
        public string? TradeSecurity { get; set; }
        public string? TradeStatus { get; set; }
        public string? Trader { get; set; }
        public string? Benchmark { get; set; }
        public string? Book { get; set; }
        public string? DealName { get; set; }
        public string? DealType { get; set; }
        public string? SourceListId { get; set; }
        public string? Side { get; set; }
    }
}
