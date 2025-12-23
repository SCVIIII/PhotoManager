using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace PhotoManager.ViewModels
{
    public partial class ContactViewModel : ObservableObject
    {
        public string Email => "1307198581@qq.com";
        public string ProgrammingLanguage => "C# (.NET) + WPF + MaterialDesign";
        public string Version => "软件版本V1.0";
        public string Disclaimer => "免责声明：本软件仅供学习交流使用，因使用本软件造成的任何损失，作者不承担任何责任。";
    }
}