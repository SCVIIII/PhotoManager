using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PhotoManager.Models;
using System.IO;
using System.Windows;




namespace PhotoManager.ViewModels
{
    public partial class Page2ViewModel : ViewModelBase
    {
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
            if (!ValidateFolder(SourceFolder, "源文件夹") ||
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
                // 获取所有JPG文件名（不含扩展名）
                var jpgFiles = Directory.GetFiles(SourceFolder, "*.jpg")
                    .Select(Path.GetFileNameWithoutExtension)
                    .ToHashSet();

                // 复制JPG文件
                foreach (var jpgFile in Directory.GetFiles(SourceFolder, "*.jpg"))
                {
                    var fileName = Path.GetFileName(jpgFile);
                    var destPath = Path.Combine(OutputFolder, fileName);
                    File.Copy(jpgFile, destPath, true);
                    jpgCount++;
                }

                // 只复制与JPG文件同名的NEF文件
                foreach (var fileName in jpgFiles)
                {
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
