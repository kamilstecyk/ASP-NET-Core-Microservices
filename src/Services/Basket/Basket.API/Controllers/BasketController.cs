using Basket.API.Entities;
using Basket.API.GrpcServices;
using Basket.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Basket.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _reposository;
        private readonly DiscountGrpcService _discountGrpcService;

        public BasketController(IBasketRepository reposository, DiscountGrpcService discountGrpcService)
        {
            _reposository = reposository ?? throw new ArgumentException(nameof(reposository));
            _discountGrpcService = discountGrpcService ?? throw new ArgumentException(nameof(discountGrpcService));
        }

        [HttpGet("{userName}", Name = "GetBasket")]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> GetBasket(string userName)
        {
            var basket = await _reposository.GetBasket(userName);
            return Ok(basket ?? new ShoppingCart(userName));
        }

        [HttpPost]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> UpdateBasket([FromBody] ShoppingCart basket)
        {
            // Communicate with Discount.Grpc and calculate latest prices of products in the shopping cart
            // consume Discount Grpc 

            foreach (var item in basket.Items) 
            {
                var coupon = await _discountGrpcService.GetDiscount(item.ProductName);
                item.Price -= coupon.Amount;
            }

            return Ok(await _reposository.UpdateBasket(basket));
        }

        [HttpDelete("{username}", Name ="DeleteBasket")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteBasket(string username)
        {
            await _reposository.DeleteBasket(username);
            return Ok();
        }
    }
}
