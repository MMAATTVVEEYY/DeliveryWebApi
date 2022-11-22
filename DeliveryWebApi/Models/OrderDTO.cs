using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DeliveryWebApi.Models
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public bool NeedsDelivery { get; set; }
        public bool ReadyToPickUp { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Done { get; set; }
    }

    
}
