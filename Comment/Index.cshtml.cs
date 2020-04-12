using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NWI2.Data;

namespace NWI2
{
    public class IndexModel1 : PageModel
    {
        private readonly NWI2.Data.ApplicationDbContext _context;

        public IndexModel1(NWI2.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<GED> GED { get; set; }

        public async Task OnGetAsync()
        {
            GED = await _context.GED.ToListAsync();
        }
    }
}