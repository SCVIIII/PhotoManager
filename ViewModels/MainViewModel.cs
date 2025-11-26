using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoManager.ViewModels
{
    public partial class MainViewModel : ViewModelBase
    {
        [ObservableProperty]
        private ViewModelBase currentPage;

        public Page1ViewModel Page1 { get; } = new Page1ViewModel();
        public Page2ViewModel Page2 { get; } = new Page2ViewModel();
        public Page3ViewModel Page3 { get; } = new Page3ViewModel();

        public MainViewModel()
        {
            CurrentPage = Page1;
        }

        [RelayCommand]
        private void NavigateToPage1() => CurrentPage = Page1;

        [RelayCommand]
        private void NavigateToPage2() => CurrentPage = Page2;

        [RelayCommand]
        private void NavigateToPage3() => CurrentPage = Page3;
    }
}