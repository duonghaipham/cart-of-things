using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BraintreeHttp;
using customer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PayPal.Core;
using PayPal.v1.Payments;
using Order = customer.Models.Order;

namespace customer.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly string _clientId;
        private readonly string _secret;
        
        public CheckoutController(IConfiguration configuration)
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.Dev.json").Build().GetSection("Paypal");
            _clientId = config["ClientId"];
            _secret = config["Secret"];
        }
        
        [HttpGet]
        [Route("Checkout")]
        public IActionResult Checkout()
        {
            if (HttpContext.Session.GetInt32("id") == null)
            {
                return Redirect("SignIn");
            }
            
            return View();
        }
        
        [HttpPost]
        [Route("Checkout")]
        public async Task<IActionResult> Checkout(string name, string country, string street, string number, string city, string phone, string note, string products)
        {
            if (HttpContext.Session.GetInt32("id") == null)
            {
                return Redirect("SignIn");
            }
            
            int idUser = (int) HttpContext.Session.GetInt32("id");
            
            var productsInCart = JsonConvert.DeserializeObject(products) as JArray;
            
            var environment = new SandboxEnvironment(_clientId, _secret);
            var client = new PayPalHttpClient(environment);

            var itemList = new ItemList
            {
                Items = new List<Item>()
            };

            string total = productsInCart?
                .Sum(product => product.Value<int>("price") * product.Value<int>("inCart"))
                .ToString();
            
            Order order = Order.Create(name, country, street, number, city, note, phone, idUser, float.Parse(total ?? "0"));
            int orderId = order.Id;

            foreach (var product in productsInCart)
            {
                var item = new Item
                {
                    Name = product["name"].ToString(),
                    Currency = "USD",
                    Price = product["price"].ToString(),
                    Quantity = product["inCart"].ToString()
                };
                itemList.Items.Add(item);

                Ware.AddWareToOrder((int)product["inCart"], (int)product["id"], orderId);
            }

            var paypalOrderId = DateTime.Now.Ticks;
            var hostname = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
            var payment = new Payment()
            {
                Intent = "sale",
                Transactions = new List<Transaction>()
                {
                    new Transaction
                    {
                        Amount = new Amount
                        {
                            Total = total,
                            Currency = "USD"
                        },
                        ItemList = itemList,
                        Description = $"Invoice #{paypalOrderId}",
                        InvoiceNumber = paypalOrderId.ToString()
                    }
                },
                RedirectUrls = new RedirectUrls
                {
                    CancelUrl = $"{hostname}/Checkout/Failed",
                    ReturnUrl = $"{hostname}/Checkout/Success"
                },
                Payer = new Payer
                {
                    PaymentMethod = "paypal"
                }
            };

            PaymentCreateRequest request = new PaymentCreateRequest();
            request.RequestBody(payment);
            request.Headers.Add("Access-Control-Allow-Origin", "https://www.sandbox.paypal.com");

            try
            {
                var response = await client.Execute(request);
                var statusCode = response.StatusCode;
                Payment result = response.Result<Payment>();
                
                Order.UpdatePaymentId(orderId, result.Id);

                var links = result.Links.GetEnumerator();
                string paypalRedirectUrl = null;
                while (links.MoveNext())
                {
                    LinkDescriptionObject lnk = links.Current;
                    if (lnk.Rel.ToLower().Trim().Equals("approval_url"))
                    {
                        paypalRedirectUrl = lnk.Href;
                    }
                }

                return Redirect(paypalRedirectUrl);
            }
            catch (HttpException httpException)
            {
                Console.WriteLine(httpException.Message);
                
                return Redirect("/Checkout/Failed");
            }
        }
        
        [HttpGet]
        [Route("Checkout/Success")]
        public IActionResult Success()
        {
            ViewBag.Message = "Payment Successful!";
            string paymentId = HttpContext.Request.Query["paymentId"].ToString();
            
            Order.UpdatePaymentState(paymentId, "success");

            return View();
        }
        
        [HttpGet]
        [Route("Checkout/Failed")]
        public IActionResult Failed()
        {
            ViewBag.Message = "Payment Failed!";
            string paymentId = HttpContext.Request.Query["paymentId"].ToString();
            Order.UpdatePaymentState(paymentId, "pending");
            
            return View();
        }

        [HttpGet]
        [Route("Orders")]
        public IActionResult Orders()
        {
            if (HttpContext.Session.GetInt32("id") == null)
            {
                return Redirect("SignIn");
            }

            int idUser = (int)HttpContext.Session.GetInt32("id");
            string jsonOrders = Order.GetOrdersByUserId(idUser);

            ViewData["jsonOrders"] = jsonOrders;

            return View();
        }
    }
}