namespace CarpetApp.Models.API.Dto;

/// <summary>
/// User profile information
/// </summary>
public class UserDto
{
    public Guid Id { get; set; }
    public string UserName { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public bool IsActive { get; set; }
    public List<string> Roles { get; set; } = new();
}

/// <summary>
/// Team member information
/// </summary>
public class TeamMemberDto
{
    public Guid Id { get; set; }
    public string UserName { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public List<string> Roles { get; set; } = new();
    public DateTime JoinedDate { get; set; }
    public bool IsActive { get; set; }
}

/// <summary>
/// Team invitation information
/// </summary>
public class InvitationDto
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public List<string> RoleNames { get; set; } = new();
    public string InvitationMessage { get; set; }
    public string Status { get; set; }
    public DateTime SentDate { get; set; }
    public DateTime ExpirationDate { get; set; }
}
