using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemotingObjects
{
    public interface IClient
    {
        void sendmsg(String msg);
    }
}
