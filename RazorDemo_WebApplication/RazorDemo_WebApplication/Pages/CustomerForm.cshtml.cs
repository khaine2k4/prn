using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorDemo_WebApplication.Models;

namespace RazorDemo_WebApplication.Pages
{
    public class CustomerFormModel : PageModel
    {
        [BindProperty]
        public Customer CustomerInfo { get; set; } = new();

        public string? Message { get; set; }

        public void OnGet()
        {

            if (ModelState.IsValid)
            {
                Message = "Information is OK";
                // nếu muốn clear form sau submit:
                // ModelState.Clear();
            }
            else
            {
                Message = "Error on input data.";
            }
        }

    }
}
