using Microsoft.AspNetCore.Mvc;
using todo_list.Models.DTO;
using todo_list.Repository;
using todo_list.Repository.Interfaces;

namespace ToDoListMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ITasks _repo;
        private readonly ITasksXML _repoXML;

        public HomeController(ITasks repo, ITasksXML repoXML)
        {
            _repo = repo;
            _repoXML = repoXML;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? type = "mysql")
        {
            var taskAndCategories = new TasksAndCategoriesViewModel();
            taskAndCategories.Type = type;
            if (type != "mysql")
            {
                taskAndCategories.Tasks = await _repoXML.GetTasksAsync();
                taskAndCategories.Categories = await _repoXML.GetCategoriesAsync();
            }
            else
            {
                taskAndCategories.Tasks = await _repo.GetTasksAsync();
                taskAndCategories.Categories = await _repo.GetCategoriesAsync();
            }
            return View(taskAndCategories);
        }
        [HttpPost]
        public async Task<IActionResult> Index(TaskCreationModel item)
        {
            if (!ModelState.IsValid)
            {
                var taskAndCategories = new TasksAndCategoriesViewModel();
                taskAndCategories.Type = item.type;
                if (item.type != "mysql")
                {
                    taskAndCategories.Tasks = await _repoXML.GetTasksAsync();
                    taskAndCategories.Categories = await _repoXML.GetCategoriesAsync();
                }
                else
                {
                    taskAndCategories.Tasks = await _repo.GetTasksAsync();
                    taskAndCategories.Categories = await _repo.GetCategoriesAsync();
                }
                return View(taskAndCategories);
            }

            if (item.type != "mysql")
            {
                await _repoXML.CreateTasksAsync(item);
            }
            else
            {
                await _repo.CreateTasksAsync(item);
            }

            return RedirectToAction("Index", new { type = item.type });
        }

        [HttpGet]
        public async Task<IActionResult> DeleteTask(int id, string? type = "mysql")
        {
            if (type != "mysql")
            {
                await _repoXML.DeleteTasksAsync(id);
            }
            else
            {
                await _repo.DeleteTasksAsync(id);
            }
            return RedirectToAction("Index", new { type = type });
        }

        [HttpGet]
        public async Task<IActionResult> CompleteTask(int id, string? type = "mysql")
        {
            if (type != "mysql")
            {
                var task = await _repoXML.GetTasksAsync(id);
                if (task == null)
                {
                    return NotFound();
                }
                await _repoXML.CompleteTasksAsync(id);
            }
            else
            {
                var task = await _repo.GetTasksAsync(id);
                if (task == null)
                {
                    return NotFound();
                }
                await _repo.CompleteTasksAsync(id);
            }

            return RedirectToAction("Index", new { type = type });
        }
    }
}