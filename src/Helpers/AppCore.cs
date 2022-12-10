using System.Diagnostics;
using System.IO;

namespace arcoreimg_app.Helpers
{
    public class AppCore
    {
        public static Process CreateArCoreImgProcess(string args)
        {
            ProcessStartInfo procstartInfo = new ProcessStartInfo();
            Process process = new Process();
            procstartInfo.FileName = "./arcoreimg.exe";
            procstartInfo.Arguments = args;
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
                len /= 1024;
            }

            return string.Format("{0:0.##} {1}", len, sizes[order]);
        }

        public static EvaluationInformation CheckImage(string imagePath)
        {
            EvaluationInformation evaluation = new EvaluationInformation
            {
                ImagePath = imagePath
            };

            double fileLength = new FileInfo(imagePath).Length;
            Process process = CreateArCoreImgProcess($"eval-img --input_image_path=\"{imagePath}\"");
            process.Start();

            string error = process.StandardError.ReadToEnd();
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            evaluation.Information = $"{Path.GetFileName(imagePath)} ({GetFileSize(fileLength)}) | ";

            if (!string.IsNullOrEmpty(error))
            {
                evaluation.Score = 0;

                error = error.Replace("\r\n", " ");

                evaluation.Information += error;
            }
            else if (int.TryParse(output, out int score))
            {
                evaluation.Score = score;

                if (score < 49)
                    evaluation.Information += "Poor Quality Image";
                else if (score > 90)
                    evaluation.Information += "Best Quality Image";
                else
                    evaluation.Information += "Good Quality Image";
            }
            else
            {
                evaluation.Score = 0;
                evaluation.Information += string.IsNullOrEmpty(output) ? "No result available" : output;
            }

            return evaluation;
        }
    }
}