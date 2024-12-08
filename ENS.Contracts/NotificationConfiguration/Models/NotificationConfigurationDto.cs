namespace ENS.Contracts.NotificationConfiguration.Models;
public class NotificationConfigurationDto
{
    public Guid? UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public Channel[]? Channels { get; set; }
    public string[]? UserIdInChannels { get; set; }
    public string TemplateId { get; set; }
    public string TemplateMessage { get; set; }
    public Channel[]? TemplateChannels { get; set; }
}