using System.ComponentModel.DataAnnotations;
namespace todo_list.Models
{
    public class Tasks
    {
        [Key]
        public int id { get; set; }
        public int category_id { get; set; }
        [Required]
        public string name { get; set; } = null!;
        [Required]
        public DateTime? due_date { get; set; }
        [Required]
        public bool status { get; set; }
    }
}
