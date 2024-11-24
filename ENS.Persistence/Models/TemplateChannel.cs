using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ENS.Persistence.Models;
[Table("TemplateChannels")]
internal class TemplateChannel
{
    [Key]
    public Guid Id { get; set; }
    public Template Template { get; set; }
    public Channel Channel { get; set; }
    public string Message { get; set; }
}
