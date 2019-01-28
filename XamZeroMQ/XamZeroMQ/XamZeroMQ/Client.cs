using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Threading;
using NetMQ;
using NetMQ.Sockets;

using Timer = System.Timers.Timer;

namespace XamZeroMQ
{
   public  class Client
    {
        public bool Cancel { get; set; }
        public  Tuple<string,string> ReceiveMessage()
        {
           // Console.Title = "NetMQ Weather Update Client";

            const int zipToSubscribeTo = 10001;
            const int iterations = 1000;

            var totalTemp = 0;
            var totalHumidity = 0;

            //Console.WriteLine("Collecting updates for weather service for zipcode {0}...", zipToSubscribeTo);
            var timer = new Timer(15000);
            using (var subscriber = new SubscriberSocket())
            {
                subscriber.Connect("tcp://PC_ip:3245");
                Thread.Sleep(2000 );
                subscriber.Subscribe(zipToSubscribeTo.ToString(CultureInfo.InvariantCulture));
                timer.Start();
                timer.Elapsed += (sender, args) =>
                {
                    timer.Stop();
                    Cancel = true;

                };
                while (!Cancel)
                {
                    var results = subscriber.ReceiveFrameString();
                    // Console.Write(".");

                    // "zip temp relh" ... "10001 84 23" -> ["10001", "84", "23"]

                    var split = results.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    var zip = int.Parse(split[0]);
                    if (zip != zipToSubscribeTo)
                    {
                        throw new Exception($"Received message for unexpected zipcode: {zip} (expected {zipToSubscribeTo})");
                    }

                    totalTemp += int.Parse(split[1]);
                    totalHumidity += int.Parse(split[2]);
                }
  


            }
          return  new Tuple<string, string>((totalTemp / iterations).ToString(), (totalHumidity / iterations).ToString());
        }

        public void HelloWorld()
        {
            var timer = new Timer(60000);
            const string endpoint = "tcp://PC_ip:4505";
            //10.151.111.166       12.207.170.27
            timer.Start();
            timer.Elapsed += (sender, args) =>
            {
                timer.Stop();
                Cancel = true;
            };

            using (var replierSocket = new ResponseSocket())
            {

                replierSocket.Connect(endpoint);
                Thread.Sleep(2000);
                while (true)
                {
                    var replyFromRequester = replierSocket.ReceiveFrameString();
                    Debug.WriteLine("Got reply " + replyFromRequester);
                    replierSocket.SendFrame("Response socket say hello");

                }
            }
        }


    }
}
