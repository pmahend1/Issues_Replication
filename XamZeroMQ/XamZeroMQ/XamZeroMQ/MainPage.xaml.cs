using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XamZeroMQ
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private Server server;
        private  void ButtonServer_OnClicked(object sender, EventArgs e)
        {
            //
            server = new Server();
              //  server.SendMessage();
              server.HelloWorld();
           //await DisplayAlert("Server", y.ToString(), "Ok");
        }

        private  void ButtonClient_OnClicked(object sender, EventArgs e)
        {
            //var x = Client.ReceiveMessage();
            //await DisplayAlert("Update", x.Item1 + ".... " + x.Item2, "Ok");
            var client = new Client();
            client.HelloWorld();
        }

        private void ButtonCancel_OnClicked(object sender, EventArgs e)
        {
            server.Cancel = true;
        }
    }
}
