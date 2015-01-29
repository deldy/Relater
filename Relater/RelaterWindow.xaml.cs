using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Relater
{
    /// <summary>
    /// Interaction logic for RelaterToolWindow.xaml
    /// </summary>
    public partial class RelaterWindow : UserControl
    {
        public RelaterWindow()
        {
            InitializeComponent();

        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1300:SpecifyMessageBoxOptions")]
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            try {
                var dte2 = (EnvDTE80.DTE2)Microsoft.VisualStudio.Shell.Package.GetGlobalService(typeof(Microsoft.VisualStudio.Shell.Interop.SDTE));
                var solutionDir = System.IO.Path.GetDirectoryName(dte2.Solution.FullName).ToLower();
                var activeDocument = dte2.ActiveDocument.FullName;
                var data = File.ReadAllText(System.IO.Path.Combine(System.IO.Path.GetTempPath(), "relater.txt"));

                Files.Items.Clear();

                var lines = data.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
                var found = new List<string>();
                for (int x = 1; x < lines.Length - 1; x++) {
                    if (lines[x] == activeDocument) {
                        var prev = lines[x - 1];
                        var next = lines[x + 1];

                        if (prev.ToLower().StartsWith(solutionDir.ToLower()) && prev != activeDocument) {
                            found.Add(prev);
                        }
                        if (next.ToLower().StartsWith(solutionDir.ToLower()) && next != activeDocument) {
                            found.Add(next);
                        }
                    }
                }

                var group = found.GroupBy(x => x);

                var importance = group.OrderByDescending(x => x.Count()).Select(x => new FileItem(solutionDir, x.Key, x.Count())).Take(20);

                foreach (var entry in importance) {
                    Files.Items.Add(entry);
                }
            } catch (Exception) {
                MessageBox.Show("Could not get related files.");
            }
            
        }

        public class FileItem
        {
            private string path;
            private string solutionDir;
            private int times;

            public FileItem(string solutionDir, string path, int times)
            {
                this.solutionDir = solutionDir;
                this.path = path;
                this.times = times;
            }

            public string Path
            {
                get
                {
                    return path.Substring(solutionDir.Length);
                }
            }

            public string Name
            {
                get
                {
                    return System.IO.Path.GetFileName(Path);
                }
            }

            public int Times
            {
                get
                {
                    return times;
                }
            }
        }

        private void Files_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try {
                var dte2 = (EnvDTE80.DTE2)Microsoft.VisualStudio.Shell.Package.GetGlobalService(typeof(Microsoft.VisualStudio.Shell.Interop.SDTE));
                var solutionDir = System.IO.Path.GetDirectoryName(dte2.Solution.FullName);
                var file = solutionDir + ((FileItem)Files.SelectedItem).Path;
                dte2.ItemOperations.OpenFile(file);
            } catch (Exception ex) {
                MessageBox.Show("Could not open file\n" + ex.Message);
            }
  
        }
    }
}