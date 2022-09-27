using AspNetRazor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AspNetRazor.Pages
{
    public class CreateModel : PageModel
    {        
        [BindProperty]
        public int SelectedStatusId { set; get; }

        public List<SelectListItem> StatusItems { set; get; }

        public List<string> WorkTaskNames { set; get; }

        [BindProperty]
        public WorkTask WorkTask { get; set; }
        
        public IActionResult OnGet()
        {
            StatusItems = WorkTaskDbHelper.Statuses
                                .Select(a => new SelectListItem
                                {
                                    Value = a.Id.ToString(),
                                    Text = a.Name
                                })
                               .ToList();

            SelectedStatusId = WorkTaskDbHelper.Statuses[0].Id;

            return Page();
        }

       public async Task<IActionResult> OnPostAsync()
        {
          

            WorkTask.StatusId = this.SelectedStatusId;
            WorkTask.StatusName = WorkTaskDbHelper.Statuses.FirstOrDefault(x => x.Id == this.SelectedStatusId).Name;

            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                WorkTaskDbHelper.InsertWorkTask(WorkTask);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }

            return RedirectToPage("Index");
        }
    }
}
