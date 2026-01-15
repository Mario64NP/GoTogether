using GoTogether.Domain.Common;

namespace GoTogether.Domain.Entities;

public class Event(string Title, DateTime StartsAt, string Location, string Category, string? Description) : Entity
{
    public string Title { get; set; } = Title;
    public DateTime StartsAt { get; set; } = StartsAt;
    public string Location { get; set; } = Location;
    public string Category { get; set; } = Category;
    public string? Description { get; set; } = Description;
    public string? ImageFileName { get; private set; } = null;

    public ICollection<EventInterest> EventInterests { get; private set; } = [];

    public void Update(string? title, string? description, DateTime? startsAt, string? location, string? category)
    {
        Title = title ?? Title;
        Description = description ?? Description;
        StartsAt = startsAt ?? StartsAt;
        Location = location ?? Location;
        Category = category ?? Category;
    }

    public void SetImage(string fileName)
    {
        ImageFileName = fileName;
    }

    public void RemoveImage()
    {
        ImageFileName = null;
    }
}
