using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BookSellingApp.Models
{
    public class Book
    {
        [Required]
        public int Id { get; set; }


        [DisplayName("Book Name"), StringLength(80), Required(ErrorMessage = "The name cann't be empty")]
        public string Name { get; set; }

        [DisplayName("Zoner"), StringLength(80), Required(ErrorMessage = "The name cann't be empty")]
        public string Zoner { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ReleaseDate { get; set; }
        public int Cost { get; set; }

    }
}
