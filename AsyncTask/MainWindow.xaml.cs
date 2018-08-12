using System;
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

namespace AsyncTask
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DateTime clockAtual = DateTime.MinValue;
        int delay = 500;
        bool waitEndTask = true;

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void btStart_Click(object sender, RoutedEventArgs e)
        {
            ResetUI();
            if (waitEndTask)
            {
                events.Items.Add("start btStart_Click");
                await StartClocks();
                //The Click will finish when all tasks get end                  
                events.Items.Add("end btStart_Click");
            }
            else
            {
                events.Items.Add("start btStart_Click");
                StartClocks();
                //The Click will finish immediately after the call of StartClocks() method                 
                events.Items.Add("end btStart_Click");
            }           
        }

        /// <summary>
        /// Main Task that will call others task async
        /// </summary>
        /// <returns></returns>
        private async Task StartClocks()
        {
            events.Items.Add("..running tasks");
            //Start first process that update the first clock in UI.          
            await FirstClockAsync();

            //The second process will start when the first process get end;
            await SecondClock();

            //The third process will start when the second process get end;
            await ThirdClock();

            //finally the Main taks get end;
            events.Items.Add("..ending all tasks");
        }

        private async Task ThirdClock()
        {
            events.Items.Add("....third Task started");
            DateTime fim = new DateTime(1, 1, 1, 0, 0, 30);
            for (DateTime i = clockAtual; i <= fim; i = i.AddSeconds(1))
            {
                //This Invoke is responseble to update the UI in Main Thread.
                //As it is an async task you need to wait for update in the main Thread and then go to the next step;
                await Application.Current.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background,
                  new Action(() =>
                  {
                      thirdClock.Text = i.ToLongTimeString();
                  }));
                await Task.Delay(delay);
                clockAtual = i;
            }
            events.Items.Add("....third Task finished");
        }

        private async Task SecondClock()
        {
            events.Items.Add("....second Task started");
            DateTime fim = new DateTime(1, 1, 1, 0, 0, 20);
            for (DateTime i = clockAtual; i <= fim; i = i.AddSeconds(1))
            {
                //This Invoke is responseble to update the UI in Main Thread.
                //As it is an async task you need to wait for update in the main Thread and then go to the next step;
                await Application.Current.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background,
                  new Action(() =>
                  {
                      secondClock.Text = i.ToLongTimeString();
                  }));
                clockAtual = i;
                await Task.Delay(delay);
            }
            events.Items.Add("....second Task finished");
        }

        private async Task FirstClockAsync()
        {
            events.Items.Add("....first Task started");
            DateTime fim = new DateTime(1, 1, 1, 0, 0, 10);
            for (DateTime i = clockAtual; i <= fim; i = i.AddSeconds(1))
            {
                //This Invoke is responseble to update the UI in Main Thread.
                //As it is an async task you need to wait for update in the main Thread and then go to the next step;
                await Application.Current.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background,
                   new Action(() =>
                   {
                       firstClock.Text = i.ToLongTimeString();
                   }));
                clockAtual = i;
                await Task.Delay(delay);
            }
            events.Items.Add("....first Task finished");
        }

        private void btReset_Click(object sender, RoutedEventArgs e)
        {
            ResetUI();
        }

        private void ResetUI()
        {
            //Reset the UI;
            firstClock.Text = secondClock.Text = thirdClock.Text = DateTime.MinValue.ToLongTimeString();
            clockAtual = DateTime.MinValue;
            events.Items.Clear();
        }

        private void rdAwait_Click(object sender, RoutedEventArgs e)
        {
            waitEndTask = true;
        }

        private void rdNotAwait_Click(object sender, RoutedEventArgs e)
        {
            waitEndTask = false;
        }
    }
}
