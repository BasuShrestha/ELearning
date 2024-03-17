using Elearning.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Oracle.ManagedDataAccess.Client;

namespace Elearning.Controllers
{
    public class InstructorAssignmentController : Controller
    {
        // GET: InstructorAssignmentController
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult InstructorAssignmentView()
        {
            InstructorAssignment instructorAssignment = new InstructorAssignment();
            instructorAssignment.GetInstructorAssignments();
            ViewBag.SQLData = instructorAssignment;

            Instructor instructor = new Instructor();
            instructor.GetInstructors();
            if (instructor.Instructors != null)
            {
                List<Instructor> availableInstructors = new List<Instructor> { };
                foreach (Instructor i in instructor.Instructors)
                {
                    if (i.IsDeleted == 0)
                    {
                        availableInstructors.Add(i);
                    }
                }
                ViewBag.InstructorList = new SelectList(availableInstructors, "InstructorId", "Name");
            }

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
            if (TempData["ErrorMessage"] != null)
            {
                ViewBag.Message = TempData["ErrorMessage"];
            }

            return View();
        }

        [HttpGet]
        public IActionResult AssignInstructor()
        {
            Instructor instructor = new Instructor();
            instructor.GetInstructors();
            if (instructor.Instructors != null)
            {
                List<Instructor> availableInstructors = new List<Instructor> { };
                foreach (Instructor i in instructor.Instructors)
                {
                    if (i.IsDeleted == 0)
                    {
                        availableInstructors.Add(i);
                    }
                }
                ViewBag.Instructors = new SelectList(availableInstructors, "InstructorId", "Name");
            }

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
                ViewBag.Courses = new SelectList(availableCourses, "Course_Id", "Title");
            }


            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"];
            }
            Console.WriteLine("In AssignInstructor");

            return View("AddInstructorAssignment");
        }

        [HttpPost]
        public IActionResult AddAssignmentDetails(InstructorAssignment assignment)
        {
            try
            {
                if (string.IsNullOrEmpty(assignment.Course_Id.ToString())
                    || string.IsNullOrEmpty(assignment.Instructor_Id.ToString())
                    || string.IsNullOrEmpty(assignment.AssignedDate.ToString()))
                {
                    TempData["ErrorMessage"] = "Empty field values";
                    return View();
                }
                else
                {
                    assignment.AddInstructorAssignment(assignment);
                    TempData["Message"] = "Assignment details inserted successfully";
                    return RedirectToAction("InstructorAssignmentView");
                }
            }
            catch (OracleException ex) when (ex.Number == 1)
            {
                TempData["ErrorMessage"] = "The instructor has already been assigned to the course.";
                return RedirectToAction("InstructorAssignmentView");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
                return RedirectToAction("InstructorAssignmentView");
            }
        }

        [HttpGet]
        public ActionResult DeleteAssignment(int courseId, int instructorId)
        {
            InstructorAssignment assignment = new InstructorAssignment();
            assignment = assignment.GetInstructorAssignmentById(courseId, instructorId);
            ViewBag.SQLData = assignment;
            return View("DeleteInstructorAssignment", assignment);
        }

        // POST: EnrolmentController/Delete/5
        [HttpPost]
        public ActionResult DeleteInstructorAssignment(int courseId, int instructorId)
        {
            try
            {
                InstructorAssignment enrolment = new InstructorAssignment();
                enrolment.DeleteAssignment(courseId, instructorId);
                TempData["Message"] = "Instructor Assignment deleted successfully";
                return RedirectToAction("InstructorAssignmentView");
            }
            catch (Exception exp)
            {
                TempData["Message"] = $"Exception occured: {exp}";
                return RedirectToAction("InstructorAssignmentView");
            }
        }

        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: InstructorAssignmentController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: InstructorAssignmentController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: InstructorAssignmentController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: InstructorAssignmentController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: InstructorAssignmentController/Delete/5
        
    }
}
