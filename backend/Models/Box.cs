namespace Model;

public class Box
{
	public int Id                   { get; set; }
	public int OwnerId              { get; set; }
	public User Owner               { get; set; }
	public string Title             { get; set; }
	public string? Description      { get; set; }
}
