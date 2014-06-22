// This is the main DLL file.
#include "Reporter.h"
#include <map>
#include <string>
#include <iostream>

using namespace Spire::Xls;
using namespace Spire::Xls::Charts;
using namespace System;

static std::map<std::string, std::map<int, double>> gResults;

void writeData(Workbook ^workbook, String ^%range, String ^%labelRange);
void createChart(Workbook ^workbook, String ^%range, String ^%labelRange);

namespace reporter
{
	Reporter::Reporter()
	{

	}

	Reporter::~Reporter()
	{
		gResults.clear();
	}

	void Reporter::AddResult(const char *colName, int n, double t)
	{
		gResults[colName][n] = t;
		/*String^ clistr = gcnew String(name);
		Workbook ^wb = gcnew Workbook();
		Worksheet ^sheet = wb->Worksheets[0];
		sheet->Range["A1"]->Text = clistr;
		wb->SaveToFile("Sample.xls", ExcelVersion::Version2007);*/
	}

	void Reporter::SaveToFile(const char *fname)
	{
		try
		{
			Workbook ^workbook = gcnew Workbook();
			workbook->CreateEmptySheets(2);

			String ^range = "";
			String ^labelRange = "";
			writeData(workbook, range, labelRange);
			createChart(workbook, range, labelRange);
			std::cout << "Workbook with data created!" << std::endl;

			String ^filename = gcnew String(fname);
			workbook->SaveToFile(filename, ExcelVersion::Version2007);
			std::cout << "Workbook written to " << fname << std::endl;
		}
		catch (System::Exception ^e)
		{
			std::cout << "Cannot write results to " << fname << std::endl;
			System::Console::WriteLine("Reason: {0}", e->Message);
		}
	}
}

void writeData(Workbook ^workbook, String ^%range, String ^%labelRange)
{
	Worksheet ^sheet = workbook->Worksheets[0];
	sheet->Name = "Perf Test";

	sheet->Range["A1"]->Text = "Elapsed Time for sorting...";
	sheet->Range["A1"]->Style->Font->IsBold = true;

	// columns title:
	sheet->Range["C3"]->Text = "N";
	sheet->Range["C3"]->Style->Font->IsBold = true;
	sheet->Range["C3"]->Style->HorizontalAlignment = HorizontalAlignType::Center;
	wchar_t col = 'D';
	for(auto &n : gResults)
	{
		String ^tmp = gcnew String(n.first.c_str());
		String ^rng = gcnew String(col + "3");
		sheet->Range[rng]->Text = tmp;
		sheet->Range[rng]->Style->Font->IsBold = true;
		sheet->Range[rng]->Style->HorizontalAlignment = HorizontalAlignType::Center;
		col++;
	}

	int rowID = 4;
	for (auto &cnt : gResults[gResults.begin()->first])
	{
		col = 'C';
		sheet->Range[col + rowID.ToString()]->NumberValue = cnt.first;
		sheet->Range[col + rowID.ToString()]->NumberFormat = "0";
		col++;

		for(auto &n : gResults)
		{
			sheet->Range[col + rowID.ToString()]->NumberValue = gResults[n.first][cnt.first];
			sheet->Range[col + rowID.ToString()]->NumberFormat = "0.00";
			col++;
		}
		rowID++;
	}

	wchar_t lastCol = 'C';
	lastCol += (wchar_t)gResults.size();
	range = "D3:" + lastCol + (rowID - 1).ToString();

	labelRange = "C4:C" + (rowID - 1).ToString();
}

void createChart(Workbook ^workbook, String ^%range, String ^%labelRange)
{
	Worksheet ^sheet = workbook->Worksheets[1];
	sheet->Name = "Charts";
	Chart ^chart = sheet->Charts->Add();
	sheet->GridLinesVisible = false;

	//Set region of chart data
	chart->DataRange = workbook->Worksheets[0]->Range[range];
	chart->SeriesDataFromRange = false;

	//Set position of chart
	chart->LeftColumn = 2;
	chart->TopRow = 2;
	chart->RightColumn = 12;
	chart->BottomRow = 30;


	//Chart title
	chart->ChartTitle = "Sorting Time";
	chart->ChartTitleArea->IsBold = true;
	chart->ChartTitleArea->Size = 12;

	chart->PrimaryCategoryAxis->Title = "Element Count";
	chart->PrimaryCategoryAxis->Font->IsBold = true;
	chart->PrimaryCategoryAxis->TitleArea->IsBold = true;

	chart->PrimaryValueAxis->Title = "Elapsed time (sec)";
	chart->PrimaryValueAxis->HasMajorGridLines = false;
	chart->PrimaryValueAxis->TitleArea->IsBold = true;
	chart->PrimaryValueAxis->TitleArea->TextRotationAngle = 90;	

	for each(ChartSerie ^cs in chart->Series)
	{
		cs->CategoryLabels = workbook->Worksheets[0]->Range[labelRange];
	}

	chart->Legend->Position = LegendPositionType::Bottom;
	chart->ChartType = ExcelChartType::ScatterSmoothedLineMarkers;
}