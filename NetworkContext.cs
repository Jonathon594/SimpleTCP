namespace Jonathon594.SimpleTCP
{
    /// <summary>
    /// A class holding contextual information for use in <see cref="Packet.Handle(NetworkContext, NetworkDirection)"/>.
    /// </summary>
    public class NetworkContext
    {
        private ServerClient clientSender;
        private Server server;

        /// <summary>
        /// The <see cref="ServerClient"/> who sent the packet.
        /// </summary>
        public ServerClient ClientSender { get => clientSender; set => clientSender = value; }
        /// <summary>
        /// The <see cref="Server"/> the packet was recieved on.
        /// </summary>
        public Server Server { get => server; set => server = value; }

        internal NetworkContext(ServerClient serverClient, Server server)
        {
            ClientSender = serverClient;
            Server = server;
        }

        internal NetworkContext()
        {
            ClientSender = null;
            server = null;
        }
    }
}