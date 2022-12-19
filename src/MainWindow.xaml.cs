using arcoreimg_app.Controls;
using arcoreimg_app.Helpers;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace arcoreimg_app
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string FileListPath, NewDatabaseNamePath, NewDatabaseName;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            TxtDirPath1.Text = TxtDirPath3.Text = NewDatabaseNamePath = Path.Combine(Environment.GetEnvironmentVariable("USERPROFILE"), "Documents");
            TxtDirPath2.Text = TxtDirPath4.Text = Path.Combine(Environment.GetEnvironmentVariable("USERPROFILE"), "Downloads");
        }

        private void ImagePanel_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] tempFiles = (string[])e.Data.GetData(DataFormats.FileDrop);

                List<string> files = new List<string>();
                // Exclude files that are not an image
                // It feels like a hack, is their a cleaner way to do this?
                foreach (var item in tempFiles)
                {
                    if (AppCore.IsFileExtensionAccepted(item))
                    {
                        files.Add(item);
                    }
                }
                
                CreateAndStartEvaluationTask(files.ToArray()); 
            }
        }

        private void BtnImgBrowser_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new Microsoft.Win32.OpenFileDialog
            {
                Title = "Select an ImagePath",
                Filter = "Images |*.jpg; *jpeg; *.png",
                CheckFileExists = true,
                Multiselect = true
            };
            if (dlg.ShowDialog() == true)
            {
                CreateAndStartEvaluationTask(dlg.FileNames);
            }
        }

        private void BtnDirBrowser_Click(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog dlgLst = new CommonOpenFileDialog()
            {
                Title = "Select an ImagePath Directory",
                RestoreDirectory = true,
                IsFolderPicker = true
            };

            if (dlgLst.ShowDialog() == CommonFileDialogResult.Ok)
            {
                EvaluationTask appTask = new EvaluationTask(dlgLst.FileName);
                appTask.RunWorkerAsync();
                appTask.ProgressChanged += AppTask_ProgressChanged;
                appTask.Completed += AppTask_Completed;
            }
        }

        private void CreateAndStartEvaluationTask(string[] files)
        {
            EvaluationTask appTask = new EvaluationTask(files);
            appTask.RunWorkerAsync();
            appTask.ProgressChanged += AppTask_ProgressChanged;
            appTask.Completed += AppTask_Completed;
        }

        private void AppTask_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ProgressBar.Value = e.ProgressPercentage;
        }

        private void AppTask_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            worker.Dispose();
            List<EvaluationInformation> scanned = (List<EvaluationInformation>)e.Result;

            for (int i = 0; i < scanned.Count; i++)
            {
                EvaluationInformation scan = scanned[i];
                EvaluationItemUI asListItem = new EvaluationItemUI
                {
                    Title = scan.Information,
                    Image = scan.ImagePath,
                    Score = scan.Score
                };
                ImageList.Children.Add(asListItem);
            }

            ProgressBar.Value = 100;
        }

        private void BtnDbDirBrowser_Click(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog dlgDb = new CommonOpenFileDialog()
            {
                Title = "Select Where to save the Image Database",
                InitialDirectory = Path.Combine(Environment.GetEnvironmentVariable("USERPROFILE"), "Documents"),
                IsFolderPicker = true
            };
            if (dlgDb.ShowDialog() == CommonFileDialogResult.Ok)
            {
                TxtDirPath1.Text = TxtDirPath3.Text = NewDatabaseNamePath = dlgDb.FileName;
            }
        }

        private void BtnImgDirBrowser_Click(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog dlgLst = new CommonOpenFileDialog()
            {
                Title = "Select an Image Directory",
                InitialDirectory = Path.Combine(Environment.GetEnvironmentVariable("USERPROFILE"), "Downloads"),
                IsFolderPicker = true
            };

            if (dlgLst.ShowDialog() == CommonFileDialogResult.Ok)
            {
                TxtDirPath2.Text = FileListPath = dlgLst.FileName;

                NewDatabaseName = "myimages_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".imgdb";
                Process process = AppCore.CreateArCoreImgProcess($"build-db --input_images_directory=\"{FileListPath}\" --output_db_path=\"{Path.Combine(NewDatabaseNamePath, NewDatabaseName)}\"");
                process.Start();

                try
                {
                    string result = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();
                    TxtFeedback1.Text = "New database: '" + NewDatabaseName + "' created successfully!\nYou may specify another image directory to create another database.";
                }
                catch (Exception)
                {
                    //arcoreimg.WriteLogs("App Errors", @" " + ex.Message, @"" + ex.InnerException, @"" + ex.StackTrace);
                }
            }
        }

        private void BtnTxtBrowser_Click(object sender, RoutedEventArgs e)
        {
            var dlgLst = new Microsoft.Win32.OpenFileDialog
            {
                Title = "Select a File List",
                Filter = "File List |*.txt",
                CheckFileExists = true
            };

            if (dlgLst.ShowDialog() == true)
            {
                TxtDirPath4.Text = FileListPath = dlgLst.FileName;

                NewDatabaseName = "myimages_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".imgdb";

                Process process = AppCore.CreateArCoreImgProcess($"build-db --input_image_list_path=\"{FileListPath}\" --output_db_path=\"{Path.Combine(NewDatabaseNamePath, NewDatabaseName)}\"");
                process.Start();

                try
                {
                    string result = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();//NewDatabaseName, LstFilename
                    TxtFeedback2.Text = "New database: '" + NewDatabaseName + "' created successfully!\nYou may browse another Image File List to create another Database";
                }
                catch (Exception)
                {
                    //arcoreimg.WriteLogs("App Errors", @" " + ex.Message, @"" + ex.InnerException, @"" + ex.StackTrace);
                }
            }
        }
    }
}