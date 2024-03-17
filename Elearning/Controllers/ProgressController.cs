using Elearning.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Oracle.ManagedDataAccess.Client;

namespace Elearning.Controllers
{
    public class ProgressController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ProgressView(string studentName, string courseTitle, string lessonTitle, DateTime? dateFrom, DateTime? dateTo)
        {
            //Progress progress = new Progress();
            //List<Progress> courseProgresses = progress.GetCourseProgress();
            //ViewBag.SQLData = courseProgresses;
            Progress progress = new Progress();
            List<Progress> courseProgresses = progress.GetCourseProgressFiltered(studentName, courseTitle, lessonTitle, dateFrom, dateTo);
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

        public IActionResult ShowEnrolledStudents()
        {
            Enrolment enrolment = new Enrolment();
            enrolment.GetEnrolments();
            List<Enrolment> enrolments = enrolment.Enrolments;
            ViewBag.Enrolments = enrolments;

            EnrolledCourse course = new EnrolledCourse();
            List<Student> studentsEnrolled = course.GetDistinctStudents();
            ViewBag.StudentsEnrolled = new SelectList(studentsEnrolled, "Student_Id", "Student_Name");

            List<String> statusTypes = new List<String>
            {
                "Not Started", "In Progress", "Completed"
            };
            ViewBag.StatusTypes = new SelectList(statusTypes);

            return View("SearchStudent");
        }

        public IActionResult SearchCourses(int studentId)
        {
            EnrolledCourse course = new EnrolledCourse();
            List<EnrolledCourse> coursesEnrolled = course.GetEnrolledCoursesByStudentId(studentId);

            Student student = new Student();
            student.GetStudentById(studentId);
            student = student.Students[0];
            ViewBag.Student = student;

            ViewBag.SelectedStudentId = studentId;
            ViewBag.CoursesEnrolled = new SelectList(coursesEnrolled, "Course_Id", "Title");

            return View("SearchCourses");
        }

        public IActionResult AddProgress(int courseId, int studentId)
        {
            Lesson lesson = new Lesson();
            List<Lesson> courseLessons = lesson.GetLessonByCourseId(courseId);
            Console.WriteLine($"Lesson count in add lesson: {courseLessons.Count}");
            ViewBag.CourseLessons = new SelectList(courseLessons, "Lesson_Id", "Title");

            Student student = new Student();
            student.GetStudentById(studentId);
            student = student.Students[0];
            ViewBag.Student = student;

            Course course = new Course();
            course.GetCourseById(courseId);
            course = course.Courses[0];
            ViewBag.Course = course;

            List<String> statusTypes = new List<String>
            {
                "Not Started", "In Progress", "Completed"
            };
            ViewBag.StatusTypes = new SelectList(statusTypes);
            
            return View("AddProgress");
        }

        [HttpPost]
        public IActionResult AddProgressDetail(Progress courseProgress)
        {
            try
            {
                Console.WriteLine($"Course Progress in AddCourseProgress: {courseProgress.LessonStatus}");
                if (string.IsNullOrEmpty(courseProgress.StudentId.ToString())
                    || string.IsNullOrEmpty(courseProgress.LessonId.ToString())
                    || string.IsNullOrEmpty(courseProgress.LessonStatus)
                    || string.IsNullOrEmpty(courseProgress.LastAccessedDate.ToString()))
                {
                    Progress progress = new Progress();
                    List<Progress> courseProgresses = progress.GetCourseProgress();
                    ViewBag.SQLData = courseProgresses;

                    TempData["Message"] = "Empty field values";
                    return View("ProgressView");
                }
                else
                {
                    courseProgress.AddCourseProgress(courseProgress);
                    TempData["Message"] = "Course progress inserted successfully";
                    return RedirectToAction("ProgressView");
                }
            }
            catch (OracleException ex)
            {
                //TempData["ErrorMessage"] = $"An Oracle Exception occured:{ex.Message}";
                TempData["ErrorMessage"] = $"Progress for the particular course for the student already exists";
                return RedirectToAction("ProgressView");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
                return RedirectToAction("ProgressView");
            }
        }

        [HttpGet]
        public ActionResult UpdateProgress(int studentId, int lessonId)
        {
            Progress courseProgress = new Progress();
            courseProgress = courseProgress.GetCourseProgressById(studentId, lessonId);
            ViewBag.SQLData = courseProgress;

            List<String> statusTypes = new List<String>
            {
                "Not Started", "In Progress", "Completed"
            };
            ViewBag.StatusTypes = new SelectList(statusTypes);

            return View("UpdateProgress", courseProgress);

        }


        [HttpPost]
        public ActionResult UpdateProgressDetail(Progress progress)
        {
            try
            {
                Progress courseProgress = new Progress();
                courseProgress.UpdateLessonProgress(progress);
                TempData["Message"] = "Course Progress updated successfully";
                return RedirectToAction("ProgressView");
            }
            catch (Exception exp)
            {
                TempData["Message"] = $"Exception occured: {exp}";
                return RedirectToAction("ProgressView");
            }
        }

        [HttpGet]
        public ActionResult DeleteProgress(int studentId, int lessonId)
        {
            Progress courseProgress = new Progress();
            courseProgress = courseProgress.GetCourseProgressById(studentId, lessonId);
            ViewBag.SQLData = courseProgress;
            return View("DeleteProgress", courseProgress);

        }

        [HttpPost]
        public ActionResult DeleteProgressDetails(int studentId, int lessonId)
        {
            try
            {
                Progress courseProgress = new Progress();
                courseProgress.DeleteCourseProgress(studentId, lessonId);
                TempData["Message"] = "Course Progress deleted successfully";
                return RedirectToAction("ProgressView");
            }
            catch (Exception exp)
            {
                TempData["Message"] = $"Exception occured: {exp}";
                return RedirectToAction("ProgressView");
            }
        }

    }
}
