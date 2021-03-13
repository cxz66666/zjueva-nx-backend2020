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
using Newtonsoft.Json;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace _2020_backend.Pages.Records
{
    public class ExportModel : PageModel
    {
      

        private readonly BackendContext _context;
        public ExportModel(BackendContext context)
        {
            _context = context;
        }
        [BindProperty]
        public bool PasserOnly { get; set; }
        [BindProperty]
        public bool Lastest { get; set; }
        [BindProperty]
        public bool IsJson { get; set; }
        public void OnGet()
        {
            
            Lastest = true;
        }
        public IActionResult OnPost()
        {
            if (!IsJson)
                return ExportExcel();
            else
                return ExportJson();
        }
        public IActionResult ExportExcel()
        {
            string webPath = $"{Request.Scheme}://{Request.Host}{Request.Path}";
            var fileName = $"eva-nx-{DateTime.Now.ToString("MMddHHmmss")}.xlsx";
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
            var headerRowData = new List<string> { "编号", "学号", "姓名", "性别", "年级", "专业", "电子邮件", "电话", "一志愿", "二志愿", "三志愿", "接受调剂", "面试场次", "面试时间", "添加时间", "IP", "一志愿理由", "二志愿理由", "三志愿理由", "Q1", "Q2" };
            fillRow(ref row, headerRowData, boldFontStyle);
            //sheet Content
            var query = _context.Record.AsNoTracking();
            if (Lastest)
            {
                query = query.Where(t1 => !_context.Record.Any(t2 => t2.id_student == t1.id_student && t2.addedDate > t1.addedDate));
            }


            if (PasserOnly)
            {
                query = query.Where(rec => rec.status == Status.Pass);
            }
            query = query.OrderByDescending(rec => rec.addedDate).Select(rec => rec);
            int rowIndex = 1;
            foreach (var rec in query)
            {
                row = sheet.CreateRow(rowIndex);
                var rowData = new List<string> { rec.rid.ToString(), rec.id_student, rec.name, rec.sex ? "女" : "男", gradeToString(rec.grade), rec.major, rec.email, rec.phone, wishToString(rec.firstWish), wishToString(rec.secondWish), wishToString(rec.thirdWish), rec.adjustment ? "是" : "否", rec.InterviewID.ToString(), rec.InterviewTime, rec.addedDate.ToString("MM-dd HH:mm:ss"), rec.ip, rec.firstReason, rec.secondReason, rec.thirdReason, rec.question1, rec.question2 };
                if (rowIndex % 2 == 0)
                    fillRow(ref row, rowData, greyForegroundStyle);
                else
                    fillRow(ref row, rowData, null);

                rowIndex++;
            }

            // Set Column Width
            sheet.SetColumnWidth(0, 5 * 256);
            sheet.SetColumnWidth(1, 11 * 256);
            sheet.SetColumnWidth(2, 11 * 256);
            sheet.SetColumnWidth(3, 5 * 256);
            sheet.SetColumnWidth(4, 5 * 256);
            sheet.SetColumnWidth(5, 18 * 256);
            sheet.SetColumnWidth(6, 15 * 256);
            sheet.SetColumnWidth(7, 15 * 256);
            sheet.SetColumnWidth(11, 15 * 256);
            sheet.SetColumnWidth(12, 11 * 256);
            sheet.SetColumnWidth(13, 11 * 256);
            sheet.SetColumnWidth(14, 11 * 256);
            sheet.SetColumnWidth(15, 11 * 256);
            sheet.SetColumnWidth(16, 11 * 256);
            sheet.SetColumnWidth(17, 11 * 256);
            sheet.SetColumnWidth(18, 11 * 256);
            sheet.SetColumnWidth(19, 11 * 256);
            sheet.SetColumnWidth(20, 11 * 256);
            workbook.Write(memory, true);

            memory.Position = 0;

            return File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
        public IActionResult ExportJson()
        {
            var fileName = $"eva-nxjson-{DateTime.Now.ToString("MMddHHmmss")}.json";
            var query = _context.Record.AsNoTracking();
            if (Lastest)
            {
                query = query.Where(t1 => !_context.Record.Any(t2 => t2.id_student == t1.id_student && t2.addedDate > t1.addedDate));
            }


            if (PasserOnly)
            {
                query = query.Where(rec => rec.status == Status.Pass);
            }
            query = query.OrderByDescending(rec => rec.addedDate).Select(rec => rec);
            List<Record> records = query.ToList();
            var jsonbytes= System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(records);

            return File(jsonbytes, contentType: "applcation/json", fileDownloadName: fileName);
        }
        private void fillRow(ref IRow row, in IEnumerable<string> value, in ICellStyle style, int startAtIndex = 0)
        {
            foreach (var v in value)
                row.CreateCell(startAtIndex++).SetCellValue(v);

            if (style != null)
                foreach (var cell in row.Cells)
                    cell.CellStyle = style;
        }
        private string gradeToString(int grade)
        {
            switch (grade)
            {
                case 1:
                    return "大一";
                case 2:
                    return "大二";
                case 3:
                    return "大三";
                case 4:
                    return "大四";
                default:
                    return string.Empty;
            }
        }
        private string wishToString(int wish)
        {
            switch (wish)
            {
                case 1:
                    return "电器部";
                case 2:
                    return "电脑部";
                case 3:
                    return "文宣部";
                case 4:
                    return "人资部";
                case 5:
                    return "财外部";
                default:
                    return "暂无";
            }
        }
    }
}