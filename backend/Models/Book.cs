namespace Model;

// TODO(garipew): Book model should allow co-authors mapping
public class Book
{
	public int Id                   { get; set; }
	public int AuthorId             { get; set; }
	public Author Author            { get; set; }
	public string Isbn              { get; set; }
	public int Pages                { get; set; }
	public string Title             { get; set; }
	public string? Description      { get; set; }
}
