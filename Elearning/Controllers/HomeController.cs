using System.Diagnostics;
using Elearning.Models;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;

namespace Elearning.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var courses = new List<Course> {
            new Course { Course_Id = 1, Title = "Python", Description = "Python is a versatile and widely-used programming language known for its ease of learning and efficiency. It's great for web development, data analysis, artificial intelligence, and more." },
            new Course { Course_Id = 2, Title = "Java", Description = "Java is a robust, object-oriented programming language used for building enterprise-scale applications. Learn to develop portable, high-performance applications for a wide range of computing platforms." },
            new Course{ Course_Id = 3, Title = "C#", Description = "C# is a modern, object-oriented, and type-safe programming language developed by Microsoft. It is used for developing desktop applications, web services, and enterprise software on the .NET platform." },
            new Course{ Course_Id = 4, Title = "C++", Description = "C++ is a powerful programming language that combines the high performance of C with object-oriented programming features. It's used in game development, systems/software development, and in performance-critical applications." }
            };

            return View(courses);
        }
         
        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult MyLearnings()
        {
            return View();
        }

        public IActionResult Instructor()
        {
            Instructor data = new();
            data.GetInstructors();
            ViewBag.sqldata = data;
            return View();

        }
        [HttpPost]
        public IActionResult AddInstructor(Instructor instructor)
        {
            /* Make sure that inputted value isn't null or empty */
            if (String.IsNullOrEmpty(instructor.Name))
            {
                ViewBag.Message = "Error: Don't submit an empty value.";
                return View();
            }
            else
            {
                Instructor getFormData = new Instructor();
                getFormData.Name = instructor.Name;

                getFormData.AddInstructor(getFormData);
                ViewBag.Message = "Success: Value will be inserted into database";
                return View("Instructor");
            }
        }

        [HttpPost]
        public ActionResult Create(Instructor instructor)
        {
            // string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString;
            string connString = "USER ID=E_LEARNING;PASSWORD=E_LEARNING;DATA SOURCE=localhost:1522/orcl;";
            using (OracleConnection conn = new OracleConnection(connString))
            {
                string sql = "INSERT INTO Instructors (Name) VALUES (:Name)";
                OracleCommand cmd = new OracleCommand(sql, conn);
                cmd.Parameters.Add(new OracleParameter("Name", instructor.Name));

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    ViewBag.Message = "Instructor added successfully";
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "Error: " + ex.Message;
                }
            }

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}