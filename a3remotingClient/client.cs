using RemotingObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace RemotingObjects
{
    class Client
    {
        public IServerRemo server;
        public String name;
        public String currentRoom;
        public Dictionary<string, DateTime> myrooms;
        public DateTime old=DateTime.Now;

        public Client(String cname) {
            this.name = cname;
            this.currentRoom = "";
            this.myrooms = new Dictionary<string, DateTime>();

            TcpChannel channel = new TcpChannel();
            ChannelServices.RegisterChannel(channel, false);
            //RemotingConfiguration.RegisterWellKnownServiceType(typeof(RemotingObjects.IClient), "RemotingClient", WellKnownObjectMode.SingleCall);
            this.server = (IServerRemo)Activator.GetObject(typeof(RemotingObjects.IServerRemo), "tcp://localhost:8080/RemotingPersonService");
            if (server == null)
            {
                Console.WriteLine("Couldn't create Remoting Object 'Person'.");
            }

            Console.WriteLine("Hello welcome to Ji's chat lobby.\nYou could choose a chat room to join or create a new one.\n" + "useful commands:\n 1. jcreate [roomname]\n 2. jjoin [roomname] //receive messages from a chatroom, but can't speak\n 3. jswitch [roomname] //after join  a chatroom, you can siwtch to this room to say something\n 4. jleave [roomname]//you won't receive any message from this room\n 5. jshowrooms //show all existing rooms\n 6. jshowmyrooms //show the rooms that you join\n 7. rename [newname]");
            
            String msg = "";
            String[] word;
            Thread t = new Thread(rrrun);
            t.Start();
            while (true)
            {
                try
                {
                    msg = Console.ReadLine();
                    word = msg.Split();

                    if (word[0].Equals("jcreate"))
                    {
                        Console.WriteLine(this.server.CreateRoom(word[1]));
                    }
                    else if (word[0].Equals("jjoin"))
                    {
                        String res = this.server.joinRoom(this.name, word[1]);
                        Console.WriteLine(res);
                        string[] result = res.Split();
                        if (result[0].Equals("this"))
                        {
                            Console.WriteLine("you could create this room");
                        }
                        else
                        {
                            this.myrooms.Add(word[1], DateTime.Now);
                        }
                    }
                    else if (word[0].Equals("jswitch"))
                    {
                        Boolean findit = false;
                        foreach (String room in this.myrooms.Keys.ToList()) {
                            if (room.Equals(word[1])) {
                                this.currentRoom = word[1];
                                Console.WriteLine("you have switched to room: " + room);
                                findit=true;
                            }
                        }
                        if (!findit) {
                            Console.WriteLine("you have to join it first");
                        }
                        
                    }
                    else if (word[0].Equals("jshowrooms"))
                    {
                        Console.WriteLine(this.server.getallrooms());
                    }
                    else if (word[0].Equals("jshowmyrooms"))
                    {
                        string myroomlist = "rooms: ";
                        foreach (String room in this.myrooms.Keys.ToList())
                        {
                            myroomlist += room + " ";
                        }
                        Console.WriteLine(myroomlist);
                    }
                    else if (word[0].Equals("jleave")) {
                        if (this.currentRoom.Equals(word[1])){
                            this.currentRoom = "";
                        }
                        Boolean deleteroomfind = false;
                        foreach (String room in this.myrooms.Keys.ToList())
                        {
                            if (room.Equals(word[1]))
                            {
                                this.myrooms.Remove(word[1]);
                                deleteroomfind = true;
                                Console.WriteLine("room: " + word[1]+" has been removed");
                            }
                        }
                        if (!deleteroomfind) {
                            Console.WriteLine("you haven't join this room yet");
                        }
                    }
                    else if (word[0].Equals("jrename")){
                        this.name = word[1];
                        Console.WriteLine("your name has been reset as: "+this.name);
                    }
                    else
                    {
                        if (this.currentRoom.Equals(""))
                        {
                            Console.WriteLine("Please switch to a room first. Thank you");
                        }
                        else
                        {
                            msg = this.name + " from [" + this.currentRoom + "] said: " + msg;
                            this.server.Send(msg, this.currentRoom);
                        }
                    }
                    //Console.WriteLine(this.server.foo(msg));
                    //Console.WriteLine(this.server.threadisfine(""));
                    //user.server.RegistClient(client.name,client);
                    //server.broadcast(msg);
                }
                catch (System.Net.Sockets.SocketException e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
        }
        private static void MyElapsedMethod(Client source, ElapsedEventArgs e)
        {
            foreach (String room in source.myrooms.Keys.ToList())
            {
                DateTime newtime = source.server.msgUpdatedTime(room);
                if (DateTime.Compare(source.myrooms[room], newtime) < 0)
                {
                    Console.WriteLine(source.server.getMsgFromRoom(source.name, room));
                    source.myrooms[room] = newtime;
                }
            }

            /*
            String result = source.server.getMsgFromRoom(source.name, source.currentRoom);
            if (!source.currentRoom.Equals("")&&!result.Equals("")) {
                if (!result.Equals(source.old)) { 
                Console.WriteLine(result);
                }
            }
            source.old = result;
            */
        }
        public void rrrun() {
            System.Timers.Timer aTimer = new System.Timers.Timer();
            aTimer.Elapsed += (sender, e) => MyElapsedMethod(this, e);
            aTimer.Interval = 200;
            aTimer.Enabled = true;
        }

        static void Main(string[] args)
        {
            
            //Clientimp client = new Clientimp();
            String name = "";
            Console.WriteLine("Please enter your name: ");
            name = Console.ReadLine();
            Client user = new Client(name);
            //Console.WriteLine(user.server.foo(user.name));
            //user.server.RegistClient(client.name, client);
            //server.RegistClient(client.name, client);
            //Console.WriteLine(server.RegistClient(client.name,client));

        }


    }
}