using System.Diagnostics;
using System.IO;

namespace arcoreimg_app.Helpers
{
    public class AppCore
    {
        /// <summary>
        /// Create a cmd Process
        /// </summary>
        /// <param name="args">args</param>
        /// <returns></returns>

        public static Process CreateProcess(string args)
        {
            ProcessStartInfo procstartInfo = new ProcessStartInfo();
            Process process = new Process();
            procstartInfo.FileName = "./arcoreimg.exe";
            procstartInfo.Arguments = args;
            // Do not show the black cmd.
            procstartInfo.CreateNoWindow = true;
            process.StartInfo = procstartInfo;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            return process;
        }

        public static string GetFileSize(double len)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            int order = 0;
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }

            return string.Format("{0:0.##} {1}", len, sizes[order]);
        }

        public static AsScanned CheckImage(string imagePath)
        {
            AsScanned scan = new AsScanned();
            scan.Image = imagePath;

            double filelen = new FileInfo(imagePath).Length;
            Process process = CreateProcess($"eval-img --input_image_path=\"{imagePath}\"");
            process.Start();

            string error = process.StandardError.ReadToEnd();
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            scan.Title = $"{Path.GetFileName(imagePath)} ({GetFileSize(filelen)}) | ";

            if (!string.IsNullOrEmpty(error))
            {
                scan.Score = 0;
                scan.Title += error;
            }
            else if (int.TryParse(output, out int score))
            {
                scan.Score = score;

                if (score < 49)
                    scan.Title += "Poor Quality Image";
                else if (score > 90)
                    scan.Title += "Best Quality Image";
                else
                    scan.Title += "Good Quality Image";
            }
            else
            {
                scan.Score = 0;
                scan.Title += string.IsNullOrEmpty(output) ? "No result available" : output;
            }

            return scan;
        }
    }
}