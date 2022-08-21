using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookApi.Models
{
    public class book
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        [Key]
        public int Id { get; set; }

        [Column(TypeName = "Varchar(50)")]
        public string Name { get; set; }

        [Column(TypeName = "Varchar(50)")]
        public string Zoner { get; set; }


        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ReleaseDate { get; set; }
        public int Cost { get; set; }
        public string ImageUrl { get; set; }
    }
}
