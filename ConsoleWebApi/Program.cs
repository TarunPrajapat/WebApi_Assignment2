using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using WebApiDay1.Models;
using WebApiDay1.Controllers;

namespace ConsoleWebApi
{
    class Program
    {
        static void Main(string[] args)
        {
            GetProduct().Wait();
            Console.WriteLine("Enter Id");
            int id = int.Parse(Console.ReadLine());
            GetProductById(id).Wait();
            Console.WriteLine("Please Enter Required data");
            Product product = new Product();
            Console.WriteLine("Enter id");
            product.Id = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter Name");
            product.Name = Console.ReadLine();
            Console.WriteLine("Enter quantity");
            product.QtyInStock = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter Description");
            product.Description = Console.ReadLine();
            Console.WriteLine("Enter Supplier");
            product.Supplier = Console.ReadLine();
            Insert(product).Wait();
            Console.WriteLine("Product Added Successfully!");
            GetProduct().Wait();
      
            Console.WriteLine("Enter the id to Update Record");
            int Id = int.Parse(Console.ReadLine());
            Product p = new Product();
            Console.WriteLine("Enter updated Name");
            p.Name = Console.ReadLine();
            Console.WriteLine("Enter updated quantity");
            p.QtyInStock = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter updated Description");
            p.Description = Console.ReadLine();
            Console.WriteLine("Enter updated Supplier");
            p.Supplier = Console.ReadLine();

            Put(Id,p).Wait();
            GetProduct().Wait();

            Console.WriteLine("Enter id to Delete");
            int id1 = int.Parse(Console.ReadLine());
            Delete(id1).Wait();
            Console.WriteLine("Data Deleted");
            GetProduct().Wait();
        }
        static async Task GetProduct()
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
                    foreach (var temp in product)
                    {
                        Console.WriteLine("Id:{0}\tName:{1}", temp.Id, temp.Name);

                    }
                }
                else
                {
                    Console.WriteLine(response.ReasonPhrase);
                    Console.WriteLine("Internal server Error");
                }
            }
        }
        static async Task GetProductById(int id)
        {
            using (var client=new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44372/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync("api/Product/"+id);
                if (response.IsSuccessStatusCode)
                {
                    Product product = await response.Content.ReadAsAsync<Product>();
                    Console.WriteLine("Id:{0}\tName:{1}", product.Id, product.Name);
                }
                else
                {
                    Console.WriteLine(response.ReasonPhrase);
                    Console.WriteLine("Internal server Error");
                }
            }
        }
        static async Task Insert(Product product)
        {
            using (var client=new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44372/");
                HttpResponseMessage response = await client.PostAsJsonAsync("api/Product", product);
                if(response.IsSuccessStatusCode)
                {
                    Uri returnUrl = response.Headers.Location;
                    Console.WriteLine(returnUrl);
                }
            }
        }
        static async Task Put(int id,Product product)
        {
            using(var client=new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44372/");
                HttpResponseMessage response = await client.PutAsJsonAsync("api/Product/"+id, product);
                 if(response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Data Modified");
                }
            }
        }
        static async Task Delete(int id)
        {
            using(var client=new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44372/");
                HttpResponseMessage response = await client.DeleteAsync("api/Product" + id);
                if(response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Success");
                }
            }
        }
    }
}
