using Microsoft.AspNetCore.Mvc;
using System;

namespace LibraryApi.Controllers
{
    public class MenuController : Controller
    {
        // GET /tacos
        //Must be a public class
        [HttpGet("/tacos")]
        public IActionResult OrderTacos()
        {
            var response = new MenuModel
            {
                Description = "A taco",
                Price = 3.10M,
                PriceGoodUntil = DateTime.Now.AddDays(2)
            };

            //What gets returned is sent to the client
            return Ok(response);
        }

        public class MenuModel
        {
            public string Description { get; set; }
            public decimal Price { get; set; }
            public DateTime PriceGoodUntil { get; set; }
        }
    }
}
