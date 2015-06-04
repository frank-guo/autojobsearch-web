using System.ComponentModel.DataAnnotations;

namespace MVCMovie.Models
{
    public class TitleCond
    {
        public int ID { get; set; }

        [StringLength(60, MinimumLength = 3)]
        [RegularExpression("^([a-zA-Z0-9 .'-]+)$", ErrorMessage = "Enter only alphabets, numbers and . ' - ")]
        public string titleCond { get; set; }

    }
}