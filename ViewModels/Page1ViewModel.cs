using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using PhotoManager.ViewModels;
using System;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PhotoManager.ViewModels
{
    /// <summary>
    /// 程序任务:根据手机缩略图文件夹中的JPG文件名，从源文件夹中复制对应的JPG和NEF文件到输出文件夹
    /// 现只支持尼康的NEF格式，其他格式待自行扩展
    /// </summary>
    public partial class Page1ViewModel : ObservableObject
    {
        private string _thumbnailFolder;
        private string _sourceFolder;
        private string _outputFolder;
        private string _resultMessage;

        public string ThumbnailFolder
        {
            get => _thumbnailFolder;
            set => SetProperty(ref _thumbnailFolder, value);
        }

        public string SourceFolder
        {
            get => _sourceFolder;
            set => SetProperty(ref _sourceFolder, value);
        }

        public string OutputFolder
        {
            get => _outputFolder;
            set => SetProperty(ref _outputFolder, value);
        }

        public string ResultMessage
        {
            get => _resultMessage;
            set => SetProperty(ref _resultMessage, value);
        }

        // 使用 IRelayCommand
        public IRelayCommand BrowseThumbnailFolderCommand { get; }
        public IRelayCommand BrowseSourceFolderCommand { get; }
        public IRelayCommand BrowseOutputFolderCommand { get; }
        public IRelayCommand StartCommand { get; }
        public IRelayCommand ExitCommand { get; }

        public Page1ViewModel()
        {
            BrowseThumbnailFolderCommand = new RelayCommand(BrowseThumbnailFolder);
            BrowseSourceFolderCommand = new RelayCommand(BrowseSourceFolder);
            BrowseOutputFolderCommand = new RelayCommand(BrowseOutputFolder);
            StartCommand = new RelayCommand(StartProcessing);
            ExitCommand = new RelayCommand(Exit);
        }

        private void BrowseThumbnailFolder()
        {
            var folder = SelectFolder("选择缩略图文件夹");
            if (!string.IsNullOrEmpty(folder))
                ThumbnailFolder = folder;
        }

        private void BrowseSourceFolder()
        {
            var folder = SelectFolder("选择源文件文件夹");
            if (!string.IsNullOrEmpty(folder))
                SourceFolder = folder;
        }

        private void BrowseOutputFolder()
        {
            var folder = SelectFolder("选择输出文件夹");
            if (!string.IsNullOrEmpty(folder))
                OutputFolder = folder;
        }

        //WPF原生的文件夹选择对话框较为复杂，使用WinForms的对话框更为简便

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

        private void StartProcessing()
        {
            // 验证输入
            if (string.IsNullOrEmpty(ThumbnailFolder) ||
                string.IsNullOrEmpty(SourceFolder) ||
                string.IsNullOrEmpty(OutputFolder))
            {
                ResultMessage = "错误：请填写所有文件夹路径！";
                return;
            }

            if (!Directory.Exists(ThumbnailFolder))
            {
                ResultMessage = "错误：缩略图文件夹不存在！";
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
                // 获取缩略图文件夹中的所有JPG文件名（不含扩展名）
                var thumbnailFiles = Directory.GetFiles(ThumbnailFolder, "*.jpg", SearchOption.TopDirectoryOnly)
                    .Select(f => Path.GetFileNameWithoutExtension(f))
                    .ToList();

                int jpgCount = 0;
                int nefCount = 0;

                foreach (var fileName in thumbnailFiles)
                {
                    // 复制JPG文件
                    string jpgSourcePath = Path.Combine(SourceFolder, $"{fileName}.jpg");
                    string jpgOutputPath = Path.Combine(OutputFolder, $"{fileName}.jpg");
                    if (File.Exists(jpgSourcePath))
                    {
                        File.Copy(jpgSourcePath, jpgOutputPath, true);
                        jpgCount++;
                    }

                    // 复制NEF文件
                    string nefSourcePath = Path.Combine(SourceFolder, $"{fileName}.nef");
                    string nefOutputPath = Path.Combine(OutputFolder, $"{fileName}.nef");
                    if (File.Exists(nefSourcePath))
                    {
                        File.Copy(nefSourcePath, nefOutputPath, true);
                        nefCount++;
                    }
                }

                ResultMessage = $"处理完成！成功复制JPG文件：{jpgCount}个，NEF文件：{nefCount}个";
            }
            catch (Exception ex)
            {
                ResultMessage = $"处理出错：{ex.Message}";
            }
        }

        private void Exit()
        {
            Application.Current.Shutdown();
        }
    }
}