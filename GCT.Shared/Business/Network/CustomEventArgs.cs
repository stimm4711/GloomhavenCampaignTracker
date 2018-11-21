using System;

namespace Business.Network
{
    public class ClientConnectedEventArgs : EventArgs
    {
        public ClientConnectedEventArgs(string s)
        {
            Message = s;
        }

        public string Message { get; set; }
    }
}
