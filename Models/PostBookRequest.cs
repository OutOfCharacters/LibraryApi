using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LibraryApi.Models
{
    public class PostBookRequest :IValidatableObject
    {
        //these are max length, min length, regular expressions, emails, etc.
        [Required]
        [MaxLength(200)]
        public string Title { get; set; }
        [Required]
        public string Author { get; set; }
        public string Genre { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Title.ToLower() == "pet semetary" && Author.ToLower() == "king")
            {
                yield return new ValidationResult("That book is too scary!", new string[] { "title", "author" });
            }
        }
    }
}
