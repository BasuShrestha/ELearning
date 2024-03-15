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
            if (string.IsNullOrEmpty(instructor.Name) || string.IsNullOrEmpty(instructor.Course_Id.ToString()))
            {
                TempData["Message"] = "Empty field values";
                return View();
            }
            else 
            { 
                Instructor newInstructor = new Instructor();
                newInstructor.Name = instructor.Name;
                newInstructor.Course_Id = instructor.Course_Id;
                newInstructor.AddInstructor(newInstructor);
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
            if (string.IsNullOrEmpty(instructor.Name) || string.IsNullOrEmpty(instructor.Course_Id.ToString()))
            {
                TempData["Message"] = "Empty fields submitted";
                return RedirectToAction("InstructorView");
            }
            else
            {
                Instructor updatedInstructor = new Instructor();
                updatedInstructor.Name = instructor.Name;
                updatedInstructor.Course_Id = instructor.Course_Id;
                Console.WriteLine(instructor.Name);
                Console.WriteLine(instructor.Course_Id);
                updatedInstructor.UpdateInstructor(instructor);
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
