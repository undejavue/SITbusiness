using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Diagnostics;


namespace unServer
{
    public class unServerCore
    {

        private TcpListener listener;
        private int portClient;

        private string serverReply;
        private Thread listenerThread;

        public unServerCore(int port = 7879, string message="Нет сведений")
        {
            this.portClient = port;
            this.serverReply = message;
        }

        public void Start()
        {
            listenerThread = new Thread(unListenerThread);
            listenerThread.IsBackground = true;
            listenerThread.Name = "unListener";
            listenerThread.Start();
        }

        protected void unListenerThread()
        {
            try
            {
                IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
                listener = new TcpListener(ipAddress, portClient);
                listener.Start();
                while (true)
                {
                    Socket clientSocket = listener.AcceptSocket();                 
                    UnicodeEncoding encoder = new UnicodeEncoding();
                    byte[] buffer = encoder.GetBytes(serverReply);
                    clientSocket.Send(buffer, buffer.Length,0);
                    clientSocket.Close();
                }
            }
            catch (SocketException ex)
            {
                Trace.TraceError(String.Format("unServer {0}", ex.Message));
            }
        }

        public void Stop()
        {
            listener.Stop();
        }

        public void Suspend()
        {
            listener.Stop();
        }

        public void Resume()
        {
            Start();
        }

    }
}
