using Microsoft.EntityFrameworkCore;

namespace Model;

public enum Status {
	READING,
	REREADING,
	FINISHED,
	ABANDONED,
	PLANNED
}

[PrimaryKey(nameof(UserId), nameof(BookId))]
public class UserBook
{
	public int UserId            { get; set; }
	public User User             { get; set; }
	public int BookId            { get; set; }
	public Book Book             { get; set; }
	public int Rating            { get; set; }
	public Status Status         { get; set; }
	public int PagesRead         { get; set; }
	public DateTime? BeginDate   { get; set; }
	public DateTime? FinishDate  { get; set; }
}
