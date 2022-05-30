using System;
using Microsoft.AspNetCore.Mvc;

namespace customer.Controllers
{
    public class CheckoutController : Controller
    {
        public class CheckoutInfo
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Country { get; set; }
            public string Street { get; set; }
            public string Number { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public string Phone { get; set; }
            public string Note { get; set; }
            public string TotalCost { get; set; }
            public string ProductsInCart { get; set; }
        }
        
        [HttpPost]
        [Route("DoCheckout")]
        public IActionResult DoCheckOut([FromBody] CheckoutInfo checkoutInfo)
        {
            Console.WriteLine(checkoutInfo);
            
            return null;
        }
    }
}