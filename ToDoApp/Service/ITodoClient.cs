using System.Net.Http.Json;
using System.Text.Json;
using ToDoApp.DataModel;

namespace ToDoApp.Service
{
    public interface ITodoClient
    {
        public Task<List<TaskItem>> GetAllTasksAsync();

        public Task<bool> AddTaskAsync(TaskItem task);

        public Task DeleteTaskAsync(int id);

        public Task UpdateTaskAsync(TaskItem task);
          
        
    }
}
