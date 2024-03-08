using System.Diagnostics;
using Elearning.Models;
using Microsoft.AspNetCore.Mvc;

namespace Elearning.Controllers
{
    public class InstructorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult InstructorView()
        {
            Instructor instructor = new Instructor();
            instructor.GetInstructors();
            ViewBag.SQLData = instructor;

            Course course = new Course();
            course.GetCourses();
            ViewBag.CourseSqlData = course;
            return View();
        }

        [HttpPost]
        public IActionResult AddInstructor(Instructor instructor)
        {
            if (!ModelState.IsValid)
            {
                return View(instructor);
            }

            if (string.IsNullOrEmpty(instructor.Name))
            {
                ViewBag.Message = "Empty values in the fields";
                return View(instructor);
            }

            AddInstructorDetail(instructor);

            ViewBag.Message = "Instructor details inserted successfully";
            return RedirectToAction("InstructorView");
        }

        [HttpPost]
        public IActionResult AddInstructorDetail(Instructor instructor)
        {
            if (string.IsNullOrEmpty(instructor.Name) || string.IsNullOrEmpty(instructor.Course_Id.ToString()))
            {
                ViewBag.Message = "Empty field values";
                return View();
            }
            else 
            { 
                Instructor newInstructor = new Instructor();
                newInstructor.Name = instructor.Name;
                newInstructor.Course_Id = instructor.Course_Id;
                newInstructor.AddInstructor(newInstructor);
                ViewBag.Message = "Instructor details added";
                return RedirectToAction("InstructorView");
            }
        }

        [HttpPost]
        public IActionResult UpdateInstructor(int id)
        {
            Instructor instructor = new();
            instructor.GetInstructorById(id);

            ViewBag.sqldata = instructor;
            return View("UpdateInstructor", instructor);
        }

        [HttpPost]
        public IActionResult UpdateInstructor(Instructor instructor)
        {
            if (string.IsNullOrEmpty(instructor.Name) || string.IsNullOrEmpty(instructor.Course_Id.ToString()))
            {
                ViewBag.Message = "Empty fields submitted";
                return RedirectToAction("InstructorView");
            }
            else
            {
                Instructor updatedInstructor = new Instructor();
                updatedInstructor.Name = instructor.Name;
                updatedInstructor.Course_Id = instructor.Course_Id;
                updatedInstructor.UpdateInstructor(instructor);
                ViewBag.Message = $"Instructor: {instructor.Name} updated successfully";
                return RedirectToAction("InstructorView");
            }
        }

        public IActionResult DeleteInstructor()
        { 
            return View(new Instructor());
        }

        [HttpPost]
        public IActionResult DeleteInstructor(int id)
        {
            try
            {
                Instructor newInstructor = new Instructor();
                newInstructor.DeleteInstructor(id);
                ViewBag.Message = $"Instructor deleted from the database";
                return RedirectToAction("InstructorView");
            }
            catch (Exception exp)
            {
                ViewBag.Message = $"Error: {exp.Message}";
                return RedirectToAction("InstructorView");
            }

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
