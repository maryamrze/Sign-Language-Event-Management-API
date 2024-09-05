using System.ComponentModel.DataAnnotations;
namespace a2.Models{
    public class Organizer{
        [Key]
        public string Name{get; set;}
        [Required]
        public string Password {get; set;}
    }
}