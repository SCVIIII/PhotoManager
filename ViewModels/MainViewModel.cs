using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PhotoManager.Views;

using System.Windows;


namespace PhotoManager.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private object _currentView;
        public object CurrentView
        {
            get => _currentView;
            set => SetProperty(ref _currentView, value);
        }

        // 使用 IRelayCommand 和 RelayCommand<T>
        public IRelayCommand<Type> NavigateCommand { get; }
        public IRelayCommand ExitCommand { get; }

        public MainViewModel()
        {
            // 默认显示第一个页面
            CurrentView = new Page1View();

            // 使用 CommunityToolkit.Mvvm 的 RelayCommand
            NavigateCommand = new RelayCommand<Type>(Navigate);
            ExitCommand = new RelayCommand(Exit);
        }

        private void Navigate(Type viewType)
        {
            switch (viewType.Name)
            {
                case nameof(Views.Page1View):
                    CurrentView = new Page1View();
                    break;
                case nameof(Views.Page2View):
                    CurrentView = new Page2View();
                    break;
                
            }
        }

        private void Exit()
        {
            Application.Current.Shutdown();
        }
    }
}