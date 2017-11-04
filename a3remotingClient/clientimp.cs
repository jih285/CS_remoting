using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemotingObjects
{
    [Serializable]
    class Clientimp :IClient
    {
        public String name;
        public Clientimp()
        {
            this.name = "";
        }
        public void sendmsg(string msg)
        {
            Console.WriteLine(msg);
        }
    }
}
