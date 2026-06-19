using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

using Model;

namespace Data;

public class PandoraContext(DbContextOptions<PandoraContext> options) : DbContext(options)
{
	public DbSet<User>       Users          { get; set; }
	public DbSet<UserBook>   UserBooks      { get; set; }
	public DbSet<UserUser>   Connections    { get; set; }
	public DbSet<Box>        Boxes          { get; set; }
	public DbSet<BoxBook>    Entries        { get; set; }
	public DbSet<Book>       Books          { get; set; }
	public DbSet<Author>     Authors        { get; set; }
	public DbSet<AuthorBook> AuthorBooks    { get; set; }
}
