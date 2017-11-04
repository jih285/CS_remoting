using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemotingObjects
{
    public interface IServerRemo
    {
        void broadcast(String msg);
        String RegistClient(String name, IClient Client);
        void Send(String msg, String roomname);
        String threadisfine(String msg);
        String getMsgFromRoom(String name, String room);
        String CreateRoom(String name);
        String joinRoom(String name, String room);
        DateTime msgUpdatedTime(String room);
        String getallrooms();
    }
}
