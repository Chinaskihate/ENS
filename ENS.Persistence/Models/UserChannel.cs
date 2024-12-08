using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ENS.Persistence.Models;
[Table("UserChannels")]
internal class UserChannel
{
    [Key]
    public Guid Id { get; set; }
    public User User { get; set; }
    public int Channel { get; set; }
    public string UserIdInChannel { get; set; }
}
