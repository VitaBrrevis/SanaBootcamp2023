using Dapper;
using System.Data;
using todo_list.Context;
using todo_list.Models.DTO;
using todo_list.Models;
using todo_list.Repository.Interfaces;
using System.Xml;
using System.Globalization;

namespace todo_list.Repository
{
    public class TasksRepoXML : ITasksXML
    {
        private readonly string pathTasks;
        private readonly string pathCategories;

        public TasksRepoXML(IWebHostEnvironment env)
        {
            pathTasks = string.Concat(env.WebRootPath, "/Storage/Tasks.xml");
            pathCategories = string.Concat(env.WebRootPath, "/Storage/Categories.xml");
        }

        public async Task CompleteTasksAsync(int id)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(pathTasks);
            var taskXml = (XmlElement?)doc.SelectSingleNode($"/Tasks/Task[@id={id}]");
            if (taskXml != null)
            {
                bool oldValue = bool.Parse(taskXml.GetAttribute("status"));
                taskXml.SetAttribute("status", (!oldValue).ToString());
                doc.Save(pathTasks);
            }
        }

        public async Task CreateTasksAsync(TaskCreationModel item)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(pathTasks);

            XmlElement taskXml = doc.CreateElement("Task");
            int id = await FindNextIdAsync();
            taskXml.SetAttribute("id", id.ToString());
            taskXml.SetAttribute("category_id", item.category_id.ToString());
            taskXml.SetAttribute("name", item.name);
            taskXml.SetAttribute("due_date", item.due_date.ToString());
            taskXml.SetAttribute("status", false.ToString());

            doc.DocumentElement.AppendChild(taskXml);
            doc.Save(pathTasks);
        }

        public async Task DeleteTasksAsync(int id)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(pathTasks);
            var taskXml = doc.SelectSingleNode($"/Tasks/Task[@id={id}]");

            if (taskXml != null)
            {
                doc.DocumentElement.RemoveChild(taskXml);
                doc.Save(pathTasks);
            }
        }

        public async Task<IEnumerable<Categories>> GetCategoriesAsync()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(pathCategories);
            XmlNodeList? categoriesXml = doc.SelectNodes("/Categories/Category");

            var categories = new List<Categories>();
            if (categoriesXml != null)
            {
                foreach (XmlNode categoryXml in categoriesXml)
                {
                    categories.Add(await ParseXmlCategoriesAsync(categoryXml));
                }
            }
            return categories;
        }

        public async Task<Tasks> GetTasksAsync(int id)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(pathTasks);
            var taskXml = doc.SelectSingleNode($"/Tasks/Task[@id={id}]");
            if (taskXml == null)
            {
                return null;
            }
            return await ParseXmlTasksAsync(taskXml);
        }

        public async Task<IEnumerable<Tasks>> GetTasksAsync()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(pathTasks);
            XmlNodeList? tasksXml = doc.SelectNodes("/Tasks/Task");

            var task = new List<Tasks>();
            if (tasksXml != null)
            {
                foreach (XmlNode taskXml in tasksXml)
                {
                    task.Add(await ParseXmlTasksAsync(taskXml));
                }
            }

            return task.OrderBy(i => i.status).ThenByDescending(i => i.due_date.HasValue).ThenBy(i => i.due_date); ;
        }

        public async Task<Tasks> ParseXmlTasksAsync(XmlNode taskXml)
        {
            var task = new Tasks();
            task.id = int.Parse(taskXml.Attributes["id"].Value);
            task.category_id = int.Parse(taskXml.Attributes["category_id"].Value);
            task.name = taskXml.Attributes["name"].Value;
            string? date = taskXml.Attributes["due_date"].Value;
            DateTime.TryParseExact(date, new string[] { "dd/MM/yyyy h:mm:ss tt", "d/M/yyyy h:mm:ss tt" }, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dueDate);
            task.due_date = dueDate != DateTime.MinValue ? dueDate : (DateTime?)null;
            task.status = bool.Parse(taskXml.Attributes["status"].Value);
            return task;
        }

        public async Task<Categories> ParseXmlCategoriesAsync(XmlNode categoryXml)
        {
            var category = new Categories();
            category.id = int.Parse(categoryXml.Attributes["id"].Value);
            category.name = categoryXml.Attributes["name"].Value;
            return category;
        }

        public async Task<int> FindNextIdAsync()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(pathTasks);
            return doc.SelectNodes("//Tasks/Task")
               .Cast<XmlElement>()
               .Max(c => Int32.Parse(c.Attributes["id"].Value)) + 1;
        }
    }
}
