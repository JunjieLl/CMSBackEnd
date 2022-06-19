using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CMS
{
    [Table("manage")]
    [Index("RoomId", Name = "room_id")]
    public partial class Manage
    {
        [Key]
        [Column("user_id")]
        public string UserId { get; set; } = null!;
        [Key]
        [Column("room_id")]
        [StringLength(10)]
        public string RoomId { get; set; } = null!;
        /// <summary>
        /// 占位符
        /// </summary>
        [Column("placeholder")]
        [StringLength(255)]
        public string? Placeholder { get; set; }

        [ForeignKey("RoomId")]
        [InverseProperty("Manages")]
        public virtual Room Room { get; set; } = null!;
        [ForeignKey("UserId")]
        [InverseProperty("Manages")]
        public virtual RoomManager User { get; set; } = null!;
    }
}
