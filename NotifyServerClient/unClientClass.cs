using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace NotifyServerClient
{
    public class unClientClass
    {
        const int bufferSize = 1024;

        string serverName = Properties.Settings.Default.ServerName;
        int port = Properties.Settings.Default.ServerPort;

        TcpClient client = new TcpClient();      
        NetworkStream stream = null;

        public void sendNotify(string notify)
        {
            try
            {
                client.Connect(serverName, port);

                stream = client.GetStream();
                UnicodeEncoding encoder = new UnicodeEncoding();
                byte[] buffer = encoder.GetBytes(notify);

                stream.Write(buffer, 0, buffer.Length);

            }
            catch (SocketException ex)
            {
                Trace.TraceError(String.Format("unClientClass says: {0}", ex.Message));
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
                if (client.Connected)
                {
                    client.Close();
                }
            }
        }

    }
}
