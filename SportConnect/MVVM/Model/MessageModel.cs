using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportConnect.MVVM.Model
{
    class MessageModel
    {

        //This will pull the Username and other details of the chat that will displayed on the chat page
        public string Username { get; set; }
        public string UsernameColor { get; set; }

        public string ImageSource { get; set; }
        public string Message { get; set; }

        public DateTime Time { get; set; }

        public bool IsNativeOrigin { get; set; }

        public bool FirstMessage { get; set; }
    }
}
