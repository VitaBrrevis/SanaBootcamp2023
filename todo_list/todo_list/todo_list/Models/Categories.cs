using System.ComponentModel.DataAnnotations;

namespace todo_list.Models
{
    public class Categories
    {
        [Key]
        public int id { get; set; }

        [Required]
        public string name { get; set; } = null!;
    }
}
