using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Client1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter IP Address");
            string s1 = Console.ReadLine();
            Console.WriteLine("Enter  Name You Want To Chat");
            string s2 = Console.ReadLine();
            //Console.WriteLine("Enter Port Number");
            //int s3 = Convert.ToInt32(Console.ReadLine());
            Socket sck = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(s1), 43000);
            sck.Connect(endPoint);
            Console.WriteLine("Connected.");
            for (int i = 0; i < 100; i++)
            {
                Console.WriteLine("Enter Message");
                string msg = Console.ReadLine();
                byte[] msgBuffer = Encoding.Default.GetBytes(msg);
                sck.Send(msgBuffer, 0, msgBuffer.Length, 0);
                byte[] buffer = new byte[225];
                int rec = sck.Receive(buffer, 0, buffer.Length, 0);
                Array.Resize(ref buffer, rec);
                Console.WriteLine("<<"+s2+">>"+Encoding.Default.GetString(buffer));

            }
            Console.Read();
        }
    }
}
