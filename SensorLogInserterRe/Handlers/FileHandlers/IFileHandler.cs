using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorLogInserterRe.Handlers.FileHandlers
{
    interface IFileHandler
    {
        List<string> CheckFiles(string folderPass, DateTime startDate, DateTime endDate);
    }
}
