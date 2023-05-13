using System.ComponentModel.DataAnnotations;

namespace Mag.Common.Models;

public class UserEditDto
{
    [Required(ErrorMessage = "Поле не заполнено")]
    [Display(Name = "Имя пользователя")]
    public string? UserName { get; set; }
    
    [Required(ErrorMessage = "Поле не заполнено")]
    [Display(Name = "Старый пароль")]
    [StringLength(20, MinimumLength =3, ErrorMessage = "Не достаточно символов")]
    [DataType(DataType.Password)]
    public string? PasswordOld { get; set; }
    
    [Required(ErrorMessage = "Поле не заполнено")]
    [Display(Name = "Новый пароль")]
    [StringLength(20, MinimumLength =3, ErrorMessage = "Не достаточно символов")]
    [DataType(DataType.Password)]
    public string? PasswordNew { get; set; }
}