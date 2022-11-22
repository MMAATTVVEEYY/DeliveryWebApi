using System.ComponentModel.DataAnnotations;

namespace DeliveryWebApi.Models
{
    public class Delivery
    {
        [Key]
        public int Id { get; set; }
        public string Address { get; set; }
        public int OrderId{get; set; }
        public DateTime Started { get; set; }
        public DateTime? Ended { get; set; }
        public bool? DeliveryReceived { get; set; }
    }
}
