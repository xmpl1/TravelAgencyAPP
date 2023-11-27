using System.ComponentModel.DataAnnotations;

namespace TravelAgencyAPP.ViewModels.Countries
{
    public class CreateCountryViewModel
    {
        [Required(ErrorMessage = "Введите страну")]
        [Display(Name = "Страна")]
        public string CountryName { get; set; }
    }
}
