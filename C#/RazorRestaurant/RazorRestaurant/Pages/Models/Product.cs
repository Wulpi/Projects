﻿using System.ComponentModel.DataAnnotations;

namespace RazorRestaurant.Pages.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public byte[]? image { get; set; }
    }
}
