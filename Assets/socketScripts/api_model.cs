using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace Assets
{
    public class Order
    {
        public string _id { get; set; }
        public User user { get; set; }
        public Restaurant restaurant { get; set; }
        public List<Product> products { get; set; }
        public int totalAmount { get; set; }
        public string status { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
    }

    public class Product
    {
        public Product2 product { get; set; }
        public string _id { get; set; }
    }

    public class Product2
    {
        public string _id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int price { get; set; }
    }

    public class Restaurant
    {
        public string _id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
    }

    public class OrdersData
    {
        public List<Order> orders { get; set; }
    }

    public class User
    {
        public string _id { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string role { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public bool isVerified { get; set; }
    }

}