using System;
using System.Net;
using System.Net.Sockets;

namespace ChatServer
{
    internal class Server
    {
        private TcpListener? _tcpListener;
        private List<Client>? _clients = new List<Client> ();
        private string _messageToSend;

        public string MessageToSend
        {
            get { return _messageToSend; }
            set { _messageToSend = value; }
        }
        public TcpListener Listener => _tcpListener;
        public List<Client> Clients => _clients;

        public Server()
        {
            _tcpListener = new TcpListener(IPAddress.Any, 8888);
        }

        public async void StartServerAsync()
        {
            try
            {
                Listener.Start();

                while (true)
                {
                    Console.WriteLine("Waiting for a new connection...");
                    TcpClient tcpClient = Listener.AcceptTcpClient();
                    Client newClient = new Client(tcpClient, this);
                    Clients.Add(newClient);
                    Console.WriteLine("New user connected!");
                    Task.Run(newClient.ProcessAsync);
                }
            }
            catch
            {

            }
        }

        public void SendMessage(Client client, string message)
        {
            int senderIndex = Clients.IndexOf(client);
            foreach(Client clientToSend in Clients)
            {
                if (Clients.IndexOf(clientToSend) != senderIndex)
                {
                    clientToSend.Writer.WriteLine(message);
                    clientToSend.Writer.Flush();
                }
            }
        }

        public void StopServer()
        {
            Listener.Stop();
        }
    }
}
