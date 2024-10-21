using ChatAppClient.MVVM.Core;
using ChatAppClient.MVVM.Model;
using ChatAppClient.Net;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ChatAppClient.MVVM.ViewModel
{
    internal class MainViewModel
    {
        public ObservableCollection<UserModel> Users{ get; set; }
        public ObservableCollection<string> Messages{ get; set; }
        public RelayCommand ConnectToServerCommand { get; set; }
        public RelayCommand SendMessageCommand { get; set; }
        public string Username { get; set; }
        public string Message { get; set; }
        private Server _server;
        public MainViewModel() {
        
            Users = new ObservableCollection<UserModel>();
            Messages = new ObservableCollection<string>();
            _server = new Server();
            _server.connectedEvent += UserConnected;
            _server.messageRecievedEvent += MessageRecieved;
            _server.connectedEvent += RemoveUsers;


            ConnectToServerCommand = new RelayCommand(o => _server.ConnectToServer(Username), o => !string.IsNullOrEmpty(Username));
            Console.WriteLine("KUR" + Message);
            SendMessageCommand = new RelayCommand(o => _server.SendMessageToServer(Message), o => !string.IsNullOrEmpty(Message));
        }

        private void RemoveUsers()
        {
            var uid = _server.PacketReader.ReadMessaage();
            var user = Users.Where( x => x.UID == uid).FirstOrDefault();
            Application.Current.Dispatcher.Invoke(() => Users.Remove(user));

        }

        private void MessageRecieved()
        {
            var msg = _server.PacketReader.ReadMessaage();
            Application.Current.Dispatcher.Invoke(() => Messages.Add(msg));
        
        }


        private void UserConnected()
        {
            var user = new UserModel
            {
                Username = _server.PacketReader.ReadMessaage(),
                UID = _server.PacketReader.ReadMessaage(),
            };

            if (!Users.Any(x => x.UID == user.UID))
            {
                Application.Current.Dispatcher.Invoke(() => Users.Add(user));
            }
        }
    }
}
