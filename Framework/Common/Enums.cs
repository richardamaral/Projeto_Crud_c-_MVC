using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Common
{
    internal class Enums
    { //SERVIDOR AWS AMAZON S3
        public enum ActionS3AWS : int
        {
            SalveFile = 1,
            DeleteFile,
            DeleteFiles
        }
    }
}
