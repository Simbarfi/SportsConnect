using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SportConnect.Talk;

namespace SportConnect
{
    public class Talk : ObservableCollection<Message>
    {
        public Talk()
        {
            // Example of content for the Chat. 
            this.Add(new Message() { Sender = "Sam", Content = "Hi, did you sign up for the upcoming pool tournament?" });
            this.Add(new Message() { Sender = "Daniel", Content = "I signed up for the one in July" });
            this.Add(new Message() { Sender = "Sam", Content = "We are short of one player. Would you like to join?" });
            this.Add(new Message() { Sender = "Daniel", Content = "Count me in!! I was looking for a team to join" });
            this.Add(new Message() { Sender = "Sam", Content = "Awesome!! Broskiii we going to win. We got a good team this year" });

        }

        public class Message
        {
            public string Sender { get; set; }
            public string Content { get; set; }
        }

    }
}
