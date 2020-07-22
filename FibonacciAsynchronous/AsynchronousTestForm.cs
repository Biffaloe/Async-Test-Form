//Modify the example of fig 23.3 to process the results by using
//the array returned by the Task produced by Task method WhenAll

using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FibonacciAsynchronous
{
   public partial class AsynchronousTestForm : Form
   {
      public AsynchronousTestForm()
      {
         InitializeComponent();
      }

      // start asynchronous calls to Fibonacci
      private async void startButton_Click(object sender, EventArgs e)
      {
         outputTextBox.Text =
            "Starting Task to calculate Fibonacci(40) and Fibonacci(41)\r\n";

         // create Task to perform Fibonacci(40) and Fibonacci(41) calculation in a thread
         Task<TimeData> task1 = Task.Run(() => StartFibonacci(40));
         Task<TimeData> task2 = Task.Run(() => StartFibonacci(41));

         TimeData[] result = await Task.WhenAll(task1, task2); // wait for both to complete

         // determine time that first thread started
         DateTime startTime = 
            (result[0].StartTime < result[1].StartTime) ?
            result[0].StartTime : result[1].StartTime;

         // determine time that last thread ended
         DateTime endTime =
            (result[0].EndTime > result[1].EndTime) ?
            result[0].EndTime : result[1].EndTime;

         // display total time for calculations
         double totalMinutes = (endTime - startTime).TotalMinutes;
         outputTextBox.AppendText(
            $"Total calculation time = {totalMinutes:F6} minutes\r\n");
      }

      // starts a call to fibonacci and captures start/end times
      TimeData StartFibonacci(int n)
      {
         // create a TimeData object to store start/end times
         var result = new TimeData();

         AppendText($"Calculating Fibonacci({n})");
         result.StartTime = DateTime.Now;
         long fibonacciValue = Fibonacci(n);
         result.EndTime = DateTime.Now;

         AppendText($"Fibonacci({n}) = {fibonacciValue}");
         double minutes = 
            (result.EndTime - result.StartTime).TotalMinutes;
         AppendText($"Calculation time = {minutes:F6} minutes\r\n");

         return result;
      }

      // Recursively calculates Fibonacci numbers
      public long Fibonacci(long n)
      {
         if (n == 0 || n == 1)
         {
            return n;
         }
         else
         {
            return Fibonacci(n - 1) + Fibonacci(n - 2);
         }
      }

      // append text to outputTextBox in UI thread
      public void AppendText(String text)
      {
         if (InvokeRequired) // not GUI thread, so add to GUI thread
         {
            Invoke(new MethodInvoker(() => AppendText(text)));
         }
         else // GUI thread so append text
         {
            outputTextBox.AppendText(text + "\r\n");
         }
      }
   }
}
