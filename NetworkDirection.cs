namespace Jonathon594.SimpleTCP
{
    /// <summary>
    /// Indicates a direction on the network, ie <see cref="Server"/> to <see cref="Client"/> or vice-versa.
    /// The value indicates the recipient.
    /// </summary>
    public enum NetworkDirection
    {
        /// <summary>
        /// Indicates a <see cref="Packet"/> was sent to the server.
        /// </summary>
        Server,
        /// <summary>
        /// Indicates a <see cref="Packet"/> was sent to the client.
        /// </summary>
        Client
    }
}
