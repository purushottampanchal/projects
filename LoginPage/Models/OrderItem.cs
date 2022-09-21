namespace LoginPage.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Qty { get; set; }
        public int Cost { get; set; }
        public string UserName { get; set; }
        public string Address { get; set; }
        public string OrderStatus { get; set; }

    }
}
