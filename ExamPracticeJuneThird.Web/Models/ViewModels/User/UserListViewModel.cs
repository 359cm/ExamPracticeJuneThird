// Models/ViewModels/User/UserListViewModel.cs
public class UserListViewModel
{
    public List<UserViewModel> Users { get; set; }
    public int TotalCount { get; set; } 
}

public class UserViewModel
{
    public int UserId { get; set; }
    public string Username { get; set; }
    public string FullName { get; set; }
    public DateTime BirthDate { get; set; } 
}
