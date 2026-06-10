namespace Models;

public class User
{
	public int Id              { get; set; }
	public string Username     { get; set; }
	public string Email        { get; set; }
	public string passwordHash { get; set; }
	public DateTime createdAt  { get; set; }
}
