using todo_list.Models.DTO;
using todo_list.Models;
using System.Xml;

namespace todo_list.Repository.Interfaces
{
    public interface ITasksXML
    {
        public Task<IEnumerable<Tasks>> GetTasksAsync();
        public Task<Tasks?> GetTasksAsync(int id);
        public Task<IEnumerable<Categories>> GetCategoriesAsync();
        public Task CreateTasksAsync(TaskCreationModel item);
        public Task DeleteTasksAsync(int id);
        public Task CompleteTasksAsync(int id);
        public Task<Tasks> ParseXmlTasksAsync(XmlNode taskXml);
        public Task<Categories> ParseXmlCategoriesAsync(XmlNode categoryXml);
        public Task<int> FindNextIdAsync();
    }
}
