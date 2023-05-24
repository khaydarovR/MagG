using System.ComponentModel.DataAnnotations;

namespace Mag.Common.ViewModels;

public class UserEditVM
{
    public string UserId { get; set; }
    [Display(Name = "Новое имя пользователя")]
    public string UserName { get; set; }
    [Display(Name = "Роль пользователя")]
    public string SelectedRole { get; set; }
    public List<string> Roles { get; set; }
}