using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PhotoManager.Models;
using System.IO;
using System.Windows;


namespace PhotoManager.ViewModels
{
    public partial class Page1ViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string thumbnailsFolder = string.Empty;

        [ObservableProperty]
        private string sourceFolder = string.Empty;

        [ObservableProperty]
        private string outputFolder = string.Empty;

        [ObservableProperty]
        private FileOperationResult operationResult = new();

        [ObservableProperty]
        private bool isProcessing = false;

        [RelayCommand]
        private async Task Start()
        {
            if (!ValidateFolder(ThumbnailsFolder, "缩略图文件夹") ||
                !ValidateFolder(SourceFolder, "源文件夹") ||
                !ValidateFolder(OutputFolder, "输出文件夹", true))
            {
                return;
            }

            IsProcessing = true;
            OperationResult.StatusMessage = "处理中...";

            await Task.Run(() => ProcessFiles());

            IsProcessing = false;
            OperationResult.StatusMessage = "处理完成！";
        }

        private void ProcessFiles()
        {
            var jpgCount = 0;
            var nefCount = 0;

            try
            {
                // 获取缩略图文件夹中的所有JPG文件
                var thumbnailFiles = Directory.GetFiles(ThumbnailsFolder, "*.jpg")
                    .Select(Path.GetFileNameWithoutExtension)
                    .ToHashSet();

                // 处理源文件夹中的文件
                foreach (var fileName in thumbnailFiles)
                {
                    // 复制JPG文件
                    var jpgSourcePath = Path.Combine(SourceFolder, fileName + ".jpg");
                    if (File.Exists(jpgSourcePath))
                    {
                        var jpgDestPath = Path.Combine(OutputFolder, fileName + ".jpg");
                        File.Copy(jpgSourcePath, jpgDestPath, true);
                        jpgCount++;
                    }

                    // 复制NEF文件
                    var nefSourcePath = Path.Combine(SourceFolder, fileName + ".nef");
                    if (File.Exists(nefSourcePath))
                    {
                        var nefDestPath = Path.Combine(OutputFolder, fileName + ".nef");
                        File.Copy(nefSourcePath, nefDestPath, true);
                        nefCount++;
                    }
                }

                Application.Current.Dispatcher.Invoke(() =>
                {
                    OperationResult.JpgCount = jpgCount;
                    OperationResult.NefCount = nefCount;
                });
            }
            catch (System.Exception ex)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    ShowMessage($"处理过程中发生错误: {ex.Message}");
                });
            }
        }
    }
}
