using CMS.CONFIG;
using System.Text.Json.Serialization;
namespace CMS.Models;

public class PeriodActivityPostDto
{
    public string ActivityName { get; set; } = null!;

    public string ActivityStatus { get; set; } = null!;

    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime StartTime { get; set; }

    public int Duration { get; set; }

    public string? ActivityDescription { get; set; }

    public sbyte PoliticallyRelevant { get; set; }

    public string RoomId { get; set; } = null!;

    public string? CommonUserId { get; set; }

    public int managerWeek { get; set; }
}