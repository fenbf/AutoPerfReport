#pragma once

#include <string>
#include <math.h>
#include <map>
#include <memory>


class PerfTestBase
{
protected:
	double _elapsedTimeSec{ 0.0 };
	std::string _name;

public:	
	virtual ~PerfTestBase() { }
	virtual void run(int n) = 0;

	double elapsedTimeSec() const { return _elapsedTimeSec; }
	std::string name() const { return _name; }
};

typedef std::vector<std::shared_ptr<PerfTestBase>> PerfTestList;

class PerfResults
{
public:
	std::map<std::string, std::map<int, double>> Map;
};


class BubbleSortPerfTest : public PerfTestBase
{
public:
	BubbleSortPerfTest() 
	{
		_name = "Bubble Sort";
	}

	void run(int n) override
	{
		_elapsedTimeSec = 0.2 * n * n;
	}
};

class MergeSortPerfTest : public PerfTestBase
{
public:
	MergeSortPerfTest() 
	{
		_name = "Merge Sort";
	}

	void run(int n) override
	{
		_elapsedTimeSec = 6.1 * n * log(static_cast<double>(n));
	}
};

class QuickSortPerfTest : public PerfTestBase
{
public:
	QuickSortPerfTest() 
	{
		_name = "Quick Sort";
	}

	void run(int n) override
	{
		_elapsedTimeSec = 4.2 * n * log(static_cast<double>(n));
	}
};