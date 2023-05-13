using Mag.Common;

namespace Mag.DAL.Entities;

public class UserState
{
    public Guid Id { get; set; }
    
    public StateEnum State { get; set; }

    public string? Descriptions { get; set; }
}