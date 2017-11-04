using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemotingObjects
{
    class room
    {
        public String name;
        public String msg;
        public DateTime msgTimeStamp;
        //public Dictionary<string, Boolean> members;
        public ArrayList members;
        public int CountMembers;
        public String chatlog;

        public room(String name) {
            this.name = name;
            this.msg = "";
            //this.members = new Dictionary<string, Boolean>();
            this.members = new ArrayList();
            this.CountMembers = 0;
            this.msgTimeStamp = new DateTime();
            this.chatlog = "";
        }
        public void insertMsg(string msg) {
            this.msg = msg;
            this.msgTimeStamp = DateTime.Now;
            this.chatlog += msg + "\n";
            /*
            this.CountMembers = members.Count;
            foreach (String member in this.members.Keys.ToList()) {
                this.members[member] = true;
            }
            */
        }
        public DateTime getUpdatedTime() {
            return this.msgTimeStamp;
        }
        public String getMsg(String user) {

            /*if (this.msg.Equals("")||this.CountMembers==0||!this.members[user])
            if (!this.members[user])
            {
                return "";
                //return this.msg;
            }
            else {

                this.members[user] = false;
                this.CountMembers--;
            }
            */

            return this.msg;
        }
        public String insertMember(String name) {
            //this.members[name] = true;
            this.members.Add(name);
            return "you have joined room: " + this.name+"\n ------------chat log------------ \n"+this.chatlog;
        }
    }
}
