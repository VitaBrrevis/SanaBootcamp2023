using todo_list.Models.DTO;
using todo_list.Models;

namespace todo_list.Repository.Interfaces
{
    public interface ITasks
    {
        public Task<IEnumerable<Tasks>> GetTasksAsync();
        public Task<Tasks> GetTasksAsync(int id);
        public Task<IEnumerable<Categories>> GetCategoriesAsync();
        public Task CreateTasksAsync(TaskCreationModel item);
        public Task DeleteTasksAsync(int id);
        public Task CompleteTasksAsync(int id);
    }
}