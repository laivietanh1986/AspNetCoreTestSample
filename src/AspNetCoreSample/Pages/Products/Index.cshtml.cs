using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AspNetCoreSample.Data;

namespace AspNetCoreSample.Pages.Products
{
    public class IndexModel : PageModel
    {
        private readonly AspNetCoreSample.Data.ApplicationDbContext _context;

        public IndexModel(AspNetCoreSample.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Product> Product { get;set; }

        public async Task OnGetAsync()
        {
            Product = await _context.Products.ToListAsync();
        }
    }
}
