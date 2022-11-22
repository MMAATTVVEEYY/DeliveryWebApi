using DeliveryWebApi.Data;
using DeliveryWebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;

namespace DeliveryWebApi.Controllers
{
    [Route("api/Delivery")]
    [ApiController]
    public class DeliveryAPIController : ControllerBase
    {
        private readonly DeliveryAPIDbContext _context;
        private readonly IHttpClientFactory _client;
        public DeliveryAPIController(DeliveryAPIDbContext context, IHttpClientFactory client)
        {
            _context = context;
            _client = client;
        }

        [HttpGet]
        public async Task<IEnumerable<Delivery>> Get()
        {
            return await _context.Deliveries.ToListAsync();
        }

        [HttpGet("Id")]
        [ProducesResponseType(typeof(Delivery), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int Id)
        {
            var Delivery = await _context.Deliveries.FindAsync(Id);
            return Delivery == null ? NotFound() : Ok(Delivery);
        }


        [HttpPost]
        public async Task<IActionResult> Post(Delivery NewDelivery)
        {
            //Убедиться, что на этот ордер нет доставки
            var PotentialConflictDeliveries =  _context.Deliveries.Where(x => x.OrderId == NewDelivery.OrderId).ToList();
            if (PotentialConflictDeliveries.Count != 0)
                {
                    return Conflict("This order already has delivery");
                }
            // посмотреть в ордерах, нужна ли этому ордеру доставка
            var httpClient = _client.CreateClient("Orders");
            var GetOrderRequest = new HttpRequestMessage(HttpMethod.Get, $"api/Order/Id?Id={NewDelivery.OrderId}");
            GetOrderRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var GetOrderResponce = await httpClient.SendAsync(GetOrderRequest);

            if (GetOrderResponce.IsSuccessStatusCode)
            {
                var Order = await GetOrderResponce.Content.ReadFromJsonAsync<OrderDTO>();
                if (!Order.NeedsDelivery)
                {
                    return Conflict($"Order does not need Delivery, cant POST a delivery");
                }
                //Создаем Delivery
                NewDelivery.Ended = null;
                NewDelivery.DeliveryReceived = null;
                _context.Deliveries.Add(NewDelivery);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(Post), NewDelivery);
            }

            return NotFound("No such order");
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var DeliveryToDelete = await _context.Deliveries.FindAsync(id);
            if (DeliveryToDelete == null)
            {
                return NotFound();
            }
            _context.Deliveries.Remove(DeliveryToDelete);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("id")]
        public async Task<IActionResult> Post(int id, Delivery UpdatedDelivery)
        {
            var DeliveryToUpdate = await _context.Deliveries.FindAsync(id);
            if (DeliveryToUpdate == null)
            {
                return NotFound();
            }
            DeliveryToUpdate.Address= UpdatedDelivery.Address;
            DeliveryToUpdate.Ended= UpdatedDelivery.Ended;
            DeliveryToUpdate.Started= UpdatedDelivery.Started;
            DeliveryToUpdate.DeliveryReceived= UpdatedDelivery.DeliveryReceived;
            // Изменить orderId - привязать доставку кдругому заказу.
            // 1 заказ - 1 доставка нет смысла менять OrderId
            //DeliveryToUpdate.OrderId = UpdatedDelivery.OrderId;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPatch("UpdatAddress/id")]
        public async Task<IActionResult> UpdatAddress(int id, string NewAddress)
        {
            var DeliveryToUpdate = await _context.Deliveries.FindAsync(id);
            if (DeliveryToUpdate == null)
            {
                return NotFound();
            }
            DeliveryToUpdate.Address = NewAddress;
            await _context.SaveChangesAsync();
            return NoContent();
        }


    }
}
