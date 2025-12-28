using GoTogether.Domain.Common;

namespace GoTogether.Domain.Entities;

public class Event(string Title, DateTime StartsAt, string Location, string Description) : Entity
{
    public string Title { get; set; } = Title;
    public DateTime StartsAt { get; set; } = StartsAt;
    public string Location { get; set; } = Location;
    public string Description { get; set; } = Description;
    public string? ImageFileName { get; private set; } = null;

    public ICollection<EventInterest> EventInterests { get; private set; } = [];

    public void Update(string title, string description, DateTime startsAt, string location)
    {
        Title = title;
        Description = description;
        StartsAt = startsAt;
        Location = location;
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
