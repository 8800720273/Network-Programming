using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace MultipleContClient
{
    class Client11
    {
        public string ID
        {
            get;
            private set;
        }
        public IPEndPoint EndPoint
        {
            get;
            private set;
        }
        Socket sck;
        public Client11(Socket accepted)
        {
            sck = accepted;
            ID = Guid.NewGuid().ToString();
            EndPoint = (IPEndPoint)sck.RemoteEndPoint;
            sck.BeginReceive(new byte[] { 0 }, 0, 0, 0, callback, null);
        }
        void callback(IAsyncResult ar)
        {
            try
            {
                sck.EndReceive(ar);
                byte[] buf = new byte[8192];
                int rec = sck.Receive(buf, buf.Length, 0);
                if(rec < buf.Length)
                {
                    Array.Resize<byte>(ref buf, rec);
                }
                if(Received != null)
                {
                    Received(this, buf);
                }
                sck.BeginReceive(new byte[] { 0 }, 0, 0, 0, callback, null);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                Close();
                if(Disconnect != null)
                {
                    Disconnect(this);
                }
            }
        }
        public void Close()
        {
            sck.Close();
            sck.Dispose();
        }
        public delegate void ClientReceivedHandler(Client11 sender, byte[] data);
        public delegate void ClientDisconnectedHandler(Client11 sender);
        public event ClientReceivedHandler Received;
        public event ClientDisconnectedHandler Disconnect;
    }
}
