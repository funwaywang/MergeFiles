using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MergeFiles
{
    public partial class MainWindow : Window
    {
        public static readonly DependencyProperty IsMergingProperty = DependencyProperty.Register(nameof(IsMerging), typeof(bool), typeof(MainWindow));

        public MainWindow()
        {
            InitializeComponent();
        }

        public bool IsMerging
        {
            get => (bool)GetValue(IsMergingProperty);
            set => SetValue(IsMergingProperty, value);
        }

        public ObservableCollection<FileInfo> Files { get; } = new ObservableCollection<FileInfo>();

        private void AddFiles_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Multiselect = true
            };

            if (dialog.ShowDialog(this) == true)
            {
                foreach (var file in dialog.FileNames)
                {
                    if (!Files.Any(f => string.Equals(f.Path, file, StringComparison.OrdinalIgnoreCase)))
                    {
                        Files.Add(new FileInfo(file));
                    }
                }
            }
        }

        private void DeleteFiles_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var selectedFiles = Files.Where(f => f.IsSelected).ToList();
            if (selectedFiles.Any())
            {
                if (MessageBox.Show(this, "Are you sure delete the selected files?", "Delete Files", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
                {
                    foreach (var file in selectedFiles)
                    {
                        Files.Remove(file);
                    }
                }
            }
        }

        private async void Merge_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (IsMerging)
            {
                return;
            }

            var files = Files.ToList();
            if (files.Any())
            {
                var dialog = new SaveFileDialog();
                if (dialog.ShowDialog(this) == true)
                {
                    IsMerging = true;
                    try
                    {
                        await MergeFilesAsync(files, dialog.FileName);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(this, ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    finally
                    {
                        IsMerging = false;
                    }
                }
            }
        }

        private async Task MergeFilesAsync(List<FileInfo> files, string destinationFile)
        {
            using (var destinationStream = new FileStream(destinationFile, FileMode.Create, FileAccess.Write))
            {
                foreach (var file in files)
                {
                    if (!File.Exists(file.Path))
                    {
                        throw new FileNotFoundException("File not found.", file.Path);
                    }

                    using (var sourceStream = new FileStream(file.Path, FileMode.Open, FileAccess.Read))
                    {
                        await sourceStream.CopyToAsync(destinationStream);
                    }
                }
            }
        }
    }
}
