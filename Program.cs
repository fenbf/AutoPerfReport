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

            var res = runAllTests(perfTests, 10, 100, 10);
            printResults(res);
            saveResults(res, "res.xls");
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

        private static void saveResults(Results res, string filename)
        {
            Workbook workbook = new Workbook();
            Worksheet sheet = workbook.Worksheets[0];
            sheet.Name = "Perf Test";

            sheet.Range["A1"].Text = "Elapsed Time for sorting...";
            sheet.Range["A1"].Style.Font.IsBold = true;

            // columns title:
            sheet.Range["C3"].Text = "N";
            sheet.Range["C3"].Style.Font.IsBold = true;
            char col = 'D';
            foreach (var n in res.Map.Keys)
            {
                sheet.Range[col + "3"].Text = n;
                sheet.Range[col + "3"].Style.Font.IsBold = true;
                col++;
            }

            int rowID = 4;
            foreach (int cnt in res.Map[res.Map.First().Key].Keys)
            {
                col = 'C';
                sheet.Range[col + rowID.ToString()].NumberValue = cnt;
                sheet.Range[col + rowID.ToString()].NumberFormat = "0.00";
                col++;
                foreach (var n in res.Map.Keys)
                {
                    sheet.Range[col + rowID.ToString()].NumberValue = res.Map[n][cnt];
                    sheet.Range[col + rowID.ToString()].NumberFormat = "0.00";
                    col++;
                }
                rowID++;
            }

            try
            {
                workbook.SaveToFile(filename);
                System.Console.WriteLine("Results written to {0}", filename);
            }            
            catch (System.Exception e)
            {
                System.Console.WriteLine("Cannot write results to {0}!", filename);
                System.Console.WriteLine("Reason: {0}", e.Message);
            }
        }

        private static Results runAllTests(List<PerfTestBase> perfTests, int startCount, int endCount, int stepCount)
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

    }
}
