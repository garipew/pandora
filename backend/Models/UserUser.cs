using Microsoft.EntityFrameworkCore;

namespace Models;

[PrimaryKey(nameof(FollowerId), nameof(FollowedId))]
public class UserUser
{
	public int FollowerId      { get; set; }
	public User Follower       { get; set; }
	public int FollowedId      { get; set; }
	public User Followed       { get; set; }
	public DateTime createdAt  { get; set; }
}
