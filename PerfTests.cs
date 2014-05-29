using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoPerfReport
{
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
            ElapsedTimeSec = 10.2 * n * n;
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
