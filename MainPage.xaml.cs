// Copyright (c) Microsoft. All rights reserved.

using Microsoft.Azure.Devices.Client;
using Microsoft.Devices.Tpm;
using System;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Gpio;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Buildy
{
    public sealed partial class MainPage : Page
    {
        private SolidColorBrush redBrush = new SolidColorBrush(Windows.UI.Colors.Red);
        private SolidColorBrush greenBrush = new SolidColorBrush(Windows.UI.Colors.Green);
        private SolidColorBrush blueBrush = new SolidColorBrush(Windows.UI.Colors.Blue);

        private GpioPin redpin;
        private GpioPin greenpin;
        private GpioPin bluepin;

        
        public MainPage()
        {
            InitializeComponent();
            InitializeDevice();
            ReceiveEvents();
        }

        private void InitializeDevice()
        {
            var gpio = GpioController.GetDefault();

            if (gpio == null)
            {
                throw new Exception("There is no GPIO controller on this device.");
            }

            redpin = gpio.OpenPin(5);
            greenpin = gpio.OpenPin(6);
            bluepin = gpio.OpenPin(13);

            redpin.SetDriveMode(GpioPinDriveMode.Output);
            greenpin.SetDriveMode(GpioPinDriveMode.Output);
            bluepin.SetDriveMode(GpioPinDriveMode.Output);

            LightBlue();
        }
        private async Task ReceiveEvents()
        {
            var device = new TpmDevice(0);

            var iotHubUri = device.GetHostName();
            var deviceId = device.GetDeviceId();
            var sasToken = device.GetSASToken();

            var deviceClient = DeviceClient.Create
                (iotHubUri, AuthenticationMethodFactory.CreateAuthenticationWithToken(deviceId, sasToken));

            while (true)
            {
                var receivedMessage = await deviceClient.ReceiveAsync();
                if (receivedMessage == null) continue;

                var buildStatus = Encoding.UTF8.GetString(receivedMessage.GetBytes());

                await TranslateBuildToColor(buildStatus);
                await deviceClient.CompleteAsync(receivedMessage);
            }
        }
        private async Task TranslateBuildToColor(string buildStatus)
        {
            switch (buildStatus)
            {
                case "failed":
                    LightRed();                    
                    break;
                case "succeeded":
                    LightGreen();                    
                    break;
            }
        }

        private void LightBlue()
        {
            redpin.Write(GpioPinValue.Low);
            greenpin.Write(GpioPinValue.Low);
            bluepin.Write(GpioPinValue.High);
            LED.Fill = blueBrush;
        }
        private void LightGreen()
        {
            redpin.Write(GpioPinValue.Low);
            greenpin.Write(GpioPinValue.High);
            bluepin.Write(GpioPinValue.Low);
            LED.Fill = greenBrush;
        }
        private void LightRed()
        {
            redpin.Write(GpioPinValue.High);
            greenpin.Write(GpioPinValue.Low);
            bluepin.Write(GpioPinValue.Low);
            LED.Fill = redBrush;
        }


    }
}
