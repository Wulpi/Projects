using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RazorRestaurant.Pages.Models;

namespace RazorRestaurant.Data
{
    public class RazorRestaurantContext : DbContext
    {
        public RazorRestaurantContext (DbContextOptions<RazorRestaurantContext> options)
            : base(options)
        {
        }

        public DbSet<RazorRestaurant.Pages.Models.Product> Product { get; set; } = default!;
    }
}
