using todo_list.Models;

namespace todo_list.Models.DTO
{
    public class TasksAndCategoriesViewModel
    {
        public IEnumerable<Tasks> Tasks { get; set; } = new List<Tasks>();
        public IEnumerable<Categories> Categories { get; set; } = new List<Categories>();
    }
}