using System;

namespace Jonathon594.SimpleTCP
{
    /// <summary>
    /// Contains the <see cref="ServerClient"/> instance that connected.
    /// </summary>
    public class ClientConnectedEventArgs : EventArgs
    {
        /// <summary>
        /// The <see cref="ServerClient"/> that connected.
        /// </summary>
        public ServerClient Client { get; set; }

        /// <summary>
        /// Creates a new <see cref="EventArgs"/> for <see cref="Server.ClientConnected"/>
        /// </summary>
        /// <param name="serverClient">The <see cref="ServerClient"/> that connected.</param>
        public ClientConnectedEventArgs(ServerClient serverClient)
        {
            Client = serverClient;
        }
    }
}
