using Dapper;
using System.Data;
using todo_list.Context;
using todo_list.Models.DTO;
using todo_list.Models;
using todo_list.Repository.Interfaces;

namespace todo_list.Repository
{
    public class TasksRepo : ITasks
    {
        private readonly DapperContext _context;
        public TasksRepo(DapperContext context)
        {
            _context = context;
        }

        public async Task CompleteTasksAsync(int id)
        {
            var query = "UPDATE Tasks SET status = ~status WHERE id = @id";

            using var connection = _context.CreateConnection();
            await connection.ExecuteAsync(query, new { id });
        }

        public async Task CreateTasksAsync(TaskCreationModel item)
        {
            var query = @"
            INSERT INTO Tasks (category_id, name, due_date, status) 
            VALUES (@category_id, @name, @due_date, 0)";

            var param = new DynamicParameters();
            param.Add("@name", item.name, DbType.String);
            param.Add("@category_id", item.category_id, DbType.Int32);
            param.Add("@due_date", item.due_date, DbType.DateTime);

            using var connection = _context.CreateConnection();
            await connection.ExecuteAsync(query, param);
        }

        public async Task DeleteTasksAsync(int id)
        {
            var query = "DELETE FROM Tasks WHERE id = @id";

            using var connection = _context.CreateConnection();
            await connection.ExecuteAsync(query, new { id });
        }

        public async Task<IEnumerable<Categories>> GetCategoriesAsync()
        {
            var query = "SELECT * FROM Categories";

            using var connection = _context.CreateConnection();
            var categories = await connection.QueryAsync<Categories>(query);
            return categories.ToList();
        }

        public async Task<Tasks> GetTasksAsync(int id)
        {
            var query = "SELECT * FROM Tasks WHERE id = @id";

            using var connection = _context.CreateConnection();
            var task = await connection.QuerySingleOrDefaultAsync<Tasks>(query, new { id });
            return task;
        }

        public async Task<IEnumerable<Tasks>> GetTasksAsync()
        {
            var query = "SELECT * FROM Tasks";

            using var connection = _context.CreateConnection();
            var task = await connection.QueryAsync<Tasks>(query);

            task = task.OrderBy(i => i.status)
                       .ThenByDescending(i => i.due_date.HasValue)
                       .ThenBy(i => i.due_date);

            return task.Distinct().ToList();
        }
    }
}