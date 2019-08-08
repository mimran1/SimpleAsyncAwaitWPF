using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SimpleAsyncAwaitWPF
{
    /// <summary>
    /// Interaction logic for Window2.xaml
    /// </summary>
    public partial class Window2 : Window
    {
        public Window2()
        {
            InitializeComponent();
           
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            PerformTask1();
            PerformTaskAsync1();
            PerformTaskAsync2();
            PerformTask2();
        }


        /// <summary>
        /// Difference between PerformTask1 and PerformTask2 is that 2 returns a value from the task it performs.
        /// Only pay attention as to how the async versions of these functions work. Uncomment the perfom function in the Button_Click_1 function one by one to see how the UI behaves when you have a large task to perform.
        /// </summary>
        public void PerformTask1()
        {
            DowloadHtml("https://www.microsoft.com/en-us/");
            //Following two lines of code is executed when line above finishes
            //Question: What does the complier do in the mean time? It blocks EVERYTHING till the above piece of code finishes
            textblockEdit.Text = "Sync";
            MessageBox.Show("Page1 done downloading data sync.");
        }

        /// <summary>
        /// Difference between PerformTaskAsync1 and PerformTaskAsync2 is that 2 shows how to retrieve a return value from a task.
        /// </summary>
        public async Task PerformTaskAsync1()
        {
            await DownloadHtmlAsync("https://www.microsoft.com/en-us/");
            //Following two lines of code is executed only when line above finishes.
            //Question: What does the complier do in the mean time? It returns control back to the UI so your dumb ass can do other stuff.
            //Compare with the PerformTask function.
            /* A little bit more explanation as to what is happening:
             * When DownloadHtmlAsync is invoked, complier goes to DownloadHtmlAsync 
             * In DownloadHtmlAsync it encounters this line: var html = await webClient.DownloadStringTaskAsync(url);
             * Say this piece of line takes 5 seconds to respond. The complier does NOT execute the next line.
             * Since we are awaiting the compiler sees that this is a async function so it checks who called it, which happens to be PerformTaskAsync1.
             * The control now returns to the line aboce i.e.: await DownloadHtmlAsync("https://www.microsoft.com/en-us/");
             * It sees the await word again and checks to see if its a async function which it is, so it calls its calling function, which happens to be Button_Click_1.
             * At this point the control is given back to the UI. It is as if you clicked the button, the clicked event performs its task and now the UI is ready for any thing else.
             * Now all this takes less than a second to happen. But remember our var html = await webClient.DownloadStringTaskAsync(url); lines takes 5 seconds.
             * When this task finishes the everthing write under it executes which is:
             * using (var streamWriter = new StreamWriter(@"C:\Users\imranm\source\repos\SimpleAsyncAwaitWPF\SimpleAsyncAwaitWPF\output.html"))
                {
                    await streamWriter.WriteAsync(html);
                }
                Again an await statement is encountered and things proceed in similar fashion.
             */
            textblockEdit.Text = "Async";
            MessageBox.Show("Page1 done downloading data async.");
        }

        public async Task DownloadHtmlAsync(String url)
        {
            for (int i = 0; i < 5; i++)
            {
                var webClient = new WebClient();
                var html = await webClient.DownloadStringTaskAsync(url);
                using (var streamWriter = new StreamWriter(@"C:\Users\imranm\source\repos\SimpleAsyncAwaitWPF\SimpleAsyncAwaitWPF\output.html"))
                {
                    await streamWriter.WriteAsync(html);
                }
            }
        }

        /// <summary>
        /// Difference between PerformTask1 and PerformTask2 is that 2 returns a value from the task it performs.
        /// Only pay attention as to how the async versions of these functions work. Uncomment the perfom function in the Button_Click_1 function one by one to see how the UI behaves when you have a large task to perform.
        /// </summary>
        public void PerformTask2()
        {
            var task = GetHtml("https://www.microsoft.com/en-us/");
            //Following two lines of code is executed only when line above finishes.
            //Question: What does the complier do in the mean time? It returns control back to the UI so your dumb ass can do other stuff.
            //Compare with the PerformTask function.
            textblockEdit.Text = "Got HTML sync";
            MessageBox.Show("Page1 done getting data sync.");
        }


        /// <summary>
        /// Difference between PerformTaskAsync1 and PerformTaskAsync2 is that 2 shows how to retrieve a return value from a task.
        /// </summary>
        public async Task PerformTaskAsync2()
        {
            var task = GetHtmlAsync("https://www.microsoft.com/en-us/");
            //Following line is executed IMMIDIATELY 
            MessageBox.Show("Page 1 showing some message while awaiting result from web.");
            //Following two lines of code is executed only when line above finishes.
            //Question: What does the complier do in the mean time? It returns control back to the UI so your dumb ass can do other stuff.
            //Compare with the PerformTask function.
            await task;
            textblockEdit.Text = "Got HTML Async";
            MessageBox.Show("Page1 done getting data async.");
        }

        public String GetHtml(String url)
        {
            String urlRepeated = "Empty";
            for (int i = 0; i < 5; i++)
            {
                var webClient = new WebClient();
                urlRepeated = webClient.DownloadString(url);
            }
            return urlRepeated;
        }

        public async Task<string> GetHtmlAsync(String url)
        {
            String urlRepeated = "Empty";
            for (int i = 0; i < 5; i++)
            {
                var webClient = new WebClient();
                urlRepeated = await webClient.DownloadStringTaskAsync(url);
            }
            return urlRepeated;
        }



        public void DowloadHtml(String url)
        {
            for (int i = 0; i < 5; i++)
            {
                var webClient = new WebClient();
                var html = webClient.DownloadData(url);
                using (var streamWriter = new StreamWriter(@"C:\Users\imranm\source\repos\SimpleAsyncAwaitWPF\SimpleAsyncAwaitWPF\output.html"))
                {
                    streamWriter.Write(html);
                }
            }
        }


    }
}
