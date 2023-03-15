using System.Net.Sockets;

namespace ChatServer
{
    internal class Client
    {
        private TcpClient? _client;
        private Server? _server;
        private StreamReader? _reader;
        private StreamWriter? _writer;

        public StreamReader Reader => _reader;
        public StreamWriter Writer => _writer;
        public TcpClient? TcpClient => _client;
        public Server? Server => _server;


        public Client(TcpClient client, Server server)
        {
            _client = client;
            _server = server;

            _reader = new StreamReader(client.GetStream());
            _writer = new StreamWriter(client.GetStream());
        }

        public async void ProcessAsync()
        {
            while(true)
            {
                try
                {
                    string messageFromClient = Reader.ReadLine();

                    if (!string.IsNullOrEmpty(messageFromClient))
                    {
                        Task.Run(() => Server.SendMessage(this, messageFromClient));
                    }
                }
                catch
                {
                    Console.WriteLine("User disconnected!");
                    break;
                }
            }
        }
    }
}
