using System.ComponentModel.DataAnnotations;

namespace task2.Areas.Admin.Viewmodel
{
    public class CreatedTeamVM
    {
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }
        [Required]
        [MaxLength(255)]
        public string Description { get; set; }
        [Required]
      
        public IFormFile Photo { get; set; }
    }
}
