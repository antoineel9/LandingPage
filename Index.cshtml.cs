using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace NWI2.Pages

{
    public class IndexModel : PageModel

    {
        public string CurrrentUserRole { get; set; }
        public string CurrentUserRole { get; private set; }

        //public object CurrentUserRole { get; private set; }

        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            CurrentUserRole = "";
        }
    }
}