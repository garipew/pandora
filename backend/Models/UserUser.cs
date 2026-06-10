using Microsoft.EntityFrameworkCore;

namespace Models;

[PrimaryKey(nameof(FollowerId), nameof(FollowedId))]
public class UserUser
{
	public int FollowerId      { get; set; }
	public int FollowedId      { get; set; }
	public DateTime createdAt  { get; set; }
}
