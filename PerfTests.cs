using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoPerfReport
{
    class PerfResults
    {
        public PerfResults(List<PerfTestBase> perfTests)
        {
            Map = new Dictionary<string, Dictionary<int, double>>();

            foreach (var pt in perfTests)
                Map[pt.Name] = new Dictionary<int, double>();
        }

        public Dictionary<string, Dictionary<int, double>> Map;
    }

    abstract class PerfTestBase
    {
        public double ElapsedTimeSec {get;protected set;}
        public string Name {get;protected set;}

        public abstract void run(int n);
    }

    class BubbleSortPerfTest : PerfTestBase
    {
        public BubbleSortPerfTest()
        {
            Name = "Bubble Sort";
        }

        public override void run(int n)
        {
            ElapsedTimeSec = 0.2 * n * n;
        }
    }

    class MergeSortPerfTest : PerfTestBase
    {
        public MergeSortPerfTest()
        {
            Name = "Merge Sort";
        }

        public override void run(int n)
        {
            ElapsedTimeSec = 6.1 * n * Math.Log(n);
        }
    }

    class QuickSortPerfTest : PerfTestBase
    {
        public QuickSortPerfTest()
        {
            Name = "Quick Sort";
        }

        public override void run(int n)
        {
            ElapsedTimeSec = 4.2 * n * Math.Log(n);
        }
    }
}
