using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportConnect
{
    public class Message
    {
        public Message(string username, string message, int eventId)
        {
            this.Username = username;
            this.MessageText = message;
            this.EventId = eventId;
        }
        public string Username { set; get; }
        public string MessageText { set; get; }
        public int EventId { set; get; }

    }
}
