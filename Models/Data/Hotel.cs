using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace TravelAgencyAPP.Models.Data
{
    public class Hotel
    {

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ИД")]
        public short Id { get; set; }

        [Required(ErrorMessage = "Введите название отеля ")]
        [Display(Name = "Название отеля")]
        public string HotelName { get; set; }

        [Display(Name = "Количество звезд")]
        public short NumberOfStars { get; set; }

        [Display(Name = "Рейтинг")]
        public string Rating { get; set; }

        [Display(Name = "Адрес")]
        public string Review { get; set; }

        [Required]
        [Display(Name = "Страна")]
        public short IdCountry { get; set; }

        // Навигационные свойсва

        [Display(Name = "Страна")]
        [ForeignKey("IdCountry")]
        public Country Country { get; set; }


    }
}
