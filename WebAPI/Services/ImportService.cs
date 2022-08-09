using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using WebAPI.Contexts;
using WebAPI.Misc;
using WebAPI.Models.API.Requests;
using WebAPI.Models.Database;
using WebAPI.Models.Database.Enums;
using WebAPI.Models.Excel;

namespace WebAPI.Services
{
    
    public interface IImportService
    {
        Task<List<Question>> ImportQuestions(Stream stream, ECategory category);
        Task<Text> ImportText(Stream stream);
        Task<Course> ImportMemoryCourse(Stream stream, CourseFilepond filepond);
        Task<string> ImportImage(IFormFile file);
    }
    
    public class ImportService : IImportService
    {
        
        public const char ArraySeparator = ',';
        
        private readonly DatabaseContext _context;
        private readonly IDataService _dataService;
        private readonly IConfiguration _configuration;

        public ImportService(DatabaseContext context, IDataService dataService, IConfiguration configuration)
        {
            _context = context;
            _dataService = dataService;
            _configuration = configuration;
        }

        public async Task<List<Question>> ImportQuestions(Stream stream, ECategory category)
        {
            List<QuestionRow> rows = ReadExcel<QuestionRow>(stream).ToList();

            List<Question> dbQuestions = rows.Select(r => new Question(r, category)).ToList();

            await _context.AddRangeAsync(dbQuestions);
            await _context.SaveChangesAsync();
            
            return dbQuestions;
        }

        public async Task<Text> ImportText(Stream stream)
        {
            List<TextRow> rows = ReadExcel<TextRow>(stream).ToList();

            TextRow header = rows.First(r => !string.IsNullOrEmpty(r.textTitle));
            IEnumerable<TextRow> questions = rows.Where(r => string.IsNullOrEmpty(r.textTitle));
            
            Text text = new Text(header, questions);

            await _context.Texts.AddAsync(text);
            await _context.SaveChangesAsync();
            
            return text;
        }

        public async Task<Course> ImportMemoryCourse(Stream stream, CourseFilepond filepond)
        {
            List<MemoryCourseRow> rows = ReadExcel<MemoryCourseRow>(stream).ToList();

            MemoryCourseRow header = rows.First(r => !string.IsNullOrEmpty(r.name));
            IEnumerable<MemoryCourseRow> questions = rows.Where(r => string.IsNullOrEmpty(r.name));

            Course course = new Course(filepond.Type, header, questions);
            course.IsActive = filepond.IsActive;

            await _context.Courses.AddAsync(course);
            await _context.SaveChangesAsync();

            return course;
        }

        public async Task<string> ImportImage(IFormFile file)
        {
            FileInfo fileInfo = new FileInfo(file.FileName);

            await using var fileStream = System.IO.File.Create($"{_configuration["ImageRoot"]}/{file.FileName}");
            await file.CopyToAsync(fileStream);

            return file.FileName;
        }

        private static IEnumerable<T> ReadExcel<T>(Stream stream) where T : ExcelRow, new()
        {

            using var reader = ExcelReaderFactory.CreateReader(stream);
            reader.Read();
            var wrapper = new ExcelWrapper<T>(reader);
            while (reader.Read())
            {
                var q = wrapper.Read(reader);
                if (q.IsValid()) yield return q;
            }
        }
        
    }

    public class Request
    {
    }
}