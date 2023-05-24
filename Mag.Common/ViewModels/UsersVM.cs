using System.ComponentModel.DataAnnotations;

namespace Mag.Common.ViewModels;

public class UserVM
{
    public List<User> Users { get; set; } = new List<User>();
}

public class User
{
    [Display(Name = "Имя пользователя")]
    public string UserName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Id { get; set; }
    public string CreatedDate { get; set; }
    public string Role { get; set; }
    public string UserState { get; set; }

}