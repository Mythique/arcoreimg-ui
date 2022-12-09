using arcoreimg_app.Helpers;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;

namespace arcoreimg_app
{
    public class EvaluationTask
    {
        private string[] files;
        private BackgroundWorker worker;
        private List<EvaluationInformation> evaluations = new List<EvaluationInformation>();

        public EvaluationTask(string directoryPath)
        {
            files = Directory.EnumerateFiles(directoryPath, "*.*",
                    SearchOption.TopDirectoryOnly).Where(s => s.EndsWith(".png") || s.EndsWith(".jpg")).ToArray();
            CreateWorker();
        }

        public EvaluationTask(string[] filesPath)
        {
            files = filesPath;
            CreateWorker();
        }

        private void CreateWorker()
        {
            worker = new BackgroundWorker();
            worker.WorkerSupportsCancellation = false;
            worker.WorkerReportsProgress = true;
            worker.DoWork += DoWork;
        }

        private void DoWork(object sender, DoWorkEventArgs e)
        {
            int fileCount = files.Length;
            int currentIndex = 0;

            foreach (string file in files)
            {
                EvaluationInformation asListItem = AppCore.CheckImage(file);
                evaluations.Add(asListItem);
                worker.ReportProgress((int)((float)currentIndex / (float)fileCount * 100));
                currentIndex++;
            }
            e.Result = evaluations;
        }

        public event ProgressChangedEventHandler ProgressChanged
        {
            add { worker.ProgressChanged += value; }
            remove { worker.ProgressChanged -= value; }
        }

        public event RunWorkerCompletedEventHandler Completed
        {
            add { worker.RunWorkerCompleted += value; }
            remove { worker.RunWorkerCompleted -= value; }
        }

        public void RunWorkerAsync()
        {
            worker.RunWorkerAsync();
        }
    }
}