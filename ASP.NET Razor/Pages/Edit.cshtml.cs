using AspNetRazor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AspNetRazor.Pages
{
    public class EditModel : PageModel
    {
        [BindProperty]
        public int SelectedStatusId { set; get; }

        public List<SelectListItem> StatusItems { set; get; }

        public List<string> WorkTaskNames { set; get; }

        [BindProperty]
        public WorkTask WorkTask { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            WorkTask = WorkTaskDbHelper.WorkTasks.FirstOrDefault(m => m.Id == id);

            StatusItems = WorkTaskDbHelper.Statuses
                                .Select(a => new SelectListItem
                                {
                                    Value = a.Id.ToString(),
                                    Text = a.Name
                                })
                               .ToList();

            WorkTaskNames = WorkTaskDbHelper.WorkTasks.Select(x => x.Name).ToList();

            SelectedStatusId = WorkTask.StatusId;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            WorkTask.StatusId = this.SelectedStatusId;
            WorkTask.StatusName = WorkTaskDbHelper.Statuses.FirstOrDefault(x => x.Id == this.SelectedStatusId).Name;

            try
            {
                WorkTaskDbHelper.UpdateWorkTask(WorkTask);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }
            
            return RedirectToPage("Index");
        }
    }
}
