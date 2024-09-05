using System.ComponentModel.DataAnnotations;
namespace a2.Models{
    public class Sign{
        [Key]
        public string Id{get; set;}
        [Required]
        public string Description{get; set;}
    }
}