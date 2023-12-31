﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace TravelAgencyAPP.Models.Data
{
    public class User : IdentityUser
    {

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


        //навигационные свойства
    }
}
