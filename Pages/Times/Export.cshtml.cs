using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using _2020_backend.Data;
using _2020_backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using TencentCloud.Clb.V20180317.Models;

namespace _2020_backend.Pages.Times
{
    public class ExportModel : PageModel
    {
        private readonly BackendContext _context;
        public ExportModel(BackendContext context)
        {
            _context = context;
        }
   
        public void OnGet()
        {

        }
        public async Task<IActionResult> OnPostAsync()
        {

            string webPath = $"{Request.Scheme}://{Request.Host}{Request.Path}";
            var fileName = $"nx-time-{DateTime.Now.ToString("MMddHHmmss")}.xlsx";
            var memory = new MemoryStream();

            var workbook = new XSSFWorkbook();

            // Style
            var boldFont = workbook.CreateFont();
            boldFont.FontHeightInPoints = 11;
            boldFont.FontName = "等线";
            boldFont.IsBold = true;
            var boldFontStyle = workbook.CreateCellStyle();
            boldFontStyle.SetFont(boldFont);
            var greyForegroundStyle = workbook.CreateCellStyle();
            greyForegroundStyle.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index;
            greyForegroundStyle.FillPattern = FillPattern.SolidForeground;
            //sheet begin
            var sheet = workbook.CreateSheet("导出");

            // Sheet Header
            var row = sheet.CreateRow(0);

            var headerRowData = new List<string> { "姓名","学号","年级","性别","是否排过" };

            List<InterviewTime> interviewTimes = await _context.Time.OrderBy(x=>x.Day).ThenBy(x=>x.BeginTime).ThenBy(x=>x.Place).ToListAsync();
            foreach(var x in interviewTimes)
            {
                //ID is used for check and find whether he/she pick this , but in excel each ID is covered
                headerRowData.Add($"{x.Day} {x.BeginTime} {x.Place} {x.ID}");
            }
            fillRow(ref row, headerRowData, boldFontStyle);
            //sheet Content
            var query = _context.Record.AsNoTracking();
          
            query = query.Where(t1 => !_context.Record.Any(t2 => t2.id_student == t1.id_student && t2.addedDate > t1.addedDate)).OrderByDescending(rec => rec.addedDate).Select(rec => rec);
            int rowIndex = 1;
            foreach (var rec in query)
            {
                row = sheet.CreateRow(rowIndex);
                var rowData = new List<string> { rec.name, rec.id_student,Utils.DateHelp.GetGrade(rec.grade),rec.sex?"女":"男",rec.InterviewID>0?"1":"0" };
              for(int i = 5; i < headerRowData.Count(); i++)
                {
                    int id = int.Parse(headerRowData[i].Substring( headerRowData[i].LastIndexOf(" ")+1));
                    if (rec.Times.Exists(r => r == id))
                    {
                        rowData.Add("1");
                    }
                    else
                    {
                        rowData.Add("0");
                    }

                }
                    fillRow(ref row, rowData, greyForegroundStyle);
              

                rowIndex++;
            }

            // Set Column Width
            sheet.SetColumnWidth(0, 7 * 256);
            sheet.SetColumnWidth(1, 12 * 256);
            sheet.SetColumnWidth(2, 5 * 256);
            sheet.SetColumnWidth(3, 5 * 256);
            sheet.SetColumnWidth(4, 5 * 256);
            for (int i = 5; i < headerRowData.Count(); i++)
            {
                sheet.SetColumnWidth(i, 13 * 256);
            }
            workbook.Write(memory, true);

            memory.Position = 0;

            return File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
        private void fillRow(ref IRow row, in IEnumerable<string> value, in ICellStyle style, int startAtIndex = 0)
        {
            foreach (var v in value)
                row.CreateCell(startAtIndex++).SetCellValue(v);

            if (style != null)
            {
                foreach (var cell in row.Cells)
                {
                    if (cell.StringCellValue == "1")
                        cell.CellStyle = style;

                }
            }
                
                    
        }
    }
}