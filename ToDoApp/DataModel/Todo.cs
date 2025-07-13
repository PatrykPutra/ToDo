using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoApp.ViewModel;

namespace ToDoApp.DataModel
{
    public class Todo : ObservableCollection<Todo>,IComparable<Todo>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        private bool _isCreated = false;
        public MainPageViewModel ViewModel { get; set; }
        private bool _isCompleted;
        public bool IsCompleted 
        { 
            get => _isCompleted; 
            set
            {
                if(_isCompleted!=value)
                {
                    _isCompleted = value;
                    OnPropertyChanged(nameof(IsCompleted));
                    ViewModel.ApplyFilter();
                    if(_isCreated)
                    {
                        ViewModel.UpdateTask(this);
                    }
                }
            }
        }
        public Todo(string name, bool isCompleted,MainPageViewModel viewModel)
        {
            Name = name;
            ViewModel = viewModel;
            IsCompleted = isCompleted;
            _isCreated = true;
        }
        protected override event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public int CompareTo(Todo? other)
        {
            if(other == null) return 1;
            if (IsCompleted == false && other.IsCompleted == true) return -1;
            if(IsCompleted == true && other.IsCompleted==false) return 1;
            else return 0;
        }
    }
}
