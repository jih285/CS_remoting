using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemotingObjects
{
    public class ServerRemoImp : MarshalByRefObject, IServerRemo
    {
        ArrayList Clients = new ArrayList();
        String singlemsg = "heihei";
        ArrayList rooms = new ArrayList();

        public ServerRemoImp()
        {
            Console.WriteLine("Server is on");
        }

        public void broadcast(String msg)
        {
            foreach (IClient client in Clients) {
                client.sendmsg(msg);
            }
        }

        public string CreateRoom(string name)
        {
            this.rooms.Add(new room(name));
            return "room: " + name + "has been created";
        }

        public void Send(String msg, String roomname)
        {
            Boolean roomfind = false;
            foreach (room room in this.rooms) {
                if (room.name.Equals(roomname)) {
                    room.insertMsg(msg);
                    roomfind = true;
                }
            }
            if (!roomfind) {
                Console.WriteLine("Error, can't find this room!");
            }
        }

        public string getMsgFromRoom(string name, string room)
        {
            foreach (room aroom in this.rooms) {
                if (aroom.name.Equals(room)){
                    return aroom.getMsg(name);
                }

            }
            return "Error! cant find the room";
        }

        public String RegistClient(String name, IClient Client)
        {
            Clients.Add(Client);
            return "hello, welcome to ji's chat lobby!";
        }

        public string threadisfine(string msg)
        {
            //return "yes, thread is oooosome! " + msg;
            return "yes, thread is oooosome! " + this.singlemsg;
        }

        public string joinRoom(string name, string room)
        {
            //Boolean roomfind = false;
            foreach (room aroom in this.rooms)
            {
                if (aroom.name.Equals(room))
                {
                    return aroom.insertMember(name);
                    //roomfind = true;


                }
            }
            return "this room do not existed!";
        }

        public DateTime msgUpdatedTime(string room)
        {
            foreach (room aroom in this.rooms)
            {
                if (aroom.name.Equals(room))
                {
                    return aroom.getUpdatedTime();
                }
            }
            return new DateTime(2008, 1, 1, 6, 32, 0);
        }
    }
}
