using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace A4U3.Domain.Interfaces
{
    public interface IStaticData
    {
        IEnumerable<string> GetRobotList();
    }
}
