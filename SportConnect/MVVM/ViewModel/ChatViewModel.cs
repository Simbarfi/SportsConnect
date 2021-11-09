using SportConnect.Core;
using SportConnect.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportConnect.MVVM.ViewModel
{
    class ChatViewModel : OberservableObject
    {
        public ObservableCollection<MessageModel> Messages { get; set; }

        public ObservableCollection<ContactModel> Contacts { get; set; }

        /* Commands */
        public RelayCommand SendCommand { get; set; }

        private ContactModel _selectedContact;


        // 
        public ContactModel SelectedContact
        {
            get { return _selectedContact; }
            set
            {
                _selectedContact = value;
                OnPropertyChanged();
            }
        }



        private string _message;

        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
                OnPropertyChanged();
            }
        }


        public ChatViewModel()
        {
            Messages = new ObservableCollection<MessageModel>();
            Contacts = new ObservableCollection<ContactModel>();

            SendCommand = new RelayCommand(o =>
            {
                Messages.Add(new MessageModel
                {
                    Message = Message,
                    FirstMessage = false
                });

                Message = "";
            });

            /*Example of chat that has taken place. 
             * Will be replaced with real time chat 
             * Message will get username, message 
            */

            Messages.Add(new MessageModel
            {
                Username = "Soccer Event 10/21/24",
                IsNativeOrigin = true,
                FirstMessage = true
            });

            for (int i = 0; i < 1; i++)
            {
                Messages.Add(new MessageModel
                {

                    Username = "DragonFly35",
                    UsernameColor = "#409aff",
                    ImageSource = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQOVU7QwAUz6IRXEorrT5Cn8tdnMLgSA6besg&usqp=CAU",
                    Message = "I plan to go around 2pm",
                    Time = DateTime.Now,
                    IsNativeOrigin = false,
                    FirstMessage = true
                });
                Messages.Add(new MessageModel
                {
                    Username = "RonaldoWooo",
                    UsernameColor = "#409aff",
                    ImageSource = "C:/Users/Sharad Ranpara/source/repos/TestChat/Zebra.jpg",
                    Message = "Same here going at 2 for shooting practice",
                    Time = DateTime.Now,
                    IsNativeOrigin = false,
                    FirstMessage = true
                });
                Messages.Add(new MessageModel
                {
                    Username = "Messi91",
                    UsernameColor = "#409aff",
                    ImageSource = "C:/Users/Sharad Ranpara/source/repos/TestChat/Lion.jpg",
                    Message = "I may come a little late!",
                    Time = DateTime.Now,
                    IsNativeOrigin = false,
                    FirstMessage = true
                });
                Messages.Add(new MessageModel
                {
                    Username = "BearStillSuck",
                    UsernameColor = "#409aff",
                    ImageSource = "C:/Users/Sharad Ranpara/source/repos/TestChat/lamborghini.jpg",
                    Message = "What time is the game starting?",
                    Time = DateTime.Now,
                    IsNativeOrigin = false,
                    FirstMessage = true
                });
            }


            for (int i = 0; i < 5; i++)
            {
                /*This will show the title of the event with the chat details */
                Contacts.Add(new ContactModel
                {
                    Username = $"Soccer Event 10/21/24{i}",
                    ImageSource = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQOVU7QwAUz6IRXEorrT5Cn8tdnMLgSA6besg&usqp=CAU",
                    Messages = Messages
                });
            }

        }
    }
}
