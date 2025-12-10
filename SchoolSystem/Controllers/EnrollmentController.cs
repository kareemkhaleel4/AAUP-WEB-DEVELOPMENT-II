using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolAPI.Data;
using SchoolAPI.Models;

namespace SchoolAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnrollmentsController : ControllerBase
    {
        private readonly SchoolContext _context;

        public EnrollmentsController(SchoolContext context)
        {
            _context = context;
        }

        // POST: api/Enrollments/enroll
        [HttpPost("enroll")]
        public async Task<IActionResult> EnrollStudent(int studentId, int courseId)
        {
            var exists = await _context.Enrollments
                .AnyAsync(e => e.StudentId == studentId && e.CourseId == courseId);

            if (exists)
                return BadRequest("Student already enrolled in this course.");

            var enrollment = new Enrollment
            {
                StudentId = studentId,
                CourseId = courseId
            };

            _context.Enrollments.Add(enrollment);
            await _context.SaveChangesAsync();

            return Ok(enrollment);
        }

        // DELETE
        [HttpDelete("unenroll")]
        public async Task<IActionResult> UnenrollStudent(int studentId, int courseId)
        {
            var enr = await _context.Enrollments
                .FirstOrDefaultAsync(e => e.StudentId == studentId && e.CourseId == courseId);

            if (enr == null) return NotFound();

            _context.Enrollments.Remove(enr);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
