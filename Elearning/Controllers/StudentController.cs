using Elearning.Models;
using Microsoft.AspNetCore.Mvc;

namespace Elearning.Controllers
{
    public class StudentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult StudentView()
        {
            Student student = new Student();
            student.GetStudents();
            ViewBag.sqldata = student;

            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"];
            }
            return View();
        }

        public IActionResult ShowEnrolmentDetails(int studentId)
        {
            EnrolledCourse enrolledCourse = new EnrolledCourse();
            List<EnrolledCourse> enrolledCourses = enrolledCourse.GetEnrolledCoursesByStudentId(studentId);
            ViewBag.EnrolledCourses = enrolledCourses;

            Student student = new Student();
            student.GetStudentById(studentId);
            student = student.Students[0];
            ViewBag.StudentDetails = student;

            ViewBag.fromPage = "Students";

            return View("../Enrolment/EnrolmentDetails");
        }

        public IActionResult AddStudent()
        {
            return View("AddStudentDetail");
        }

        [HttpPost]
        public IActionResult AddStudentDetail(Student student)
        {

            Console.WriteLine($"Student ID: {student.Student_Id}");
            Console.WriteLine($"Student Name: {student.Student_Name}");
            Console.WriteLine($"Contact: {student.Contact}");
            Console.WriteLine($"DOB: {student.DOB}");
            Console.WriteLine($"Email: {student.Email}");
            Console.WriteLine($"Country: {student.Country}");
            if (string.IsNullOrEmpty(student.Student_Name)
                || string.IsNullOrEmpty(student.Contact)
                || string.IsNullOrEmpty(student.Email)
                || string.IsNullOrEmpty(student.Country))
            {
                TempData["Message"] = "Please fill all the fields before submission";
                return View(student);
            }
            else
            {
                student.AddStudent(student);
                TempData["Message"] = "Student Added Successfully";
                return RedirectToAction("StudentView");
            }
        }

        public IActionResult UpdateStudent(int studentId) 
        {
            Console.WriteLine($"In here: {studentId}");
            Student student = new Student();
            student.GetStudentById(studentId);
            Console.WriteLine(student.Students[0].Student_Name);
            ViewBag.studentData = student;
            return View("UpdateStudent", student);
        }

        [HttpPost]
        public IActionResult UpdateStudent(Student student)
        {
            if (!ModelState.IsValid)
            {
                TempData["Message"] = "Please fill all the fields before submission";
                return View(student);
            }
            else 
            {
                student.UpdateStudent(student);
                return RedirectToAction("StudentView");
            }
        }

        [HttpGet]
        public IActionResult DeleteStudent(int studentId)
        {
            Student student = new Student();
            student.GetStudentById(studentId);
            student = student.Students[0];
            ViewBag.sqldata = student;
            return View("DeleteStudent", student);
        }

        [HttpPost]
        public IActionResult DeleteStudentDetail(int studentId)
        {
            try
            {
                Student student = new Student();
                student.DeleteById(studentId);
                ViewBag.Message = $"Student deleted from the database";
                Console.WriteLine(studentId);
                TempData["Message"] = "Student deleted!";
                return RedirectToAction("StudentView");
            }
            catch (Exception exp)
            {
                TempData["Message"] = $"Error: {exp.Message}";
                return RedirectToAction("StudentView");
            }
        }
    }
}
