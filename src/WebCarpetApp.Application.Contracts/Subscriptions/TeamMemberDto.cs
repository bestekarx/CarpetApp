using System;

namespace WebCarpetApp.Subscriptions;

public class TeamMemberDto
{
    public Guid UserId { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
    public string[] RoleNames { get; set; }
    public bool IsOwner { get; set; }
    public bool IsPrimaryOwner { get; set; }
    public DateTime JoinedDate { get; set; }
}