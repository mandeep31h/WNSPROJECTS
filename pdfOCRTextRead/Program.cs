using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace pdfOCRTextRead
{
    class Program
    {
        static void Main(string[] args)
        {
            PythonEXECaller(@"C:\Users\SUKHVEER\Desktop\1May\Pattern_de_Manabu_JLPT_N1-Moji.Goi_08-48-27.pdf", "false");
        }
        private static void PythonEXECaller(string pdfFilePath, string deletePdfFile)
        {
            string result = "error";
            try
            {
                string rootFolderPath = Path.GetDirectoryName(pdfFilePath);
                string fileName = Path.GetFileNameWithoutExtension(pdfFilePath);
                try
                {
                    if (File.Exists(rootFolderPath + "\\" + fileName + ".txt"))
                        File.Delete(rootFolderPath + "\\" + fileName + ".txt");
                    if (deletePdfFile == "true")
                    {
                        /*  if (File.Exists(rootFolderPath + "\\" + fileName + "_DotNet.txt"))
                              File.Delete(rootFolderPath + "\\" + fileName + "_DotNet.txt");*/
                        if (File.Exists(rootFolderPath + "\\" + fileName + "_Ocr.txt"))
                            File.Delete(rootFolderPath + "\\" + fileName + "_Ocr.txt");
                    }
                }
                catch (Exception ex) { }
                char[] splitter = { '\r' };

                var proc = new Process();
                proc.StartInfo.FileName = AppDomain.CurrentDomain.BaseDirectory + "pyFile\\ScannedPdfText.exe";
                proc.StartInfo.RedirectStandardError = true;
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.Arguments = " \"" + pdfFilePath;
                proc.Start();
                StreamReader sReader = proc.StandardError;
                string[] output = sReader.ReadToEnd().Split(splitter);
                Thread.Sleep(10000);
                string error = string.Join("\r", output);

                proc.WaitForExit();
                var exitCode = proc.ExitCode;
                proc.Close();
                if (exitCode == 0)
                {
                    result = "done";
                }
                else
                {
                    File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "ErrorLog.txt", "exitCode:-" + exitCode.ToString() + "Error:-" + error);
                }
                if (File.Exists(rootFolderPath + "\\" + fileName + ".txt"))
                    File.Delete(rootFolderPath + "\\" + fileName + ".txt");
            }
            catch (Exception ex)
            {
                File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "ErrorLog_Catch.txt", ex.Message);
                result = ex.Message;
            }
        }
    }
}
