using System.Diagnostics;
using System.Globalization;
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

        public IActionResult Index(string date)
        {
            EnrolledCourse enrolledCourse = new EnrolledCourse();
            int filterMonth;
            int filterYear;

            if (!string.IsNullOrEmpty(date))
            {
                // Parse the date string to get the month and year
                if (DateTime.TryParseExact(date, "yyyy-MM", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
                {
                    filterMonth = parsedDate.Month;
                    filterYear = parsedDate.Year;
                }
                else
                {
                    // Handle invalid date input, maybe set to current month and year
                    filterMonth = DateTime.Now.Month;
                    filterYear = DateTime.Now.Year;
                }
            }
            else
            {
                // No date was provided, use current month and year
                filterMonth = DateTime.Now.Month;
                filterYear = DateTime.Now.Year;
            }

            // Convert month number to name
            string monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(filterMonth);
            ViewBag.FilterMonth = monthName; // Store the month name
            ViewBag.FilterYear = filterYear;

            var topCourses = enrolledCourse.GetTopThreeEnrolledCoursesWithCounts(filterMonth, filterYear);
            ViewBag.TopCourses = topCourses;

            Course course = new Course();
            course.GetCourses();
            ViewBag.AllCourses = course.Courses;

            return View();
        }



        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}