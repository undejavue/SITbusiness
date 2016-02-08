using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace testNotifyServer
{
    class Program
    {
        static void Main(string[] args)
        {
            //NotifyServer.NotifyServerCore server = new NotifyServer.NotifyServerCore();

            //server.Start();

            NotifyServerClient.unClientClass notifyClient = new NotifyServerClient.unClientClass();
            notifyClient.sendNotify("Добавлен один новый паспорт в базу устройств");

            Console.WriteLine("Для завершения нажмите Enter...");

            Console.ReadKey();
        }
    }
}
