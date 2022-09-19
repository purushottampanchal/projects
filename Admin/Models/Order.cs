namespace Admin.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string FoodItemName { get; set; }
        public int Qty { get; set; }
        public int TotalCost { get; set; }
        public string OrderStatus { get; set; }

    }
}
