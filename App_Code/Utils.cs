using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Class1
/// </summary>
public class Utils
{
    public static String LogDirectory()
    {
        return ConfigurationManager.AppSettings["LogDirectory"];
    }

    public static void LogGlobal(string line) {
        LogToFile(line, "Global-" + DateTime.Now.ToLongDateString() + ".log");
    }

    public static void LogToFile(string line, string file)
    {
        Directory.CreateDirectory(LogDirectory());

        using (StreamWriter w = File.AppendText(LogDirectory() + file))
        {
            w.WriteLine("{0:dd/MM/yy\tHH:mm:ss}\t{1}", DateTime.Now, line);
        }
    }
}