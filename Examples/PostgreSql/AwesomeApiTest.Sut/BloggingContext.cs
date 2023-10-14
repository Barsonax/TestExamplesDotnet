namespace AwesomeApiTest;

using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

public class BloggingContext : DbContext
{
    public DbSet<Blog> Blogs { get; set; }
    public DbSet<Post> Posts { get; set; }

    public BloggingContext()
    {
    }

    public BloggingContext(DbContextOptions<BloggingContext> context) : base(context)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Database=hr;Username=postgres;Password=postgres");
        }
    }
}

public class Blog
{
    public int BlogId { get; set; }
    public required string Url { get; set; }

    public List<Post>? Posts { get; } = new();
}

public class Post
{
    public int PostId { get; set; }
    public required string Title { get; set; }
    public required string Content { get; set; }

    public required int BlogId { get; set; }
    public Blog? Blog { get; set; }
}
