using System.ComponentModel.DataAnnotations;

namespace ENS.Persistence.Models;
internal class Template
{
    [Key]
    public Guid Id { get; set; }
    public List<TemplateChannel> Channels { get; set; }
    public List<User> Receivers { get; set; }
}
