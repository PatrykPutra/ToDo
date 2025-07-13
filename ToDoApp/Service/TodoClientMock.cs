
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using ToDoApp.DataModel;

namespace ToDoApp.Service
{
    class TodoClientMock : ITodoClient
    {
        private string _filePath = @"c:\temp\todos.json";
        public void AddTask(TaskItem task)
        {
            var tasks = GetAllTasks();
            var latestId = tasks.OrderByDescending(task => task.Id).FirstOrDefault();
            if (latestId != null) { task.Id = latestId.Id + 1; }
            else { task.Id = 0; }
            tasks.Add(task);
            var jsonString = JsonSerializer.Serialize(tasks);
            File.WriteAllText(this._filePath, jsonString);
        }

        public async Task<bool> AddTaskAsync(TaskItem task)
        {
            AddTask(task);
            return true;
        }

        public void DeleteTask(int id)
        {
            var tasks = GetAllTasks();
            var taskTodelete = tasks.Where(element => element.Id == id).First();
            var taskIndex = tasks.IndexOf(taskTodelete);
            tasks.RemoveAt(taskIndex);
            var jsonString = JsonSerializer.Serialize(tasks);
            File.WriteAllText(this._filePath, jsonString);
        }

        public async Task DeleteTaskAsync(int id)
        {
            DeleteTask(id);
        }

        public List<TaskItem> GetAllTasks()
        {
            List<TaskItem> tasks = new List<TaskItem>();
            if (File.Exists(this._filePath))
            {
                string jsonString = File.ReadAllText(this._filePath);
                var jsonDeserialized = JsonSerializer.Deserialize<List<TaskItem>>(jsonString);
                tasks.AddRange(jsonDeserialized);
            }
            return tasks;
        }

        public async Task<List<TaskItem>> GetAllTasksAsync()
        {
            return GetAllTasks();
        }

        public void UpdateTask(TaskItem task)
        {
            var tasks = GetAllTasks();
            var taskToUpdate = tasks.Where(element=>element.Id==task.Id).First();
            var taskIndex = tasks.IndexOf(taskToUpdate);
           
            tasks[taskIndex] = task;
            var jsonString = JsonSerializer.Serialize(tasks);
            File.WriteAllText(this._filePath, jsonString);
        }

        public async Task UpdateTaskAsync(TaskItem task)
        {
            UpdateTask(task);
        }


    }
}
