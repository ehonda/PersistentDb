using JetBrains.Annotations;

namespace PersistentDb;

[PublicAPI]
public class Movie
{
    public int Id { get; set; }

    public string Title { get; set; }

    public Movie(string title) => Title = title;
}