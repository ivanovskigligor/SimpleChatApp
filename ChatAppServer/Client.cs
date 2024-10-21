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

        }

    }
}
