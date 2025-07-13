using ToDoApp.ViewModel;
using ToDoApp.Service;

namespace ToDoApp
{
    public partial class MainPage : ContentPage
    {
        private readonly ITodoClient _todoClient = new TodoClient();
        private readonly MainPageViewModel viewModel;
        public MainPage()
        {
            viewModel = new(_todoClient);
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}
