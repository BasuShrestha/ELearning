using System.Diagnostics;
using Elearning.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NuGet.Protocol;

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
            if (course.Courses != null)
            {
                List<Course> availableCourses = new List<Course> { };
                foreach (Course c in course.Courses) 
                {
                    if (c.Is_Deleted == 0)
                    {
                        availableCourses.Add(c);
                    }
                }
                ViewBag.CourseList = new SelectList(availableCourses, "Course_Id", "Title");
            }


            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"];
            }

            return View();
        }

        public IActionResult AssignInstructor()
        {
            InstructorAssignment instructorAssignment = new InstructorAssignment();
            instructorAssignment.GetInstructorAssignments();
            ViewBag.SQLData = instructorAssignment;
            return View("../InstructorAssignment/InstructorAssignmentView");
        }

        public IActionResult ViewCourseAssignments()
        {
            InstructorAssignment instructorAssignment = new InstructorAssignment();
            instructorAssignment.GetInstructorAssignments();
            ViewBag.SQLData = instructorAssignment;
            return View("../InstructorAssignment/InstructorAssignmentView");
        }

        public IActionResult AddInstructor()
        {
            Course course = new Course();
            course.GetCourses();
            if (course.Courses != null)
            {
                List<Course> availableCourses = new List<Course> { };
                foreach (Course c in course.Courses)
                {
                    if (c.Is_Deleted == 0)
                    {
                        availableCourses.Add(c);
                    }
                }
                ViewBag.CourseList = new SelectList(availableCourses, "Course_Id", "Title");
            }
            return View("AddInstructorDetail");
        }

        [HttpPost]
        public IActionResult AddInstructorDetail(Instructor instructor)
        {
            if (string.IsNullOrEmpty(instructor.Name) || string.IsNullOrEmpty(instructor.Email))
            {
                TempData["Message"] = "Empty field values";
                return View();
            }
            else 
            { 
                instructor.AddInstructor(instructor);
                ViewBag.Message = "Instructor details added";
                TempData["Message"] = "Instructor details inserted successfully";
                return RedirectToAction("InstructorView");
            }
        }

        public IActionResult UpdateInstructor(int id)
        {
            Instructor instructor = new();
            instructor = instructor.GetInstructorById(id);
            ViewBag.sqldata = instructor;

            Course course = new Course();
            course.GetCourses();
            if (course.Courses != null)
            {
                List<Course> availableCourses = new List<Course> { };
                foreach (Course c in course.Courses)
                {
                    if (c.Is_Deleted == 0)
                    {
                        availableCourses.Add(c);
                    }
                }
                ViewBag.CourseList = new SelectList(availableCourses, "Course_Id", "Title");
            }
            return View("UpdateInstructor", instructor);
        }

        [HttpPost]
        public IActionResult UpdateInstructor(Instructor instructor)
        {
            if (string.IsNullOrEmpty(instructor.Name) 
                || string.IsNullOrEmpty(instructor.Email)
                || string.IsNullOrEmpty(instructor.Contact))
            {
                TempData["Message"] = "Empty fields submitted";
                return RedirectToAction("InstructorView");
            }
            else
            {
                Console.WriteLine(instructor.InstructorId);
                Console.WriteLine(instructor.Name);
                Console.WriteLine(instructor.Email);
                instructor.UpdateInstructor(instructor);
                TempData["Message"] = $"Instructor: {instructor.Name} updated successfully";
                return RedirectToAction("InstructorView");
            }
        }

        public IActionResult DeleteInstructor(int id)
        {
            Instructor instructor = new();
            instructor = instructor.GetInstructorById(id);
            ViewBag.sqldata = instructor;
            return View("DeleteInstructor", instructor);
        }

        [HttpPost]
        public IActionResult DeleteInstructorDetail(int instructorId)
        {
            try
            {
                Instructor newInstructor = new Instructor();
                newInstructor.DeleteInstructor(instructorId);
                ViewBag.Message = $"Instructor deleted from the database";
                TempData["Message"] = "Instructor deleted!";
                return RedirectToAction("InstructorView");
            }
            catch (Exception exp)
            {
                TempData["Message"] = $"Error: {exp.Message}";
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
