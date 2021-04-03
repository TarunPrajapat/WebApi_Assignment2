using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApiDay1.Models;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;

namespace AspNetWebApi.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        public async Task<ActionResult> Index()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44372/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync("api/Product");
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = response.Content.ReadAsStringAsync();
                    jsonString.Wait();

                    var product = JsonConvert.DeserializeObject<List<Product>>(jsonString.Result);
                    return View(product.ToList());
                }
                else
                {
                    return RedirectToAction("");
                }
            }
        }
        public async Task<ActionResult> Details(int id)
        {
            Product product = null;
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:44372/");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = await client.GetAsync("api/Product/" + id);
                    if (response.IsSuccessStatusCode)
                    {
                        product = await response.Content.ReadAsAsync<Product>();
                        return View(product);
                    }

                }
                return View(product);
         
 
        }  
        public async Task<ActionResult> Create()
        {

            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Create(Product product)
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44372/");
                HttpResponseMessage response = await client.PostAsJsonAsync("api/Product", product);
                if (response.IsSuccessStatusCode)
                {
                    Uri returnUrl = response.Headers.Location;
                }
            }
            return RedirectToAction("Index");
        }
        public async Task<ActionResult> Put(int id)
        {
            Product product = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44372/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync("api/Product/" + id);
                if (response.IsSuccessStatusCode)
                {
                    product = await response.Content.ReadAsAsync<Product>();
                    return View(product);
                }

            }
            return View(product);
        }
        [HttpPost]
        public async Task<ActionResult> Put(int id,Product product)
        {
            using (var client = new HttpClient())
            {
                //Send HTTP requests from here. 
                client.BaseAddress = new Uri("https://localhost:44372/");

            
                HttpResponseMessage response = await client.PutAsJsonAsync("api/Product/" + id,product);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                 }
            }
            return RedirectToAction("Index");

        }
        public async Task<ActionResult> Delete(int id)
        {
            using (var client = new HttpClient())
            {
                //Send HTTP requests from here. 
                client.BaseAddress = new Uri("https://localhost:44372/");
                HttpResponseMessage response = await client.DeleteAsync("api/Product/" + id);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }

            return RedirectToAction("Index");
        }


    }
}