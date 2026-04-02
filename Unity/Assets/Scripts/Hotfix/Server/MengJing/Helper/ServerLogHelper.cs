using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ET.Server
{
    public static class ServerLogHelper
    {

        /// 每小时执行一次
        /// </summary>
        public static void CheckLogSize()
        {
            if (ConfigData.LogLevel == 0)
            {
                return;
            }

            string logFolderPath = "../Logs/";
            
            // 大于1G的日志直接删除
            if (Directory.Exists(logFolderPath))
            {
                try
                {
                    string[] logFiles = Directory.GetFiles(logFolderPath, "*.log");
                    foreach (string logFilePath in logFiles)
                    {
                        long fileSizeInBytes = new FileInfo(logFilePath).Length;
                        
                        if (fileSizeInBytes >= 1073741824) // 1G = 1024 * 1024 * 1024 = 1073741824
                        {
                            File.Delete(logFilePath);
                            // File.WriteAllText(logFilePath, string.Empty); // 清空
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else
            {
                Console.WriteLine("日志文件不存在!");
            }
        }
        
        public static void LogWarning(string msg, bool log = false)
        {
            if (ConfigData.LogLevel >= 3 && log)
            {
                Log.Warning(msg);
            }
        }

        public static void LogDebug(string msg)
        {
            if (ConfigData.LogLevel >= 2)
            {
                Log.Debug(msg);
            }
        }


        

        public static void TrialBattleInfo(int zone, string loginfo)
        {
            ServerItem serverItem = ServerHelper.GetServerItemByZone(VersionMode.Beta, zone);
            if (serverItem == null)
            {
                return;
            }

            string log = $"{TimeHelper.DateTimeNow().ToString()}: {serverItem.ServerName} {loginfo}";
            ConfigData.KillInfoList.Add(log);
            if (ConfigData.KillInfoList.Count >= 10)
            {
                string filePath = "../Logs/WJ_KillPlayer.txt";
                WriteLogList(ConfigData.KillInfoList, filePath);
                ConfigData.KillInfoList.Clear();
            }
        }

        public static void PetMingBattleInfo(int zone, string loginfo)
        {
            ServerItem serverItem = ServerHelper.GetServerItemByZone(VersionMode.Beta, zone);
            if (serverItem == null)
            {
                return;
            }

            string log = $"{TimeHelper.DateTimeNow().ToString()}: {serverItem.ServerName} {loginfo}";
            ConfigData.KillInfoList.Add(log);
            if (ConfigData.KillInfoList.Count >= 10)
            {
                string filePath = "../Logs/WJ_KillPlayer.txt";
                WriteLogList(ConfigData.KillInfoList, filePath);
                ConfigData.KillInfoList.Clear();
            }
        }

        
        public static string GetNoticeNew()
        {
            long serverTime = TimeHelper.ServerNow();
            if (serverTime - ConfigData.NoticeLastGetTime < TimeHelper.Minute * 10
                && !string.IsNullOrEmpty(ConfigData.NoticeLastContent))
            {
                return ConfigData.NoticeLastContent;
            }


            string filePath = "../Logs/WJ_Notice.txt";
            if (File.Exists(filePath))
            {
                StreamReader sr = new StreamReader(filePath, Encoding.Default);
                string notice = string.Empty;
                string content = string.Empty;
                int index = 0;
                while ((content = sr.ReadLine()) != null)
                {
                    if (index == 0)
                    {
                        notice = $"{content}";
                    }
                    if (index == 1)
                    {
                        notice += $"@{content}";
                    }
                    if (index >= 2)
                    {
                        notice += $"\r\n{content}";
                    }
                    index++;
                }
                ConfigData.NoticeLastContent = notice;
            }
            else
            {
                ConfigData.NoticeLastContent = "0@公告未配置";
            }

            ConfigData.NoticeLastGetTime = serverTime;
            return ConfigData.NoticeLastContent;
        }

        public static string GetNotice()
        {
            string filePath = "../Logs/WJ_Notice.txt";
            if (File.Exists(filePath))
            {
                StreamReader sr = new StreamReader(filePath, Encoding.Default);
                string notice = string.Empty;
                string content = string.Empty;
                int index = 0;
                while ((content = sr.ReadLine()) != null)
                {
                    if (index == 0)
                    {
                        notice = $"{content}";
                    }
                    if (index == 1)
                    {
                        notice += $"@{content}";
                    }
                    if (index >= 2)
                    {
                        notice += $"\r\n{content}";
                    }
                    index++;
                }
                return  notice;
            }
            else
            {
                 return string.Empty;
            }
        }

        public static void OnStopServer()
        {
            string filePath = "../Logs/WJ_login.txt";
            WriteLogList(ConfigData.LoginInfoList, filePath);
            ConfigData.LoginInfoList.Clear();

            filePath = "../Logs/WJ_ZuoBi.txt";
            WriteLogList(ConfigData.ZuobiInfoList, filePath);
            ConfigData.ZuobiInfoList.Clear();
            
            filePath = "../Logs/WJ_KillPlayer.txt";
            WriteLogList(ConfigData.KillInfoList, filePath);
            ConfigData.KillInfoList.Clear();
        }

        public static void WriteLogList(List<string> infolist, string filePath, bool add = true)
        {
            string text = string.Empty;
            for (int i = 0; i < infolist.Count; i++)
            {
                text += infolist[i];
                text += "\r\n";
            }

            if (!add && File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            if (File.Exists(filePath))
            {
                StreamWriter sw = File.AppendText(filePath);
                sw.WriteLine(text);
                sw.Flush();
                sw.Close();
            }
            else
            {
                FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate);
                StreamWriter sw = new StreamWriter(fs);
                //开始写入
                sw.WriteLine(text);
                //清空缓冲区
                sw.Flush();
                //关闭流
                sw.Close();
                fs.Close();
            }
        }

     
        public static void LoginInfo(string log)
        {
            if (ConfigData.LogLevel == 0)
            {
                return;
            }
            log = TimeHelper.DateTimeNow().ToString() + " " + log;
            ConfigData.LoginInfoList.Add(log);
            if (ConfigData.LoginInfoList.Count >= 100)
            {
                string filePath = "../Logs/WJ_login.txt";
                WriteLogList(ConfigData.LoginInfoList, filePath);

                ConfigData.LoginInfoList.Clear();
            }
        }

     
        public static void ZuobiInfo(string log)
        {
            //if (LogLevel == 0)
            //{
            //    return;
            //}
            log = TimeHelper.DateTimeNow().ToString() + " " + log;
            ConfigData.ZuobiInfoList.Add(log);
            if (ConfigData.ZuobiInfoList.Count >= 10)
            {
                string filePath = "../Logs/WJ_ZuoBi.txt";
                WriteLogList(ConfigData.ZuobiInfoList, filePath);
                ConfigData.ZuobiInfoList.Clear();
            }
        }

        public static void OnLineInfo(string log)
        {
            log = TimeHelper.DateTimeNow().ToString() + " " + log;
            ConfigData.ZuobiInfoList.Add(log);
            if (ConfigData.ZuobiInfoList.Count >= 10)
            {
                string filePath = "../Logs/WJ_ZuoBi.txt";
                WriteLogList(ConfigData.ZuobiInfoList, filePath);
                ConfigData.ZuobiInfoList.Clear();
            }
        }

        public static void PaiMaiInfo(string log)
        {
            log = TimeHelper.DateTimeNow().ToString() + " " + log;
            string filePath = "../Logs/WJ_PaiMai.txt";
            WriteLogList(new List<string>() { log }, filePath);
        }
        

        /// <summary>
        /// 今日拍卖金币大于一亿 和   充值 100 并且 金币大于5亿
        /// </summary>
        /// <param name="log"></param>
        public static void PaiMai2Info(string log)
        {
            log =  TimeHelper.DateTimeNow().ToString() + " " + log;
            string filePath = "../Logs/WJ_PaiMai2.txt";
            WriteLogList(new List<string>() { log }, filePath, false);
        }

        public static void ChatInfo(string log)
        {
            log = TimeHelper.DateTimeNow().ToString() + " " + log;
            string filePath = "../Logs/WJ_Chat.txt";
            WriteLogList(new List<string>() { log }, filePath, true);
        }

        public static void GongZuoShi(string log)
        {
            log = TimeHelper.DateTimeNow().ToString() + " " + log;
            string filePath = "../Logs/WJ_ZuoBi.txt";
            WriteLogList(new List<string>() { log }, filePath);
        }
    }
}
