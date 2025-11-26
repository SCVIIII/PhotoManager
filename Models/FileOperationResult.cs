using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoManager.Models
{
    public partial class FileOperationResult : ObservableObject
    {
        [ObservableProperty]
        private int jpgCount;

        [ObservableProperty]
        private int nefCount;

        [ObservableProperty]
        private string statusMessage = string.Empty;
    }
}
