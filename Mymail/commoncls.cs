using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

namespace Mymail
{
    class commoncls
    {
        Connection ob1 = new Connection();
        mail ob2 = new mail();
        int iTimeDelay = 0;
        Thread objThread;
        ThreadStart objThreadStart;
        public static bool ServiceStatus = true;




        public void Logging(string str, string sType)
        {
            while (true)
            {
                try
                {
                    string file = AppDomain.CurrentDomain.BaseDirectory + "/LOGS/" + sType + DateTime.Now.ToString("ddMMyy") + ".LOG";
                    File.AppendAllText(file, DateTime.Now.ToLongTimeString() + "***" + str + Environment.NewLine);
                    break;
                }
                catch (Exception cx)
                {

                }
            }
        }
        internal void checkmail()
        {
            try
            {
                iTimeDelay = Convert.ToInt32(ConfigurationManager.AppSettings["INTERVAL"]);
                Logging("Processing Interval :" + iTimeDelay.ToString() + " MilliSeconds", "T_");
                objThreadStart = new ThreadStart(ProcessData);
                objThread = new Thread(objThreadStart);
                objThread.Start();

            }
            catch (Exception cx)
            {
                Logging("Error on checkSMS : " + cx.Message, "E_");
            }
        }

        public void ProcessData()
        {
            try
            {
                Logging("processing data : ", "T_");
                while (ServiceStatus)
                {
                    string sql = "select * from File_details where Status='active' ";
                    DataTable Dt = selectData(sql);

                    foreach (DataRow row in Dt.Rows)
                    {
                        string mail = row[2].ToString();
                        string subject = row[3].ToString();
                        string body = row[5].ToString();
                        string filetype = row[6].ToString();

                        FileResult fileResult = getfile();
                        ArrayList fileList = fileResult.FileNamesList;
                        string destinationPath = fileResult.DestinationPath;

                        foreach (string fileName in fileList)
                        {
                            bool matched = false;
                            foreach (string ft in filetype.Split(','))
                            {
                                if (MatchFileType(fileName, ft))
                                {
                                    string sourceFilePath = Path.Combine(@"C:\location\exact", fileName);
                                    string destinationFilePath = Path.Combine(destinationPath, fileName);
                                    File.Move(sourceFilePath, destinationFilePath);
                                    ob2.SendMail(mail, body, subject, destinationPath, fileName);
                                    matched = true;
                                    break;
                                }
                            }

                            if (!matched)
                            {
                                //not matching
                            }
                        }

                        Thread.Sleep(iTimeDelay);
                    }
                }
            }
            catch (Exception cx)
            {
                Logging("Error on ProcessData : " + cx.Message, "E_");
            }
        }
        private string ModifyFileName(string fileName, string fileType)
        {
            if (fileType.StartsWith("*"))
            {
                return Path.GetFileNameWithoutExtension(fileName);
            }
            else
            {
                return fileName;
            }
        }


        private bool MatchFileType(string fileName, string fileType)
        {
            fileName = ModifyFileName(fileName, fileType); // Modify the fileName based on the fileType

            if (fileType.StartsWith("*"))
            {
                return fileName.EndsWith(fileType.Replace("*", ""));
            }
            else if (fileType.EndsWith("*"))
            {
                return fileName.StartsWith(fileType.Replace("*", ""));
            }
            else
            {
                return fileName.Equals(fileType);
            }
        }
        public DataTable selectData(string sql)

        {
            DataTable dt = new DataTable();
            dt = ob1.ExecuteGetDataTable(sql);
            return dt;
        }
        public class FileResult
        {
            public string DestinationPath { get; set; }
            public ArrayList FileNamesList { get; set; }
        }

        public FileResult getfile()
        {
            string oldPath = @"C:\location\exact";
            string destinationPath = @"C:\newpath";
            ArrayList fileNamesList = new ArrayList();


            try
            {
                if (!Directory.Exists(oldPath))
                {
                    Console.WriteLine("no file found");
                }
                string[] files = Directory.GetFiles(oldPath);

                foreach (string filePath in files)
                {
                    string fileName = Path.GetFileName(filePath);
                    fileNamesList.Add(fileName);
                }
            }
            catch (Exception ex)
            {
                Logging("Error on file transferring : " + ex.Message, "E_");
            }

            return new FileResult { DestinationPath = destinationPath, FileNamesList = fileNamesList };
        }
    }
}

