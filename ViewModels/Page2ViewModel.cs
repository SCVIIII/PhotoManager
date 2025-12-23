using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Linq;
using System.Windows;

namespace PhotoManager.ViewModels
{
    public partial class Page2ViewModel : ObservableObject
    {
        [ObservableProperty]
        private string sourceFolder;

        [ObservableProperty]
        private string outputFolder;

        [ObservableProperty]
        private string resultMessage;

        public Page2ViewModel()
        {
            // 使用源生成器自动创建命令
        }

        [RelayCommand]
        private void BrowseSourceFolder()
        {
            var folder = SelectFolder("选择源文件文件夹");
            if (!string.IsNullOrEmpty(folder))
                SourceFolder = folder;
        }

        [RelayCommand]
        private void BrowseOutputFolder()
        {
            var folder = SelectFolder("选择输出文件夹");
            if (!string.IsNullOrEmpty(folder))
                OutputFolder = folder;
        }

        [RelayCommand]
        private void StartProcessing()
        {
            // 验证输入
            if (string.IsNullOrEmpty(SourceFolder))
            {
                ResultMessage = "错误：请填写源文件文件夹路径！";
                return;
            }

            if (string.IsNullOrEmpty(OutputFolder))
            {
                ResultMessage = "错误：请填写输出文件夹路径！";
                return;
            }

            if (!Directory.Exists(SourceFolder))
            {
                ResultMessage = "错误：源文件文件夹不存在！";
                return;
            }

            // 创建输出文件夹（如果不存在）
            if (!Directory.Exists(OutputFolder))
            {
                Directory.CreateDirectory(OutputFolder);
            }

            try
            {
                // 获取源文件夹中的所有JPG文件名（不含扩展名）
                var jpgFiles = Directory.GetFiles(SourceFolder, "*.jpg", SearchOption.TopDirectoryOnly)
                    .Select(f => new
                    {
                        FullPath = f,
                        FileName = Path.GetFileNameWithoutExtension(f)
                    })
                    .ToList();

                int jpgCount = 0;
                int rawCount = 0;

                foreach (var jpgFile in jpgFiles)
                {
                    string jpgFileName = jpgFile.FileName;

                    // 复制JPG文件
                    string jpgSourcePath = jpgFile.FullPath;
                    string jpgOutputPath = Path.Combine(OutputFolder, Path.GetFileName(jpgSourcePath));
                    if (File.Exists(jpgSourcePath))
                    {
                        File.Copy(jpgSourcePath, jpgOutputPath, true);
                        jpgCount++;
                    }

                    // 查找并复制对应的NEF或RAW文件
                    string nefFilePath = Path.Combine(SourceFolder, $"{jpgFileName}.nef");
                    string rawFilePath = Path.Combine(SourceFolder, $"{jpgFileName}.raw");

                    if (File.Exists(nefFilePath))
                    {
                        string nefOutputPath = Path.Combine(OutputFolder, Path.GetFileName(nefFilePath));
                        File.Copy(nefFilePath, nefOutputPath, true);
                        rawCount++;
                    }
                    else if (File.Exists(rawFilePath))
                    {
                        string rawOutputPath = Path.Combine(OutputFolder, Path.GetFileName(rawFilePath));
                        File.Copy(rawFilePath, rawOutputPath, true);
                        rawCount++;
                    }
                }

                ResultMessage = $"处理完成！成功复制JPG文件：{jpgCount}个，NEF/RAW文件：{rawCount}个";
            }
            catch (Exception ex)
            {
                ResultMessage = $"处理出错：{ex.Message}";
            }
        }

        [RelayCommand]
        private void Exit()
        {
            Application.Current.Shutdown();
        }

        // 使用WPF原生的文件夹选择对话框
        private string SelectFolder(string title)
        {
            var dialog = new OpenFolderDialog
            {
                Title = title
            };

            if (dialog.ShowDialog() == true)
            {
                return dialog.FolderName;
            }
            return null;
        }
    }
}