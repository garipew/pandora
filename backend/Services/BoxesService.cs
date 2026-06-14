using Microsoft.AspNetCore.Mvc;

using Data;
using Model;

namespace Service;

public class BoxesService
{
	private PandoraContext _context;

	public BoxesService(PandoraContext ctx)
	{
		_context = ctx;
	}

	public Box CreateBox(int ownerId, string title, string? description)
	{
		if(!_context.Users.Any(u => u.Id == ownerId)) {
			throw new UserNotFoundException();
		}
		if(_context.Boxes.Where(u => u.Title == title).Any()) {
			throw new BoxTitleConflictException();
		}

		Box newBox = new Box();
		newBox.Title = title;
		newBox.Description = description;
		newBox.OwnerId = ownerId;

		_context.Boxes.Add(newBox);
		_context.SaveChanges();

		return newBox;
	}

	public List<Box> GetBoxes(int userId)
	{
		if(!_context.Users.Any(u => u.Id == userId)) {
			throw new UserNotFoundException();
		}
		return _context.Boxes.Where(b => b.OwnerId == userId).ToList();
	}

	public Box GetBoxById(int userId, int boxId)
	{
		if(!_context.Users.Any(u => u.Id == userId)) {
			throw new UserNotFoundException();
		}
		Box? box = _context.Boxes.FirstOrDefault(b => b.OwnerId == userId && b.Id == boxId);
		if(box == null) {
			throw new BoxNotFoundException();
		}
		return box;
	}

	public Box UpdateBox(int userId, int boxId, string title, string? description)
	{
		Box box = GetBoxById(userId, boxId);
		box.Title = title;
		box.Description = description;
		_context.SaveChanges();

		return box;
	}
}

public class BoxNotFoundException : Exception
{
	public BoxNotFoundException() : base() { }
}

public class BoxTitleConflictException : Exception
{
	public BoxTitleConflictException() : base() { }
}
