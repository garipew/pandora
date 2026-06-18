using Microsoft.EntityFrameworkCore;

namespace Model;

[PrimaryKey(nameof(BoxId), nameof(BookId))]
public class BoxBook
{
	public int BoxId                { get; set; }
	public Box Box                  { get; set; }
	public int BookId               { get; set; }
	public Book Book                { get; set; }
}
