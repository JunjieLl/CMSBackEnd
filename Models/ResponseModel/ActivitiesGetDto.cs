using System.Text.Json.Serialization;
using CMS.CONFIG;
namespace CMS.Models;

public class ActivitiesGetDto
{
    public string ActivityId { get; set; } = null!;

    public string ActivityName { get; set; } = null!;

    public string ActivityStatus { get; set; } = null!;

    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime StartTime { get; set; }

    public int Duration { get; set; }


    public string? ActivityDescription { get; set; }

    public sbyte PoliticallyRelevant { get; set; } = 0;


    public string? PoliticalReview { get; set; }


    public string RoomId { get; set; } = null!;

    public string? ManagerUserId { get; set; }
    public string? CommonUserId { get; set; }

    public DateTime? EvaluateTime { get; set; }


    public string? Content { get; set; }

    public string UserName { get; set; } = null!;

    public string RoomName { get; set; } = null!;

    public string Building { get; set; } = null!;

    public string Floor { get; set; } = null!;
    public string Reason { get; set; } = "";
    public string? Image { get; set; }
}