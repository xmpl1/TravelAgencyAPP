using System.ComponentModel.DataAnnotations;

namespace TravelAgencyAPP.ViewModels.Users
{
    public class EditUserViewModel
    {
        [Required]
        public string Id { get; set; }

        [Required(ErrorMessage = "Введите E-mail")]
        [Display(Name = "E-mail")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Введите корретный E-mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Введите фамилию")]
        [Display(Name = "Фамилия")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Введите имя")]
        [Display(Name = "Имя")]
        public string FirstName { get; set; }

        [Display(Name = "Дата регистрации")]
        public DateTime DateReg { get; set; }

        [Display(Name = "Дата приема на работу")]
        public DateTime DateWorking { get; set; }

        [Display(Name = "Дата увольнения")]
        public DateTime DateDismissal { get; set; }
    }
}
