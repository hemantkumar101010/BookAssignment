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
        public DateTime ReleaseDate { get; set; }
        public int Cost { get; set; }
    }
}
