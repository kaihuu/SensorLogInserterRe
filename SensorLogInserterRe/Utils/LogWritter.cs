using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorLogInserterRe.Utils
{
    class LogWritter
    {
        public enum LogMode
        {
            Search,
            Gps,
            Acc,
            Trip,
            Ecolog,
            Error,
            Elapsedtime
        }

        private static readonly string DirectoryName = "LogFiles";

        public static void WriteLog(LogMode mode, string text)
        {
            //LogWritter.CreateDirectory();
            //string fileName;

            //switch (mode)
            //{
            //    case LogMode.Search:

            //        fileName = $@"{DirectoryName}\{DateTime.Now.ToString("yyyy-MM-dd")}\{DateTime.Now.ToString("HH")}_search.log";

            //        using (StreamWriter writer = new StreamWriter(fileName, true))
            //        {
            //            writer.WriteLine(DateTime.Now + " : " + text);
            //        }
            //        break;

            //    case LogMode.Gps:

            //        fileName = $@"{DirectoryName}\{DateTime.Now.ToString("yyyy-MM-dd")}\{DateTime.Now.ToString("HH")}_gps.log";

            //        using (StreamWriter writer = new StreamWriter(fileName, true))
            //        {
            //            writer.WriteLine(DateTime.Now + " : " + text);
            //        }
            //        break;

            //    case LogMode.Acc:

            //        fileName = $@"{DirectoryName}\{DateTime.Now.ToString("yyyy-MM-dd")}\{DateTime.Now.ToString("HH")}_acc.log";

            //        using (StreamWriter writer = new StreamWriter(fileName, true))
            //        {
            //            writer.WriteLine(DateTime.Now + " : " + text);
            //        }
            //        break;

            //    case LogMode.Trip:

            //        fileName = $@"{DirectoryName}\{DateTime.Now.ToString("yyyy-MM-dd")}\{DateTime.Now.ToString("HH")}_trip.log";

            //        using (StreamWriter writer = new StreamWriter(fileName, true))
            //        {
            //            writer.WriteLine(DateTime.Now + " : " + text);
            //        }
            //        break;

            //    case LogMode.Ecolog:

            //        fileName = $@"{DirectoryName}\{DateTime.Now.ToString("yyyy-MM-dd")}\{DateTime.Now.ToString("HH")}_ecolog.log";

            //        using (StreamWriter writer = new StreamWriter(fileName, true))
            //        {
            //            writer.WriteLine(DateTime.Now + " : " + text);
            //        }
            //        break;

            //    case LogMode.Error:

            //        fileName = $@"{DirectoryName}\{DateTime.Now.ToString("yyyy-MM-dd")}\{DateTime.Now.ToString("HH")}_error.log";

            //        using (StreamWriter writer = new StreamWriter(fileName, true))
            //        {
            //            writer.WriteLine(DateTime.Now + " : " + text);
            //        }
            //        break;

            //    case LogMode.Elapsedtime:

            //        fileName = $@"{DirectoryName}\{DateTime.Now.ToString("yyyy-MM-dd")}\{DateTime.Now.ToString("HH")}_elapsedtime.log";

            //        using (StreamWriter writer = new StreamWriter(fileName, true))
            //        {
            //            writer.WriteLine(DateTime.Now + " : " + text);
            //        }
            //        break;


            //    default:
            //        break;
            //}
        }

        private static void CreateDirectory()
        {
            if (!Directory.Exists(DirectoryName))
                Directory.CreateDirectory(DirectoryName);

            if (!Directory.Exists($@"{DirectoryName}\{DateTime.Now.ToString("yyyy-MM-dd")}"))
                Directory.CreateDirectory($@"{DirectoryName}\{DateTime.Now.ToString("yyyy-MM-dd")}");
        }

    }
}
