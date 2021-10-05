using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Project.ENTITIES.ValueObjects
{
    public class RegisterVM
    {
        [DisplayName("Kullanıcı adı"), Required(ErrorMessage = "{0} boş geçilemez.")]
        [StringLength(25,ErrorMessage = "{0} maksimum {1} olmalıdır.")]
        public string Username { get; set; }

        [DisplayName("E-Posta"), Required(ErrorMessage = "{0} boş geçilemez.")]
        [StringLength(70, ErrorMessage = "{0} maksimum {1} olmalıdır.")]
        [EmailAddress(ErrorMessage ="Geçerli bir {0} alanı giriniz")]
        public string Email { get; set; }

        [DisplayName("Şifre"), Required(ErrorMessage = "{0} boş geçilemez.")]
        [DataType(DataType.Password)]
        [StringLength(25, ErrorMessage = "{0} maksimum {1} olmalıdır.")]
        public string Password { get; set; }

        [DisplayName("Şifre Tekrar"), Required(ErrorMessage = "{0} boş geçilemez.")]
        [DataType(DataType.Password)]
        [StringLength(25, ErrorMessage = "{0} maksimum {1} olmalıdır.")]
        [Compare("Password", ErrorMessage ="{0} ile {1} uyuşmuyor ")]
        public string RePassword { get; set; }

    }
}