using Elearning.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Oracle.ManagedDataAccess.Client;

namespace Elearning.Controllers
{
    public class CourseProgressController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult CourseProgressView()
        {
            CourseProgress progress = new CourseProgress();
            List<CourseProgress> courseProgresses = progress.GetCourseProgress();
            ViewBag.SQLData = courseProgresses;

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

        public IActionResult AddProgress()
        {
            Enrolment enrolment = new Enrolment();
            enrolment.GetEnrolments();
            List<Enrolment> enrolments = enrolment.Enrolments;
            ViewBag.Enrolments = enrolments;

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
                ViewBag.AllStudents = currentStudents;
            }

            EnrolledCourse enrolledCourse = new EnrolledCourse();
            List<EnrolledCourse> eCourses = enrolledCourse.GetDistinctEnrolledCourses();
            List<Lesson> eCourseLessons = new List<Lesson>();
            foreach (EnrolledCourse ec in eCourses) 
            { 
                Lesson lesson = new Lesson();
                var lessons = lesson.GetLessonByCourseId(ec.Course_Id);
                foreach (Lesson l in lessons)
                { 
                    eCourseLessons.Add(l);
                }
            }
            ViewBag.EnrolledLessons = eCourseLessons;
            ViewBag.EnrolledCourses = new SelectList(eCourses, "Course_Id", "Title");

            List<String> statusTypes = new List<String>
            {
                "Not Started", "In Progress", "Completed"
            };
            ViewBag.StatusTypes = new SelectList(statusTypes);

            return View("AddCourseProgress");
        }

        [HttpPost]
        public IActionResult AddProgressDetail(CourseProgress progress)
        {
            try
            {
                if (string.IsNullOrEmpty(progress.StudentId.ToString())
                    || string.IsNullOrEmpty(progress.LessonId.ToString())
                    || string.IsNullOrEmpty(progress.LastAccessedDate.ToString()))
                {
                    TempData["Message"] = "Empty field values";
                    return View();
                }
                else
                {
                    progress.AddCourseProgress(progress);
                    TempData["Message"] = "CourseProgress details inserted successfully";
                    return RedirectToAction("CourseProgressView");
                }
            }
            catch (OracleException ex) when (ex.Number == 1) // Catching Oracle unique constraint error
            {
                // Handle the unique constraint violation
                TempData["ErrorMessage"] = "The student has already been enrolled in the course.";
                return RedirectToAction("CourseProgressView");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
                return RedirectToAction("CourseProgressView");
            }
        }


        // GET: ProgressController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ProgressController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProgressController/Create
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

        // GET: ProgressController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ProgressController/Edit/5
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

        // GET: ProgressController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ProgressController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
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
    }
}
