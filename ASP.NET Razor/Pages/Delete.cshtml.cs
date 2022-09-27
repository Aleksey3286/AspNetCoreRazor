using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AspNetRazor.Models;

namespace AspNetRazor.Pages
{
    public class DeleteModel : PageModel
    {
        [BindProperty]
        public WorkTask WorkTask { get; set; }

        public void OnGet(int? id)
        {
            if (id == null || WorkTaskDbHelper.WorkTasks == null)
            {
                throw new ArgumentNullException();
            }

            var worktask = WorkTaskDbHelper.WorkTasks.Where(m => m.Id == id).FirstOrDefault();

            if (worktask == null)
            {
                throw new ArgumentNullException();
            }
            else 
            {
                WorkTask = worktask;
            }
            Page();
        }

        public async Task<IActionResult>  OnPostAsync(int id)
        {
            if (id == null || WorkTaskDbHelper.WorkTasks == null)
            {
                return NotFound("You can delete task only with status 'completed'");
            }

            try
            {
                WorkTaskDbHelper.DeleteWorkTask(id);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }

            return RedirectToPage("Index");
        }
    }
}
