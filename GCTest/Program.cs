using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Timers;
using System.Threading;

namespace GCTest
{

    class Program
    {
        private const int LOAD_SIZE = 100 * 1024 * 1024;
        private const int RETRYS = 1000;
        /// <summary>
        /// ------------------------------------------
        /// Concurrent | Server| Duration(ms)
        /// ------------------------------------------
        /// false      | false |  806
        /// false      | true  | 855
        /// true       | false | 9558
        /// true       | true  | 9449
        /// ------------------------------------------

        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Stopwatch watch = new Stopwatch();
            // Wait a second in order to calm down startup
            Thread.Sleep(1000); 
            watch.Start();
            CreateLoad();
            watch.Stop();
            Console.WriteLine("Duration: {0} ms", watch.ElapsedMilliseconds);
            Console.ReadKey();
        }

        private static void CreateLoad()
        {
            for (int i = 0; i < RETRYS; i++)
            {
                byte[] data = new byte[LOAD_SIZE];
            }
        }
    }
}
