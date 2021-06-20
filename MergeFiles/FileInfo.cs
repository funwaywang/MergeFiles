using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MergeFiles
{
    public class FileInfo : INotifyPropertyChanged
    {
        private bool _IsSelected = false;

        public event PropertyChangedEventHandler PropertyChanged;

        public FileInfo(string filePath)
        {
            FileName = System.IO.Path.GetFileName(filePath);
            Path = filePath;
        }

        public string FileName { get; set; }

        public string Path { get; set; }

        public bool IsSelected
        {
            get => _IsSelected;
            set
            {
                if (_IsSelected != value)
                {
                    _IsSelected = value;
                    OnPropertyChanged(nameof(IsSelected));
                }
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
