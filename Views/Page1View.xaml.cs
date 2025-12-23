
using System.Windows.Controls;
using UserControl = System.Windows.Controls.UserControl;

namespace PhotoManager.Views
{
    /// <summary>
    /// Page1View.xaml 的交互逻辑
    /// </summary>
    public partial class Page1View : UserControl
    {
        public Page1View()
        {
            InitializeComponent();
            this.DataContext = new ViewModels.Page1ViewModel();
        }
    }
}
