using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace todo_list.Models.DTO
{
    public class TaskCreationModel
    {
        public int category_id { get; set; }

        [Required]
        public string name { get; set; } = null!;

        [Required]
        public DateTime? due_date { get; set; } = null!;
    }
}