using NetMQ;
using NetMQ.Sockets;
using System;
using System.Diagnostics;
using System.Threading;

using Timer = System.Timers.Timer;

namespace XamZeroMQ
{
    public class Server
    {
        public bool Cancel { get; set; }
        public bool SendMessage()
        {

            var netMqTimer = new NetMQTimer(100);
            var timer = new Timer(180000);
            timer.Start();
            timer.Elapsed += (sender, args) =>
            {
                this.Cancel = true;

                timer.Stop();
            };

            using (var publisher = new PublisherSocket())
            {
                publisher.Bind("tcp://*:3245");
                Thread.Sleep(3000);
                var rng = new Random();

                while (!Cancel)
                {

                    var zipcode = 10001;//rng.Next(0, 99999);
                    var temperature = rng.Next(-80, 135);
                    var humidity = rng.Next(0, 90);

                    publisher.SendFrame($"{zipcode} {temperature} {humidity}");
                }


            }

            return true;
        }

        public void HelloWorld()
        {


            var timer = new Timer(60000);
            timer.Start();
            timer.Elapsed += (sender, args) =>
            {
                this.Cancel = true;

                timer.Stop();
            };
            // Create
            const string endpoint = "tcp://*:4505";
            using (var request = new RequestSocket())
            {
                request.Bind(endpoint);


                Thread.Sleep(2000);
                while (true)
                {
                    request.SendFrame("Requester says hello");
                    var reply = request.ReceiveFrameString();
                    Debug.WriteLine("Gets reply " +reply);
                }
            }

        }


    }
}
