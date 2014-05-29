using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Spire.Xls;

namespace AutoPerfReport
{
    class Program
    {
        static void Main(string[] args)
        {
            List<PerfTestBase> perfTests = new List<PerfTestBase> { 
                new BubbleSortPerfTest(), 
                new MergeSortPerfTest(), 
                new QuickSortPerfTest() 
            };

            RunAllTests(perfTests, 10, 100, 10);
        }

        private static void RunAllTests(List<PerfTestBase> perfTests, int startCount, int endCount, int stepCount)
        {
            System.Console.Write("N;");
            foreach (var pt in perfTests)
            {
                System.Console.Write("{0};", pt.Name);
            }
            System.Console.WriteLine();

            for (int n = startCount; n <= endCount; n += stepCount)
            {
                System.Console.Write("{0};", n);
                foreach (var pt in perfTests)
                {
                    pt.run(n);
                    System.Console.Write("{0:0.0};", pt.ElapsedTimeSec);
                }
                System.Console.WriteLine();
            }
        }

        /*
         *             //Creates workbook
            Workbook workbook = new Workbook();
            //Gets first worksheet
            Worksheet sheet = workbook.Worksheets[0];

            //Writes hello world to A1
            sheet.Range["A1"].Text = "Hello,World!";

            //Save workbook to disk
            workbook.SaveToFile("Sample.xls"); */
    }
}
