using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using RazorRestaurant.Data;
using RazorRestaurant.Pages.Models;

namespace RazorRestaurant.Pages.Menu
{
    public class CreateModel : PageModel
    {
        private readonly RazorRestaurant.Data.RazorRestaurantContext _context;

        public CreateModel(RazorRestaurant.Data.RazorRestaurantContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Product Product { get; set; } = default!;
        public IFormFile? photoFile { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || _context.Product == null || Product == null)
            {
                return Page();
            }

            using MemoryStream ms = new MemoryStream();
            photoFile.CopyTo(ms);
            Product.image = ms.ToArray();

            _context.Product.Add(Product);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

    }
}
