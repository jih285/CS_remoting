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

                    if (word[0].Equals("jcreate")) {
                        Console.WriteLine(this.server.CreateRoom(word[1]));
                    }
                    else if (word[0].Equals("jjoin"))
                    {
                        String res= this.server.joinRoom(this.name, word[1]);
                        Console.WriteLine(res);
                        string[] result = res.Split();
                        if (result[0].Equals("this"))
                        {
                            Console.WriteLine("you could create this room");
                        }
                        else {
                            this.myrooms.Add(word[1],DateTime.Now);
                        }
                    }
                    else if (word[0].Equals("jswitch")) {
                        this.currentRoom = word[1];
                    }
                    else {
                        if (this.currentRoom.Equals("")) {
                            Console.WriteLine("Please switch to a room first. Thank you");
                        }
                        else { 
                            this.server.Send(msg,this.currentRoom);
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
            String msg = "";
            Console.WriteLine("Please enter your name: ");
            Client user = new Client("li");
            //Console.WriteLine(user.server.foo(user.name));
            //user.server.RegistClient(client.name, client);
            //server.RegistClient(client.name, client);
            //Console.WriteLine(server.RegistClient(client.name,client));

        }


    }
}