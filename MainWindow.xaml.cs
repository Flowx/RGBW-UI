﻿using System;
using System.IO.Ports;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LED_Bottle
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static SerialPort _port;

        public MainWindow()
        {
            InitializeComponent();

            ReloadCOMList();

            Slider_R.IsEnabled = false;
            Slider_G.IsEnabled = false;
            Slider_B.IsEnabled = false;
            Slider_W.IsEnabled = false;
        }

        private void OnClosed(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (_port != null && _port.IsOpen) _port.Close();
        }

        private void Button_ReloadCOMS_Click(object sender, RoutedEventArgs e)
        {
            ReloadCOMList();
        }

        private void ReloadCOMList()
        {
            string[] ports = SerialPort.GetPortNames();

            COM_List.ItemsSource = ports;
            COM_List.SelectedIndex = ports.Length - 1; // select last item, often the correct port
        }

        private void Button_Connection_Click(object sender, RoutedEventArgs e)
        {
            if (COM_List.SelectedIndex == -1) return;

            if (_port != null) _port.Close();

            try
            {
                SerialPort p = new SerialPort(COM_List.SelectedItem.ToString(), 115200);
                p.Open();

                if (p.IsOpen)
                {
                    _port = p;
                    this.Title = "RGBW - Connected to " + COM_List.SelectedItem.ToString();

                    // enable sliders
                    Slider_R.IsEnabled = true;
                    Slider_G.IsEnabled = true;
                    Slider_B.IsEnabled = true;
                    Slider_W.IsEnabled = true;
                }
                else throw new Exception("Failed to open");
            }
            catch (Exception ex) // we fucked up
            {
                MessageBox.Show("Failed to connect\n" + ex.Message, "Connection Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return; // disregard error and keep going
            }
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int r, g, b, w;
            r = (int)Slider_R.Value;
            g = (int)Slider_G.Value;
            b = (int)Slider_B.Value;
            w = (int)Slider_W.Value;

            Label_R.Content = "Red " + r.ToString();
            Label_G.Content = "Green " + g.ToString();
            Label_B.Content = "Blue " + b.ToString();
            Label_W.Content = "White " + w.ToString();

            if (_port == null || !_port.IsOpen) return;

            byte[] buffer = new byte[4];
            buffer[0] = (byte)r;
            buffer[1] = (byte)g;
            buffer[2] = (byte)b;
            buffer[3] = (byte)w;

            _port.Write(buffer, 0, 4);
        }

        private void Button_Off_Click(object sender, RoutedEventArgs e)
        {
            Label_R.Content = "Red 0";
            Label_G.Content = "Green 0";
            Label_B.Content = "Blue 0";
            Label_W.Content = "White 0";

            Slider_R.Value = 0;
            Slider_G.Value = 0;
            Slider_B.Value = 0;
            Slider_W.Value = 0;

            if (_port == null || !_port.IsOpen) return;

            byte[] buffer = new byte[4];
            buffer[0] = 0;
            buffer[1] = 0;
            buffer[2] = 0;
            buffer[3] = 0;

            _port.Write(buffer, 0, 4);
        }

    }
}
