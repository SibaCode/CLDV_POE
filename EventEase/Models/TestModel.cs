using System.ComponentModel.DataAnnotations;

namespace EventEase.Models
{
    public class TestModel
    {
        public int Id { get; set; }  // Primary Key

        [Required]
        public string Name { get; set; }

        [Required]
        public string Surname { get; set; }

        [Required]
        public int Age { get; set; }
    }
}
