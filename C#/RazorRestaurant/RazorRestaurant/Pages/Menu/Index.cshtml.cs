using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RazorRestaurant.Data;
using RazorRestaurant.Pages.Models;

namespace RazorRestaurant.Pages.Menu
{
    public class IndexModel : PageModel
    {
        private readonly RazorRestaurant.Data.RazorRestaurantContext _context;

        public IndexModel(RazorRestaurant.Data.RazorRestaurantContext context)
        {
            _context = context;
        }

        public IList<Product> Product { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Product != null)
            {
                Product = await _context.Product.ToListAsync();
            }
        }
    }
}
