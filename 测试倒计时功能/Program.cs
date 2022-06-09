using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace 测试倒计时功能
{
    class Program
    {
        private static Timer _timer;

        static void Main(string[] args)
        {
            Console.WriteLine($"倒计时开始:{DateTime.Now}...");
            _timer = new Timer(5000);  // 倒计时10秒
            _timer.Elapsed += new ElapsedEventHandler(OnTimeOutEvent);
            _timer.AutoReset = true;
            _timer.Start();
            Console.ReadKey();
        }

        private static void OnTimeOutEvent(object sender , ElapsedEventArgs e)
        {
            Console.WriteLine($"倒计时结束:{DateTime.Now}...");
        }
    }
}
