#include <iostream>
#include <vector>
#include <memory>

#include "NativeTest.h"

#include "reporter.h"

PerfResults runAllTests(PerfTestList &perfTests, int startCount, int endCount, int stepCount);
void printResults(PerfResults &res);
void saveResults(std::string filename);

reporter::Reporter report;

int main(int argc, char* argv[])
{
	PerfTestList perfTests 
	{
		std::shared_ptr<PerfTestBase>(new BubbleSortPerfTest()),
		std::shared_ptr<PerfTestBase>(new MergeSortPerfTest()),
		std::shared_ptr<PerfTestBase>(new QuickSortPerfTest())
	};

	auto res = runAllTests(perfTests, 10, 200, 10);
	printResults(res);
	saveResults("res.xlsx");

	return 0;
}

PerfResults runAllTests(PerfTestList &perfTests, int startCount, int endCount, int stepCount)
{
	PerfResults res;

	for (int n = startCount; n <= endCount; n += stepCount)
	{
		for(auto &pt : perfTests)
		{
			pt->run(n);
			res.Map[pt->name()][n] = pt->elapsedTimeSec();
			report.AddResult(pt->name().c_str(), n, pt->elapsedTimeSec());
		}
	}

	return res;
}

void printResults(PerfResults &res)
{
	std::cout << "N:";
	for (auto &n : res.Map)
		std::cout << n.first << ", ";
	std::cout << std::endl;

	for(auto &cnt : res.Map[res.Map.begin()->first])
	{
		std::cout << cnt.first << ";";
		for (auto &n : res.Map)
			std::cout << res.Map[n.first][cnt.first] << ";";
		std::cout << std::endl;
	}
}

void saveResults(std::string filename)
{
	report.SaveToFile(filename.c_str());
}
