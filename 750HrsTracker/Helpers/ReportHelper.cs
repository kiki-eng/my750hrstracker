using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System;
using System.Globalization;
using System.Text;
using ClosedXML.Excel;
using System.Reflection;
using System.Runtime.Loader;
using System.Diagnostics.CodeAnalysis;
using System.Formats.Asn1;
using _750HrsTracker.DTOs.Responses;
using SendGrid.Helpers.Mail;
using _750HrsTracker.Helpers.Constants;

namespace _750HrsTracker.Helpers
{
    public class ReportHelper
    {

        public static LogReportDownloadResponse ExportActivityLogReportToExcel(IEnumerable<GetActivityLogResponse> records, string exportFileName)
        {
            try
            {

                using var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add(LogCategoryConstants.ReportDownloadFileName);
                int row = 1;

                worksheet.Cell(row, 1).Value = "Activity Date";
                worksheet.Cell(row, 2).Value = "Description";
                worksheet.Cell(row, 3).Value = "Log Type";
                worksheet.Cell(row, 4).Value = "Category";
                worksheet.Cell(row, 5).Value = "Activity";
                worksheet.Cell(row, 6).Value = "Task";
                worksheet.Cell(row, 7).Value = "Hours Spent";
                worksheet.Cell(row, 8).Value = "Minutes Spent";
                worksheet.Cell(row, 9).Value = "Seconds Spent";
                worksheet.Cell(row, 10).Value = "Activity By";
                worksheet.Cell(row, 11).Value = "Property";

                foreach (var dt in records)
                {
                    var activityBy = dt.ActivityBy != null ?
                              ($"{dt.ActivityBy.FirstName} {dt.ActivityBy.LastName}")
                              : string.Empty;

                    row++;
                    worksheet.Cell(row, 1).Value = dt.ActivityDate;
                    worksheet.Cell(row, 2).Value = dt.Description;
                    worksheet.Cell(row, 3).Value = dt.LogType.ToString();
                    worksheet.Cell(row, 4).Value = dt.Category?.Name ?? "N/A";
                    worksheet.Cell(row, 5).Value = dt.Activity?.Name ?? "N/A";
                    worksheet.Cell(row, 6).Value = dt.Task?.Name ?? "N/A";
                    worksheet.Cell(row, 7).Value = dt.HoursSpent.ToString();
                    worksheet.Cell(row, 8).Value = dt.MinutesSpent.ToString();
                    worksheet.Cell(row, 9).Value = dt.SecondsSpent.ToString();
                    worksheet.Cell(row, 10).Value = activityBy;
                    worksheet.Cell(row, 11).Value = dt.Properties != null ? dt.Properties!.First().Name : "N/A";
                }

                using var stream = new MemoryStream();
                workbook.SaveAs(stream);
                var content = stream.ToArray();

                return new LogReportDownloadResponse
                {
                    FileStatus = content != null ? true : false,
                    ExportContent = content,
                    FileName = exportFileName
                };
            }catch(Exception ex)
            {
                return new LogReportDownloadResponse
                {
                    FileStatus = false,
                };
            }
        }

        internal class CustomAssemblyLoadContext : AssemblyLoadContext
        {
            public IntPtr LoadUnmanagedLibrary(string absolutePath) => LoadUnmanagedDll(absolutePath);
            protected override IntPtr LoadUnmanagedDll(string unmanagedDllName) => LoadUnmanagedDllFromPath(unmanagedDllName);
            protected override Assembly Load(AssemblyName assemblyName)
            {
                throw new NotImplementedException();
            }
        }
    }
}
