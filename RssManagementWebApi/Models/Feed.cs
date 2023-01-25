namespace RssManagementWebApi.Models;

public class Feed
{
    public string Id { get; set; }
    public string? Title { get; set; }
    public string? Url { get; set; }
    public DateTime? CreatedDate { get; set; }
    public string? Description { get; set; }
    public bool Unread { get; set; }
}