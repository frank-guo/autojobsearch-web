using System.ComponentModel.DataAnnotations;

namespace MVCMovie.Models
{
    public class TitleCond
    {
        public int ID { get; set; }

        [StringLength(60, MinimumLength = 3)]
        public string titleCond { get; set; }

    }
}