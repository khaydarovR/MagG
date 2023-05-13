using System.ComponentModel.DataAnnotations;

namespace Mag.Common.Models;

public class UserRegisterDto
{
    [Required(ErrorMessage = "Обязательное поле")]
    [Display(Name = "Номер телефона")]
    [DataType(DataType.PhoneNumber)]
    [Phone(ErrorMessage = "Не правильный формат номера")]
    public string Phone { get; set; } = null!;
    
    [Required(ErrorMessage = "Обязательное поле")]
    [EmailAddress(ErrorMessage = "Не правильный формат Email")]
    [Display(Name = "Email")]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }
    
    [Required(ErrorMessage = "Обязательное поле")]
    [Display(Name = "Пароль")]
    [StringLength(20, MinimumLength =3, ErrorMessage = "Не достаточно символов")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    
    [Required(ErrorMessage = "Правильно заполните поле")]
    [Compare("Password", ErrorMessage = "Пароли не совпадают")]
    [Display(Name = "Повторите пароль")]
    [StringLength(20, MinimumLength =3, ErrorMessage = "Не достаточно символов")]
    [DataType(DataType.Password)]
    public string Password2 { get; set; }
}