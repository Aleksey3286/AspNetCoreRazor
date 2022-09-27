using System.ComponentModel.DataAnnotations;

namespace AspNetRazor
{
    public class WorkTask
    {
        [Key]
        public int Id { get; set; }
                
        [Required(ErrorMessage = "Name is not entered")]
        public string Name { get; set; }

        [Range(1,20, ErrorMessage = "The value must be in range from 1 to 20")]
        public int Priority { get; set; }

        [Required(ErrorMessage = "Status is not selected")]
        public int StatusId { get; set; }

        public string? StatusName { get; set; }

        public WorkTask(int id, string name, int priority, int status)
        {
            Id = id;
            Name = name;
            Priority = priority;
            StatusId = status;
        }

        public WorkTask(int id, string name, int priority, int status, string statusName)
        {
            Id = id;
            Name = name;
            Priority = priority;
            StatusId = status;
            StatusName = statusName;
        }

        public WorkTask()
        { }
    }
}