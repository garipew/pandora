using Microsoft.EntityFrameworkCore;

namespace Models;

public enum Status {
	READING,
	REREADING,
	FINISHED,
	ABANDONED,
	PLANNED
}

[PrimaryKey(nameof(BoxId), nameof(BookId))]
public class BoxBook
{
	public int BoxId                { get; set; }
	public int BookId               { get; set; }
	public Status Status            { get; set; }
	public int? Rating              { get; set; }
	public int? PagesRead           { get; set; }
}
