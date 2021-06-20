using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MergeFiles
{
    public static class Commands
    {
        public static readonly RoutedUICommand AddFiles = new RoutedUICommand(nameof(AddFiles), nameof(AddFiles), typeof(Commands));
        public static readonly RoutedUICommand DeleteFiles = new RoutedUICommand(nameof(DeleteFiles), nameof(DeleteFiles), typeof(Commands));
        public static readonly RoutedUICommand Merge = new RoutedUICommand(nameof(Merge), nameof(Merge), typeof(Commands));
    }
}
