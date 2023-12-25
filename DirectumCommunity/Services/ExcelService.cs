using DirectumCommunity.Models.ViewModels;
using NPOI.HSSF.UserModel;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace DirectumCommunity.Services;

public class ExcelService
{
    public byte[] CreateSubstitutionInMonth(List<SubstitutionInMonth> source, int year, int month)
    {
        string[] daysOfWeek = { "Вс", "Пн", "Вт", "Ср", "Чт", "Пт", "Сб" };
        int daysInMonth = DateTime.DaysInMonth(year, month);
        
        var workbook = new HSSFWorkbook();
        var sheet = (HSSFSheet)workbook.CreateSheet("Sheet1");
        var rowHeader = (HSSFRow)sheet.CreateRow(0);
        
        var font = workbook.CreateFont();
        font.IsBold = true;
        var style = workbook.CreateCellStyle();
        style.SetFont(font);
        
        var cellEmployeeName = rowHeader.CreateCell(0);
        cellEmployeeName.SetCellValue("Сотрудник");
        cellEmployeeName.CellStyle = style;

        for (int i = 1; i <= daysInMonth; i++)
        {
            DateTime date = new DateTime(year, month, i);
            DayOfWeek dayOfWeek = date.DayOfWeek;
            
            var cell = rowHeader.CreateCell(i);
            cell.SetCellValue($"{i} {daysOfWeek[(int)dayOfWeek]}");
            cell.CellStyle = style;
        }

        var rowNum = 1;
        foreach (var employee in source)
        {
            var rowContent = (HSSFRow)sheet.CreateRow(rowNum);
            
            
            var cellContent = (HSSFCell)rowContent.CreateCell(0);
            cellContent.SetCellValue(employee.Name);

            foreach (var item in employee.Substitutions)
            {
                var days = GetDaysInRangeForMonth(item.StartDate.Value, item.EndDate.Value, month);
                
                ICellStyle dayStyle = workbook.CreateCellStyle();

                IDrawing patriarch = (HSSFPatriarch)sheet.CreateDrawingPatriarch();
                IComment comment = patriarch.CreateCellComment(new HSSFClientAnchor(0, 0, 0, 0, 2, 1, 4, 4));
                font.Boldweight = (short)FontBoldWeight.Bold;
                comment.String.ApplyFont(0, comment.Author.Length, font);
                comment.String = new HSSFRichTextString($"Замещает: {item.SubstituteName},\n{item.SubstituteDepartment}\nПричина: {item.Reason}");
                
                switch (item.TypeReason)
                {
                    case 1:
                        dayStyle.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Red.Index;
                        dayStyle.FillPattern = FillPattern.SolidForeground;
                        break;
                    case 2:
                        dayStyle.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.LightGreen.Index;
                        dayStyle.FillPattern = FillPattern.SolidForeground;
                        break;
                }
                
                foreach (var day in days)
                {
                    var cellDay = (HSSFCell)rowContent.CreateCell(day);
                    cellDay.CellStyle = dayStyle;
                    cellDay.CellComment = comment;
                }
            }
            
            rowNum++;
        }

        var stream = new MemoryStream();
        workbook.Write(stream);
        var content = stream.ToArray();

        return content;
    }
    
    private IEnumerable<int> GetDaysInRangeForMonth(DateTimeOffset startDate, DateTimeOffset endDate, int targetMonth)
    {
        int startMonth = startDate.Month;
        int endMonth = endDate.Month;

        DateTimeOffset start = new DateTimeOffset(startDate.Year, targetMonth, 1, 0, 0, 0, startDate.Offset);
        DateTimeOffset end = start.AddMonths(1).AddDays(-1);

        if (startMonth > targetMonth)
        {
            start = start.AddYears(1);
        }

        if (endMonth < targetMonth)
        {
            end = end.AddYears(-1);
        }

        List<int> daysInRange = new List<int>();

        DateTimeOffset current = start;

        while (current <= end)
        {
            if (current >= startDate && current <= endDate)
            {
                daysInRange.Add(current.Day);
            }
            current = current.AddDays(1);
        }

        return daysInRange;
    }
}