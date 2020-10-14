using AAC.Models;
using Nager.Date;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace AAC.ViewModels
{
    public class ExportViewModel
    {
        public AttendaceViewModel Attendance { get; set; }
        public ExportViewModel() { }

        #region Commands
        public ICommand MarkAllCommand
        {
            get => new Command(() =>
            {
                Attendance.RunnersGroups.ForEach(g => g.ForEach(r => r.ForExport = true));
            });
        }
        public ICommand UnMarkAllCommand
        {
            get => new Command(() =>
            {
                Attendance.RunnersGroups.ForEach(g => g.ForEach(r => r.ForExport = false));
            });
        }
        public ICommand ExportXLSXFileCommand
        {
            get => new Command(() =>
            {
                try
                {
                    var fn = "TFAC.xlsx";
                    var file = Path.Combine(FileSystem.CacheDirectory, fn);
                    var xlsx = GetRaceResultXLSX();
                    xlsx.Write(File.Create(file));
                    Share.RequestAsync(new ShareFileRequest(file: new ShareFile(file), title: fn));
                }
                catch (Exception e)
                {
                    Application.Current.MainPage.Navigation.NavigationStack[^1].DisplayAlert("Error", e.Message, "OK");
                }
            });
        }
        #endregion
        #region Functions
        readonly string[] MonthNames = new string[] { "Январь", "Февраль", "Март", "Апрель", "Март", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь" };
        readonly int StartMonth = 8;/* TODO: move to settings */
        public IWorkbook GetRaceResultXLSX()
        {
            XSSFWorkbook workbook = new XSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("sheet1");

            List<Runner> ForExport = new List<Runner>();/* TODO: fix too slow */
            Attendance.RunnersGroups.ForEach(g => g.ForEach(r => { if (r.ForExport) ForExport.Add(r); }));
            var FirstAttend = DateTime.Now;
            ForExport.ForEach(r => { if (r.FirstAttend < FirstAttend) FirstAttend = r.FirstAttend; });

            int RowId = 0;
            DateTime attend = FirstAttend;
            attend -= TimeSpan.FromDays(attend.Day - 1) + attend.TimeOfDay;
            for (; attend < DateTime.Now; attend = attend.AddMonths(1))/* every month */
            {
                /* month header */
                int ColumnId = 0;
                sheet.CreateRow(RowId).CreateCell(ColumnId++).SetCellValue(MonthNames[attend.Month]);
                for (int i = 0; i < DateTime.DaysInMonth(attend.Year, attend.Month); ++i)
                {
                    var ceil = sheet.GetRow(RowId).CreateCell(ColumnId++);
                    if (DateSystem.IsPublicHoliday(attend + TimeSpan.FromDays(i), CountryCode.RU) || 
                        DateSystem.IsWeekend(attend + TimeSpan.FromDays(i), CountryCode.RU))
                    {
                        var style = workbook.CreateCellStyle();
                        style.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.LightGreen.Index;
                        style.FillPattern = FillPattern.SolidForeground;
                        ceil.CellStyle = style;
                    }
                    ceil.SetCellValue(i + 1);
                }
                sheet.GetRow(RowId).CreateCell(ColumnId++).SetCellValue("За месяц");

                /* month body */
                RowId++;
                foreach (var runner in ForExport)
                {
                    ColumnId = 0;
                    int MonthAttendance = 0;
                    sheet.CreateRow(RowId).CreateCell(ColumnId++).SetCellValue(runner.Name);
                    for (int i = 0; i < DateTime.DaysInMonth(attend.Year, attend.Month); ++i)
                    {
                        int attendance = runner.AttendanceOn(attend + TimeSpan.FromDays(i));
                        var ceil = sheet.GetRow(RowId).CreateCell(ColumnId++);
                        if (DateSystem.IsPublicHoliday(attend + TimeSpan.FromDays(i), CountryCode.RU) ||
                            DateSystem.IsWeekend(attend + TimeSpan.FromDays(i), CountryCode.RU))
                        {
                            var style = workbook.CreateCellStyle();
                            style.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.LightGreen.Index;
                            style.FillPattern = FillPattern.SolidForeground;
                            ceil.CellStyle = style;
                        }
                        ceil.SetCellValue(attendance);
                        MonthAttendance += attendance;
                    }
                    sheet.GetRow(RowId).CreateCell(ColumnId++).SetCellValue(MonthAttendance);
                    /* TODO: add compare to previos month */
                    RowId++;
                }
                RowId++;
            }

            return workbook;
        }
        #endregion
    }
}
