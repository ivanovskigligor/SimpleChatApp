using ChatAppClient.Net.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatAppServer
{
    class Client
    {
        public string Username { get; set; }
        public Guid UID { get; set; }
        public TcpClient ClientSocket { get; set; }

        PacketReader _packetReader;
        public Client(TcpClient client) 
        {
            ClientSocket = client;
            UID = Guid.NewGuid();
            
            _packetReader = new PacketReader(ClientSocket.GetStream());

            var opcode = _packetReader.ReadByte();
            
            // since this is first packet we recieve validate opcode is 0 and if not drop connection?
            
            Username = _packetReader.ReadMessaage();

            Console.WriteLine($"[{DateTime.Now}]: Client has connected with the username {Username}");

            // different thread so we dont get caught up on the while loop
            Task.Run(() => Process());
        }

        void Process()
        {
            while(true)
            {
                try
                {
                    var opcode = _packetReader.ReadByte();
                    switch (opcode)
                    {
                        case 5:
                            var msg = _packetReader.ReadMessaage();
                            Console.WriteLine($"[{DateTime.Now}] : Message recieved! {msg}");
                            Program.BroadcastMessage($"[{DateTime.Now}]: [{Username}] : {msg}");
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine($"[{UID}] : Disconnected");
                    Program.BroadcastDisconnectMessage(UID.ToString());
                    ClientSocket.Close();
                    break;
                }
            }
        }
    }
}
