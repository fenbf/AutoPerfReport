using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using Spire.Xls;
using Spire.Xls.Charts;

namespace AutoPerfReport
{
    class Program
    {
        static void Main(string[] args)
        {
            List<PerfTestBase> perfTests = new List<PerfTestBase> 
            { 
                new BubbleSortPerfTest(), 
                new MergeSortPerfTest(), 
                new QuickSortPerfTest() 
            };

            var res = runAllTests(perfTests, 10, 200, 10);
            printResults(res);
            saveResults(res, "res.xlsx");
        }

        private static PerfResults runAllTests(List<PerfTestBase> perfTests, int startCount, int endCount, int stepCount)
        {
            PerfResults res = new PerfResults(perfTests);

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

        private static void printResults(PerfResults res)
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

        private static void saveResults(PerfResults res, string filename)
        {           
            try
            {
                Workbook workbook = new Workbook();
                workbook.CreateEmptySheets(2);

                string range = "";
                string labelRange = "";
                writeData(res, workbook, ref range, ref labelRange);
                createChart(workbook, range, labelRange);
                System.Console.WriteLine("Workbook with data created!");

                workbook.SaveToFile(filename, ExcelVersion.Version2007);
                System.Console.WriteLine("Workbook written to {0}", filename);
            }            
            catch (System.Exception e)
            {
                System.Console.WriteLine("Cannot write results to {0}!", filename);
                System.Console.WriteLine("Reason: {0}", e.Message);
            }
        }

        private static void writeData(PerfResults res, Workbook workbook, ref string range, ref string labelRange)
        {
            Worksheet sheet = workbook.Worksheets[0];
            sheet.Name = "Perf Test";

            sheet.Range["A1"].Text = "Elapsed Time for sorting...";
            sheet.Range["A1"].Style.Font.IsBold = true;

            // columns title:
            sheet.Range["C3"].Text = "N";
            sheet.Range["C3"].Style.Font.IsBold = true;
            sheet.Range["C3"].Style.HorizontalAlignment = HorizontalAlignType.Center;
            char col = 'D';
            foreach (var n in res.Map.Keys)
            {
                sheet.Range[col + "3"].Text = n;
                sheet.Range[col + "3"].Style.Font.IsBold = true;
                sheet.Range[col + "3"].Style.HorizontalAlignment = HorizontalAlignType.Center;
                col++;
            }

            int rowID = 4;
            foreach (int cnt in res.Map[res.Map.First().Key].Keys)
            {
                col = 'C';
                sheet.Range[col + rowID.ToString()].NumberValue = cnt;
                sheet.Range[col + rowID.ToString()].NumberFormat = "0";
                col++;
                foreach (var n in res.Map.Keys)
                {
                    sheet.Range[col + rowID.ToString()].NumberValue = res.Map[n][cnt];
                    sheet.Range[col + rowID.ToString()].NumberFormat = "0.00";
                    col++;
                }
                rowID++;
            }

            char lastCol = 'C';
            lastCol += (char)res.Map.Keys.Count;
            range = "D3:" + lastCol + (rowID - 1).ToString();

            labelRange = "C4:C" + (rowID - 1).ToString();
        }

        private static void createChart(Workbook workbook, string range, string labelRange)
        {
            Worksheet sheet = workbook.Worksheets[1];
            sheet.Name = "Charts";
            Chart chart = sheet.Charts.Add();
            sheet.GridLinesVisible = false;

            //Set region of chart data
            chart.DataRange = workbook.Worksheets[0].Range[range];
            chart.SeriesDataFromRange = false;

            //Set position of chart
            chart.LeftColumn = 2;
            chart.TopRow = 2;
            chart.RightColumn = 12;
            chart.BottomRow = 30;


            //Chart title
            chart.ChartTitle = "Sorting Time...";
            chart.ChartTitleArea.IsBold = true;
            chart.ChartTitleArea.Size = 12;

            chart.PrimaryCategoryAxis.Title = "Element Count";
            chart.PrimaryCategoryAxis.Font.IsBold = true;
            chart.PrimaryCategoryAxis.TitleArea.IsBold = true;

            chart.PrimaryValueAxis.Title = "Elapsed time (sec)";
            chart.PrimaryValueAxis.HasMajorGridLines = false;
            chart.PrimaryValueAxis.TitleArea.IsBold = true;
            chart.PrimaryValueAxis.TitleArea.TextRotationAngle = 90;
            chart.ChartArea.BackGroundColor = System.Drawing.Color.White;
            chart.ChartArea.Interior.BackgroundColor = System.Drawing.Color.White;

            foreach (var cs in chart.Series)
            {
                cs.CategoryLabels = workbook.Worksheets[0].Range[labelRange];
            }

            chart.Legend.Position = LegendPositionType.Bottom;
            chart.ChartType = ExcelChartType.ScatterSmoothedLineMarkers;
        }
    }
}
