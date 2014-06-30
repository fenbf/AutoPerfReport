#pragma once


#ifdef REPORTER_EXPORTS
	#define REPORTER_API __declspec(dllexport) 
#else
	#define REPORTER_API __declspec(dllimport) 
#endif

namespace reporter
{
	class REPORTER_API Reporter
	{
	private:
		class Results *_results;

	public:
		Reporter();
		~Reporter();

		void AddResult(const char *colName, int n, double elapsedTime);
		void SaveToFile(const char *fname);
	};

} // namespace reporter