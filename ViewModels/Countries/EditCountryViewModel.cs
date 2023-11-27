using System.ComponentModel.DataAnnotations;

namespace TravelAgencyAPP.ViewModels.Countries
{
    public class EditCountryViewModel
    {
        public short Id { get; set; }

        [Required(ErrorMessage = "Введите страну")]
        [Display(Name = "Страна")]
        public string CountryName { get; set; }
    }
}
