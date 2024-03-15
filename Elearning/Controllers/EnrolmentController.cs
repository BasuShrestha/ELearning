using Elearning.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Oracle.ManagedDataAccess.Client;

namespace Elearning.Controllers
{
    public class EnrolmentController : Controller
    {
        // GET: EnrolmentController
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult EnrolmentView()
        {
            Enrolment enrollment = new Enrolment();
            enrollment.GetEnrolments();
            ViewBag.SQLData = enrollment;

            Student student = new Student();
            student.GetStudents();
            if (student.Students != null)
            { 
                List<Student> currentStudents = new List<Student>();
                foreach (Student s in student.Students) 
                {
                    if (s.Is_Deleted == 0)
                    { 
                        currentStudents.Add(s);
                    }
                }
                ViewBag.Students = new SelectList(currentStudents, "Student_Id", "Student_Name");
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
            if (TempData["ErrorMessage"] != null)
            {
                ViewBag.Message = TempData["ErrorMessage"];
            }

            return View();
        }

        // GET: EnrolmentController/Create
        public IActionResult AddEnrolment()
        {
            Student student = new Student();
            student.GetStudents();
            if (student.Students != null)
            {
                List<Student> currentStudents = new List<Student>();
                foreach (Student s in student.Students)
                {
                    if (s.Is_Deleted == 0)
                    {
                        currentStudents.Add(s);
                    }
                }
                ViewBag.Students = new SelectList(currentStudents, "Student_Id", "Student_Name");
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
            return View("AddEnrolmentDetail");
        }

        // POST: EnrolmentController/Create
        [HttpPost]
        public IActionResult AddEnrolmentDetail(Enrolment enrolment)
        {
            try
            {
                if (string.IsNullOrEmpty(enrolment.Student_Id.ToString()) 
                    || string.IsNullOrEmpty(enrolment.Course_Id.ToString()) 
                    || string.IsNullOrEmpty(enrolment.EnrolledOnDate.ToString()))
                {
                    TempData["Message"] = "Empty field values";
                    return View();
                }
                else
                {
                    enrolment.AddEnrolment(enrolment);
                    TempData["Message"] = "Enrolment details inserted successfully";
                    return RedirectToAction("EnrolmentView");
                }
            }
            catch (OracleException ex) when (ex.Number == 1) // Catching Oracle unique constraint error
            {
                // Handle the unique constraint violation
                TempData["ErrorMessage"] = "The student has already been enrolled in the course.";
                return RedirectToAction("EnrolmentView");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
                return RedirectToAction("EnrolmentView");
            }
        }

        // GET: EnrolmentController/UpdateEnrolment/5
        public ActionResult UpdateEnrolment(int id)
        {
            return View();
        }

        // POST: EnrolmentController/UpdateEnrolment/5
        [HttpPost]
        public ActionResult UpdateEnrolment(int id, IFormCollection collection)
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

        // GET: EnrolmentController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: EnrolmentController/Delete/5
        [HttpPost]
        public ActionResult DeleteEnrolment(int studentId, int courseId)
        {
            try
            {
                Enrolment enrolment = new Enrolment();
                enrolment.DeleteEnrolment(studentId, courseId);
                TempData["Message"] = "Enrolment deleted successfully";
                return RedirectToAction("EnrolmentView");
            }
            catch (Exception exp)
            {
                TempData["Message"] = $"Exception occured: {exp}";
                return RedirectToAction("EnrolmentView");
            }
        }
    }
}
