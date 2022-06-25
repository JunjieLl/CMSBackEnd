using System.Text.Json.Serialization;
using ConverterLibrary;

namespace CMS.Models;

public class ModifyRecordDto
{
    public string RecordId { get; set; } = null!;

    public string Reason { get; set; } = null!;

    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime ModifyTime { get; set; }

    public string UserId { get; set; } = null!;

    public string ActivityId { get; set; } = null!;

}