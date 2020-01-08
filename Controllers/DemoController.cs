using LibraryApi.Utils;
using Microsoft.AspNetCore.Mvc;
using System;

namespace LibraryApi.Controllers
{
    public class DemoController : Controller
    {
        IGenerateIds idGenerator;

        public DemoController(IGenerateIds idGenerator)
        {
            this.idGenerator = idGenerator;
        }

        //this is how you add single parameters into the path.
        [HttpGet("/beerme/{qty}")]
        public IActionResult GetBeer(int qty)
        {
            string plural = qty == 1 ? "" : "s";
            return Ok($"Giving you {qty} beer{plural}");
        }

        //this is how you limit what your parameters can be
        [HttpGet("blogs/{year:int:min(2015)}/{month:int:range(1,12)}/{day:int:range(1,31)}")]
        public IActionResult GetPostFor(int year, int month, int day)
        {
            return Ok($"Getting blog posts for {year}/{month}/{day}");
        }

        //this is how you use a query string
        [HttpGet("/magazines")]
        public IActionResult GetMagazines([FromQuery] string topic)
        {
            return Ok($"Giving you magazines for {topic}!");
        }

        //this is how you pull information from the header
        [HttpGet("/whoami")]
        public IActionResult WhoAmI([FromHeader(Name = "User-Agent")] string ua)
        {
            return Ok($"I see you are running {ua}");
        }

        //This is how you pull information from the body. You need a class to use!
        [HttpPost("/courseenrollments")]
        public IActionResult EnrollForCourse([FromBody] EnrollmentRequest enrollment)
        {
            //validate it... (do this later
            //add it to the domain (database, etc.)
            //return some kind of meaningful response.
            var response = new EnrollmentResponse
            {
                CourseId = enrollment.CourseId,
                Instructor = enrollment.Instructor,
                EnrollmentId = idGenerator.GetEnrollmentId()
            };

            return Ok(response);
        }

        public class EnrollmentRequest
        {
            public string CourseId { get; set; }
            public string Instructor { get; set; }
        }

        public class EnrollmentResponse
        {
            public string CourseId { get; set; }
            public string Instructor { get; set; }
            public Guid EnrollmentId { get; set; }
        }
    }
}
