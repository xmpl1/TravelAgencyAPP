using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.CodeAnalysis;

namespace TravelAgencyAPP.Models.Data
{
    public class Tour
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ИД")]
        public short Id { get; set; }

        [Required(ErrorMessage = "Введите название тура")]
        [Display(Name = "Тур")]
        public string TourName { get; set; }

      /*  [Required]
        [Display(Name = "Выберите страну")]
        public short IdCountry { get; set; }*/

        [Required]
        [Display(Name = "Выберите отель")]
        public short IdHotel { get; set; }

        [Required(ErrorMessage = "Введите количество ночей")]
        [Display(Name = "Количество ночей")]
        public short NumberOfNights { get; set; }

        [Required]
        [Display(Name = "Пользователь")]
        public string IdUser { get; set; }

        // Навигационные свойства

       /* [Display(Name = "Страна")]
        [ForeignKey("IdCountry")]
        public Country Country { get; set; }*/


        [Display(Name = "Отель")]
        [ForeignKey("IdHotel")]
        public Hotel Hotel { get; set; }


        [Display(Name = "Пользователь")]
        [ForeignKey("IdUser")]
        public User User { get; set; }

    }
}
