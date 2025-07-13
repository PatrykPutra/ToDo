using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Json;
using ToDoApp.DataModel;
using System.Text.Json;
namespace ToDoApp.Service
{
    public class TodoClient : ITodoClient
    {
        private readonly HttpClient _httpClient;

        public TodoClient()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7168/api/")
            };
        }

        public async Task<List<TaskItem>> GetAllTasksAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<List<TaskItem>>("tasks");
            return response ?? new List<TaskItem>();
        }

        public async Task<bool> AddTaskAsync(TaskItem task)
        {
            var response = await _httpClient.PostAsJsonAsync("tasks", task);
            return response.IsSuccessStatusCode;
        }
        public async Task DeleteTaskAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"tasks/{id}");
            response.EnsureSuccessStatusCode();
        }
        public async Task UpdateTaskAsync(TaskItem task)
        {
            var json = JsonSerializer.Serialize(task);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"tasks/{task.Id}", content);
            response.EnsureSuccessStatusCode();
        }
    }
}
