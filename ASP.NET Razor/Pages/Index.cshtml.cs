using AspNetRazor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AspNetRazor.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public IEnumerable<WorkTask> WorkTasks { get; set; } = default!;

        [BindProperty]
        public int SelectedStatusId { set; get; }

        public List<SelectListItem> StatusItems { set; get; }

        public List<string> WorkTaskNames { set; get; }

        public int StatusId { get; set; }

        public void OnGet()
        {
            WorkTaskDbHelper.ReadWorkTasks();

            WorkTasks = WorkTaskDbHelper.WorkTasks;

            WorkTaskNames = WorkTaskDbHelper.WorkTasks.Select(x => x.Name).ToList();

            StatusItems = WorkTaskDbHelper.Statuses
                                .Select(a => new SelectListItem
                                {
                                    Value = a.Id.ToString(),
                                    Text = a.Name
                                })
                               .ToList();
        }
    }
}