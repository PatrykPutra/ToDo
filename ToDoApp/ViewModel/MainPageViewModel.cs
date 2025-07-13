using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using ToDoApp.DataModel;
using ToDoApp.Service;

namespace ToDoApp.ViewModel
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Todo> TodoList { get; set; } = new();
        public ObservableCollection<Todo> FilteredTodos { get; set; } = new();
        private bool _isFilterOcupied = false;
        private readonly ITodoClient _todoClient;
        private string? _newTaskText;
        public string NewTaskText
        {
            get
            {
                return _newTaskText;
            }
            set
            {
                _newTaskText = value;
                OnPropertyChanged();
            }
        }
        private string _taskFilter = "All";
        public string TaskFilter
        {
            get
            {
                return _taskFilter;
            }
            set
            {
                _taskFilter = value; 
                OnPropertyChanged();
                ApplyFilter();
            }
        }
        public ICommand AddCommand { get; }
        public ICommand UpdateCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand SetFilterCommand { get; }


        public event PropertyChangedEventHandler? PropertyChanged;
        public MainPageViewModel(ITodoClient todoClient)
        {
            _todoClient = todoClient;
            AddCommand = new Command(()=>AddTask(NewTaskText));
            UpdateCommand = new Command<Todo>(UpdateTask);
            DeleteCommand = new Command<Todo>(DeleteTask);
            SetFilterCommand = new Command<string>(SetFilter);
            Task.Run(async () => await LoadAllTasksFromDb());
        }

        public async void AddTask(string newTaskText)
        {
            await _todoClient.AddTaskAsync(new TaskItem { Text = newTaskText, IsCompleted = false });
            await LoadAllTasksFromDb();
            NewTaskText = string.Empty;
            ApplyFilter();
        }
        public async void UpdateTask(Todo item)
        {
            var newTaskItem = new TaskItem()
            {
                Id = item.Id,
                Text = item.Name,
                IsCompleted = item.IsCompleted
            };
            await _todoClient.UpdateTaskAsync(newTaskItem);
        }
        public async void DeleteTask(Todo item)
        {
            if (TodoList.Contains(item))
            {
                TodoList.Remove(item);
                await _todoClient.DeleteTaskAsync(item.Id);
                ApplyFilter();

            }
        }
        public async Task LoadAllTasksFromDb()
        {
            var tasks = await _todoClient.GetAllTasksAsync();
            var todos = new List<Todo>();
            
            foreach (var task in tasks)
            {
               var todo = new Todo(task.Text, task.IsCompleted,this) { Id = task.Id};
               todos.Add(todo);
            }
            TodoList = new ObservableCollection<Todo>(todos);
            ApplyFilter();
        }
        public void SetFilter(string filter)
        {
            TaskFilter = filter;
        }
        public void ApplyFilter()
        {
            if (!_isFilterOcupied)
            {
                _isFilterOcupied = true;
                IEnumerable<Todo> filteredTodos;
                FilteredTodos.Clear();
               
                if (TaskFilter == "Completed") { filteredTodos = TodoList.Where(todo => todo.IsCompleted); }
                else if (TaskFilter == "Pending") { filteredTodos = TodoList.Where(todo => !todo.IsCompleted); }
                else { filteredTodos = TodoList; }

                foreach (var todo in filteredTodos)
                {
                    FilteredTodos.Add(todo);
                }
                _isFilterOcupied = false;
            }
        }
     
        public void OnPropertyChanged([CallerMemberName] string propertyName ="")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
