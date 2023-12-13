using System.ComponentModel.DataAnnotations;

namespace TravelAgencyAPP.ViewModels.Tours
{
    public class EditToursViewModel
    {
        public short Id { get; set; }

        [Required(ErrorMessage = "Введите название тура")]
        [Display(Name = "Название тура")]
        public string TourName { get; set; }

        /*[Required]
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
    }
}
