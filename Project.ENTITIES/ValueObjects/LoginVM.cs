using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Project.ENTITIES.ValueObjects
{
    public class LoginVM
    {
        [DisplayName("Kullanıcı adı"), Required(ErrorMessage = "{0} alanı boş geçilemez.")]
        [StringLength(25,ErrorMessage = "{0} maksimum {1} olmalıdır.")]
        public string Username { get; set; }

        [DisplayName("Şifre"), Required(ErrorMessage = "{0} alanı boş geçilemez.")]
        [DataType(DataType.Password)]
        [StringLength(25, ErrorMessage = "{0} maksimum {1} olmalıdır.")]
        public string Password { get; set; }
    }
}