using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace OpenFileControl
{
    public partial class Page : UserControl
    {
        public Page()
        {
            InitializeComponent();
        }

        private void bOpenFileDialog_Click(object sender, RoutedEventArgs e)
        {
            // Create an instance of the open file dialog box.
            OpenFileDialog openImageDialog = new OpenFileDialog();

            // Set filter options and filter index.
            openImageDialog.Filter = "Image Files |*.jpeg;*.png;*.gif|All Files (*.*)|*.*";
            openImageDialog.FilterIndex = 1;

            openImageDialog.Multiselect = false;

            // Call the ShowDialog method to show the dialog box.
            bool? userClickedOK = openImageDialog.ShowDialog();

            // Process input if the user clicked OK.
            if (userClickedOK == true)
            {
                // Open the selected file to read.
                System.IO.Stream fileStream = openImageDialog.OpenFile();

                using (fileStream)
                {
                    var name = "..\\..\\images\\image" + DateTime.Now.ToString("yyyyddMMhhmmss") + GetExtension(openImageDialog.FileName);
                    var writer = new StreamWriter(name);
                    using (writer)
                    {
                        var memoryStream = new MemoryStream();
                        fileStream.CopyTo(memoryStream);
                        writer.Write(memoryStream.GetBuffer());
                    }
                }
            }
        }

        private string GetExtension(string filename)
        {
            var dotPosition = filename.IndexOf('.');
            return filename.Substring(dotPosition);
        }

        private string GetFileName(string filename)
        {
            var dotPosition = filename.IndexOf('.');
            return filename.Substring(0, dotPosition);
        }
    }
}
