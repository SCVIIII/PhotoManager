using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PhotoManager.ViewModels
{
    public partial class ViewModelBase : ObservableObject
    {
        [RelayCommand]
        protected virtual void Exit()
        {
            Application.Current.Shutdown();
        }

        protected void ShowMessage(string message)
        {
            MessageBox.Show(message, "信息", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        protected bool ValidateFolder(string folderPath, string fieldName, bool autoCreate = false)
        {
            if (string.IsNullOrWhiteSpace(folderPath))
            {
                ShowMessage($"{fieldName}不能为空");
                return false;
            }

            if (!autoCreate && !System.IO.Directory.Exists(folderPath))
            {
                ShowMessage($"{fieldName}不存在");
                return false;
            }

            if (autoCreate && !System.IO.Directory.Exists(folderPath))
            {
                try
                {
                    System.IO.Directory.CreateDirectory(folderPath);
                }
                catch
                {
                    ShowMessage($"无法创建{fieldName}");
                    return false;
                }
            }

            return true;
        }
    }
}
