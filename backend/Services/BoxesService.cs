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
}

public class BoxTitleConflictException : Exception
{
	public BoxTitleConflictException() : base() { }
}
