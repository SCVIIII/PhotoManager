using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoManager.ViewModels
{
    public partial class Page3ViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string contactEmail = "1307198581@qq.com";

        [ObservableProperty]
        private string programmingLanguage = "C#";

        [ObservableProperty]
        private string softwareVersion = "V1.0";

        [ObservableProperty]
        private string disclaimer = "本软件按\"原样\"提供，不附带任何明示或暗示的担保。使用本软件导致的任何损失，开发者不承担任何责任。";
    }
}
