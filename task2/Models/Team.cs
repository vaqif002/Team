using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;

namespace task2.Models
{
    public class Team
    {
        public int id { get; set; }
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }
        [Required]
        [MaxLength(255)]
        public string  Description { get; set; }

        public bool IsDelected { get; set; }
        [Required]
        [MaxLength(255)]
        public string ImagePath { get; set; }
    }
}
