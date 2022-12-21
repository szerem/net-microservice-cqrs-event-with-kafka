using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using CQRS.Core.Commands;

namespace Post.Cmd.Api.Commands
{
    public class NewPostCommand : BaseCommand
    {
        [JsonIgnore]
        public Guid Id { get; set; }
        [MaxLength(64)]
        public string Author { get; set; }
        public string Message { get; set; }
    }
}