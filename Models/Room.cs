using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CMS
{
    [Table("room")]
    public partial class Room
    {
        public Room()
        {
            Activities = new HashSet<Activity>();
            Users = new HashSet<CommonUser>();
            UsersNavigation = new HashSet<RoomManager>();
        }

        [Key]
        [Column("room_id")]
        [StringLength(10)]
        public string RoomId { get; set; } = null!;
        [Column("room_name")]
        [StringLength(255)]
        public string RoomName { get; set; } = null!;
        [Column("building")]
        [StringLength(10)]
        public string Building { get; set; } = null!;
        [Column("floor")]
        [StringLength(10)]
        public string Floor { get; set; } = null!;
        [Column("room_description")]
        [StringLength(255)]
        public string? RoomDescription { get; set; }
        [Column("capacity")]
        public int? Capacity { get; set; }
        [Column("image")]
        [StringLength(255)]
        public string? Image { get; set; }

        [InverseProperty("Room")]
        public virtual ICollection<Activity> Activities { get; set; }

        [ForeignKey("RoomId")]
        [InverseProperty("Rooms")]
        public virtual ICollection<CommonUser> Users { get; set; }
        [ForeignKey("RoomId")]
        [InverseProperty("Rooms")]
        public virtual ICollection<RoomManager> UsersNavigation { get; set; }
    }
}
