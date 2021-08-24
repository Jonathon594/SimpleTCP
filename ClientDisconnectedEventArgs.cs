using System;
using System.Collections.Generic;
using System.Text;

namespace Jonathon594.SimpleTCP
{
    /// <summary>
    /// Contains the <see cref="ServerClient"/> instance that disconnected.
    /// </summary>
    public class ClientDisconnectedEventArgs : EventArgs
    {
        /// <summary>
        /// The <see cref="ServerClient"/> that disconnected.
        /// </summary>
        public ServerClient Client { get; set; }

        /// <summary>
        /// Creates a new <see cref="EventArgs"/> for <see cref="Server.ClientDisconnected"/>
        /// </summary>
        /// <param name="serverClient">The <see cref="ServerClient"/> that disconnected.</param>
        public ClientDisconnectedEventArgs(ServerClient serverClient)
        {
            Client = serverClient;
        }
    }
}
