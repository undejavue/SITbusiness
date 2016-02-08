using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Diagnostics;

namespace NotifyServer
{
    public class NotifyServerCore
    {
        private TcpListener listenerLocal;
        private Thread listenerThreadLocal;
        private int PortLocal;
        private string ServerLocal;

        private TcpListener listenerRemote;
        private Thread listenerThreadRemote;
        private int PortRemote;

        private string receivedMessage = "Нет данных!";

        public NotifyServerCore()
        {
            this.PortLocal = Properties.Settings.Default.localNotifyPort;
            this.PortRemote = Properties.Settings.Default.remoteClientPort;
            this.ServerLocal = Properties.Settings.Default.serverAddress;
        }

        public void Start()
        {
            listenerThreadLocal = new Thread(ListenerThreadLocal);
            listenerThreadLocal.IsBackground = true;
            listenerThreadLocal.Name = "LocalNotifyListener";
            listenerThreadLocal.Start();

            listenerThreadRemote = new Thread(ListenerThreadRemote);
            listenerThreadRemote.IsBackground = true;
            listenerThreadRemote.Name = "RemoteNotifyListener";
            listenerThreadRemote.Start();
        }

        protected void ListenerThreadLocal()
        {
            try
            {
                IPAddress ipAddress = IPAddress.Parse(ServerLocal);
                listenerLocal = new TcpListener(ipAddress, PortLocal);
                listenerLocal.Start();
                while (true)
                {
                    Socket clientSocket = listenerLocal.AcceptSocket();
                    UnicodeEncoding encoder = new UnicodeEncoding();

                    byte[] buffer = new byte[1024];
                    clientSocket.Receive(buffer);
                    this.receivedMessage = encoder.GetString(buffer);

                    clientSocket.Close();
                }
            }
            catch (SocketException ex)
            {
                Trace.TraceError(String.Format("NotifyServerCore says: {0}", ex.Message));
            }
        }

        protected void ListenerThreadRemote()
        {
            try
            {
                IPAddress ipAddress = IPAddress.Parse(Properties.Settings.Default.serverAddress);
                listenerRemote = new TcpListener(ipAddress, PortRemote);
                listenerRemote.Start();
                while (true)
                {
                    Socket clientSocket = listenerRemote.AcceptSocket();
                    UnicodeEncoding encoder = new UnicodeEncoding();
                    byte[] buffer = encoder.GetBytes(receivedMessage);
                    clientSocket.Send(buffer, buffer.Length, 0);
                    clientSocket.Close();
                }
            }
            catch (SocketException ex)
            {
                Trace.TraceError(String.Format("NotifyServerCore says for remote: {0}", ex.Message));
            }
        }

        public void Stop()
        {
            //listenerLocal.Stop();
            listenerRemote.Stop();
        }

        public void Suspend()
        {
            //listenerLocal.Stop();
            listenerRemote.Stop();
        }

        public void Resume()
        {
            Start();
        }

    }
}
