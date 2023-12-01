﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TravelAgencyAPP.Models.Data;

namespace TravelAgencyAPP.ViewModels.Hotels
{
    public class CreateHotelViewModel
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
        public string? Rating { get; set; }

        [Display(Name = "Адрес")]
        public string Review { get; set; }

        [Required(ErrorMessage = "Выберите страну ")]
        [Display(Name = "Страна")]
        public short IdCountry { get; set; }

    }
}
