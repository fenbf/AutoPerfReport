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
        class Results
        {
            public Results(List<PerfTestBase> perfTests)
            {
                Map = new Dictionary<string,Dictionary<int,double>>();

                foreach (var pt in perfTests)
                    Map[pt.Name] = new Dictionary<int,double>();
            }

            public Dictionary<string, Dictionary<int, double>> Map;
        }

        static void Main(string[] args)
        {
            List<PerfTestBase> perfTests = new List<PerfTestBase> { 
                new BubbleSortPerfTest(), 
                new MergeSortPerfTest(), 
                new QuickSortPerfTest() 
            };

            var res = RunAllTests(perfTests, 10, 100, 10);
            printResults(res);
        }

        private static void printResults(Results res)
        {
            System.Console.Write("N;");
            foreach (var n in res.Map.Keys)
                System.Console.Write("{0};", n);
            System.Console.WriteLine();

            foreach (int cnt in res.Map[res.Map.First().Key].Keys)
            {
                System.Console.Write("{0};", cnt);
                foreach (var n in res.Map.Keys)
                    System.Console.Write("{0:0.00};", res.Map[n][cnt]);
                System.Console.WriteLine();
            }
        }

        private static Results RunAllTests(List<PerfTestBase> perfTests, int startCount, int endCount, int stepCount)
        {
            Results res = new Results(perfTests);

            for (int n = startCount; n <= endCount; n += stepCount)
            {
                foreach (var pt in perfTests)
                {
                    pt.run(n);
                    res.Map[pt.Name][n] = pt.ElapsedTimeSec;
                }
            }

            return res;
        }

        /*
         *               //Creates workbook
            Workbook workbook = new Workbook();
            //Gets first worksheet
            Worksheet sheet = workbook.Worksheets[0];

            //Writes hello world to A1
            sheet.Range["A1"].Text = "Hello,World!";

            //Save workbook to disk
            workbook.SaveToFile("Sample.xls");           */
    }
}
