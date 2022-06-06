using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CMS
{
    [Table("modify_record")]
    [Index("ActivityId", Name = "activity_id")]
    [Index("UserId", Name = "modify_record_ibfk_1")]
    public partial class ModifyRecord
    {
        [Key]
        [Column("record_id")]
        [StringLength(10)]
        public string RecordId { get; set; } = null!;
        [Column("reason")]
        [StringLength(255)]
        public string Reason { get; set; } = null!;
        [Column("modify_time", TypeName = "datetime")]
        public DateTime ModifyTime { get; set; }
        /// <summary>
        /// any user who change the detail
        /// </summary>
        [Column("user_id")]
        public string UserId { get; set; } = null!;
        [Column("activity_id")]
        [StringLength(10)]
        public string ActivityId { get; set; } = null!;

        [ForeignKey("ActivityId")]
        [InverseProperty("ModifyRecords")]
        public virtual Activity Activity { get; set; } = null!;
        [ForeignKey("UserId")]
        [InverseProperty("ModifyRecords")]
        public virtual User User { get; set; } = null!;
    }
}
