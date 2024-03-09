using System.Diagnostics;
using Elearning.Models;
using Elearning.Utilities;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;

namespace Elearning.Controllers
{
    public class CourseController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult CourseView()
        {
            Course course = new Course();
            course.GetCourses();
            ViewBag.sqldata = course;

            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"];
            }
            return View();
        }

        [HttpPost]
        public IActionResult AddCourse(Course model) 
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (String.IsNullOrEmpty(model.Title))
            {
                TempData["Message"] = "Error: Don't submit an empty value. PLEASE";
                return View(model);
            }

            AddCourseDetail(model);

            TempData["Message"] = "Success: Value will be inserted into the database";

            return RedirectToAction("CourseView");
        }

        [HttpPost]
        public IActionResult AddCourseDetail(Course course)
        {
            if (string.IsNullOrEmpty(course.Title) || string.IsNullOrEmpty(course.Description))
            {
                TempData["Message"] = "Empty fields submitted";
                return View();
            }
            else
            {
                Course newCourse = new Course();
                newCourse.Title = course.Title;
                newCourse.Description = course.Description;
                newCourse.AddCourse(newCourse);
                TempData["Message"] = "Course detail added successfully";
                return RedirectToAction("CourseView");
            }
        }

        public IActionResult UpdateCourse(int id)
        { 
            Course course = new();
            course.GetCourseById(id);

            ViewBag.sqldata = course;
            return View("UpdateCourse", course);
        }

        [HttpPost]
        public IActionResult UpdateCourse(Course course)
        {
            if (string.IsNullOrEmpty(course.Title) || string.IsNullOrEmpty(course.Description))
            {
                TempData["Message"] = "Empty fields submitted";
                return RedirectToAction("CourseView");
            }
            else
            { 
                //Course updatedCourse = new Course();
                //updatedCourse.Title = course.Title;
                //updatedCourse.Description = course.Description;
                course.UpdateCourse(course);
                TempData["Message"] = $"Course: {course.Title} updated successfully";
                return RedirectToAction("CourseView");
            }
        }
        
        public IActionResult DeleteCourse()
        { 
            return View(new Course());
        }

        [HttpPost]
        public IActionResult DeleteCourse(int id) 
        {
            try
            {
                Course newCourse = new Course();
                newCourse.DeleteCourse(id);
                TempData["Message"] = $"Course deleted from the database";
                return RedirectToAction("CourseView");
            }
            catch (Exception exp)
            {
                TempData["Message"] = $"Error: {exp.Message}";
                return RedirectToAction("CourseView");
            }

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
