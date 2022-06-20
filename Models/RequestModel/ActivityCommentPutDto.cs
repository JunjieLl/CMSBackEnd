
using System.Text.Json.Serialization;
using CMS.CONFIG;

namespace CMS.Models;


public class ActivityCommentPutDto
{
    public string ActivityId { get; set; } = null!;

    public string ActivityStatus { get; set; } = null!;

    public string? ManagerUserId { get; set; }

    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime? EvaluateTime { get; set; }

    public string? Content { get; set; }
}