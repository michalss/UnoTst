using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;


public enum LogType
{
    error,
    warning,
    information,
    debug
}


public class Log
{
    //ApplicationData.Current.LocalFolder
    private static string pluginFolder = Path.Combine(ApplicationData.Current.LocalCacheFolder.Path, "plugins");
    private static string logRoot = Path.Combine(ApplicationData.Current.LocalCacheFolder.Path, "logs");
    private static string logFile = Path.Combine(ApplicationData.Current.LocalCacheFolder.Path, "logs", "logs.txt");
    public static void Logger(LogType logType, string functionSubName, Exception exception, string extraData = "")
    {
        try
        {
            if (!Directory.Exists(logRoot)) Directory.CreateDirectory(logRoot);
            if (!File.Exists(logFile)) File.Create(logFile).Dispose();
            string logFormat = $"#{DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss")}#   [{logType.ToString()}]  CustomMessage={extraData}  Name=({functionSubName}) Message={exception.Message} | StackTrace={exception.StackTrace}\n";
            File.AppendAllTextAsync(logFile, logFormat);
        }
        catch (Exception ex)
        {
            var unkErr = ex.Message;
        }

    }

    public static void Logger(LogType logType, string functionSubName, string message, string extraData = "")
    {
        try
        {
            if (!Directory.Exists(logRoot)) Directory.CreateDirectory(logRoot);
            if (!File.Exists(logFile)) File.Create(logFile).Dispose();
            string logFormat = $"#{DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss")}#   [{logType.ToString()}]  CustomMessage={extraData}  Name=({functionSubName}) Message={message}\n";
            File.AppendAllTextAsync(logFile, logFormat);
        }
        catch (Exception ex)
        {
            var unkErr = ex.Message;
        }

    }


    public static bool ClearLogFile()
    {
        try
        {
            if (File.Exists(logFile)) File.Delete(logFile);
            return true;
        }
        catch (Exception ex)
        {
            Logger(LogType.error, nameof(ClearLogFile), ex);
            return false;
        }
     
    }

    public static List<String> GetPluginFolderContent()
    {
        try
        {
            var fList = new List<string>();
            if (Directory.Exists(pluginFolder))
            {
                foreach (var item in Directory.GetFiles(pluginFolder,"*.*"))
                {
                     fList.Add(Path.GetFileName(item.Trim()));
                }
            }

            return fList;
        }
        catch (Exception ex)
        {
            var unkErr = ex.Message;
            return new();
        }

    }


    public static List<String> GetLogs()
    {
        try
        {
            if (File.Exists(logFile))
            {
                var ret = File.ReadAllLines(logFile);
                return ret.ToList();
            }

            return new();
        }
        catch (Exception ex)
        {
            var unkErr = ex.Message;
            return new();
        }

    }
}
