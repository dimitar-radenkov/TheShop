namespace TheShop.Models
{
    public class OrderOffer
    {
        public int OrderId { get; set; }

        public int? OfferId { get; set; }

        public bool HasValidOffer => this.OfferId != null;
    }
}
