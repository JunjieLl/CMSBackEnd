using System.Text.Json.Serialization;
using ConverterLibrary;
using System;
using System.Collections.Generic;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CMS
{
    [Table("activity")]
    [Index("CommonUserId", Name = "activity_ibfk_1")]
    [Index("ManagerUserId", Name = "manager_user_id")]
    [Index("RoomId", Name = "room_id")]
    public partial class Activity
    {
        public Activity()
        {
            ModifyRecords = new HashSet<ModifyRecord>();
        }

        [Key]
        [Column("activity_id")]
        [StringLength(10)]
        public string ActivityId { get; set; } = null!;
        [Column("activity_name")]
        [StringLength(255)]
        public string ActivityName { get; set; } = null!;
        [Column("activity_status")]
        [StringLength(255)]
        public string ActivityStatus { get; set; } = null!;
        [Column("start_time", TypeName = "datetime")]
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime StartTime { get; set; }
        [Column("duration")]
        public int Duration { get; set; }
        [Column("activity_description")]
        [StringLength(255)]
        public string? ActivityDescription { get; set; }
        [Column("politically_relevant")]
        public sbyte PoliticallyRelevant { get; set; }
        [Column("political_review")]
        [StringLength(255)]
        public string? PoliticalReview { get; set; }
        [Column("room_id")]
        [StringLength(10)]
        public string RoomId { get; set; } = null!;
        /// <summary>
        /// user who is going to organize an activity
        /// </summary>
        [Column("common_user_id")]
        public string? CommonUserId { get; set; }
        [Column("evaluate_time", TypeName = "datetime")]
        public DateTime? EvaluateTime { get; set; }
        [Column("content")]
        [StringLength(255)]
        public string? Content { get; set; }
        [Column("manager_user_id")]
        public string? ManagerUserId { get; set; }

        [ForeignKey("CommonUserId")]
        [InverseProperty("Activities")]
        public virtual User? CommonUser { get; set; }
        [ForeignKey("ManagerUserId")]
        [InverseProperty("Activities")]
        public virtual RoomManager? ManagerUser { get; set; }
        [ForeignKey("RoomId")]
        [InverseProperty("Activities")]
        public virtual Room Room { get; set; } = null!;
        [InverseProperty("Activity")]
        public virtual ICollection<ModifyRecord> ModifyRecords { get; set; }
    }
}
