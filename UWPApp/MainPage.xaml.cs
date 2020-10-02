using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UWPApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        private static readonly Random rnd = new Random();

        private DeviceClient deviceClient = DeviceClient.CreateFromConnectionString("IotLänk", TransportType.Mqtt); 



        public MainPage()
        {
            this.InitializeComponent();
            RecieveMessageAsynd().GetAwaiter();
        }

        private async Task SendMessageAsync()
        {
            while (true)
            {
                var data = new TemperatureModel
                {
                    Temperature = rnd.Next(20, 30),
                    Humidity = rnd.Next(40, 60)
                };

                //JSON = {"temperature": 20, "humidity":44
                var json = JsonConvert.SerializeObject(data);

                var payload = new Message(Encoding.UTF8.GetBytes(json));
                await deviceClient.SendEventAsync(payload);

                Console.WriteLine($"Message sent: {json}");
                await Task.Delay(60 * 1000);
            }
        }


        private async Task RecieveMessageAsynd()
        {
            while (true)
            {
                var payload = await deviceClient.ReceiveAsync();

                if (payload == null)               
                    continue;

                LvMessages.Items.Add(Encoding.UTF8.GetString(payload.GetBytes()));
                
               
            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await SendMessageAsync();
        }
    }
}
