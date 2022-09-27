using AspNetRazor.Models;
using System.ComponentModel.DataAnnotations;

namespace AspNetRazor.Pages.WorkTasks
{
    public class WorkTaskNameAttribute : ValidationAttribute
    {
        public WorkTaskNameAttribute(string taskName)
            => TaskName = taskName;

        public string TaskName { get; }

        public string GetErrorMessage() =>
            $"The current name is exist. Set the other name";

        protected override ValidationResult? IsValid(
            object? value, ValidationContext validationContext)
        {
            var workTask = (WorkTask)validationContext.ObjectInstance;
            var taskName = (string)value!;

            if (WorkTaskDbHelper.WorkTasks.Exists(x => x.Name.Trim() == TaskName.Trim()))
            {
                return new ValidationResult(GetErrorMessage());
            }

            return ValidationResult.Success;
        }
    }
}
