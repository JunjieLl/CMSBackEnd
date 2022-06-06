using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CMS
{
    [Table("room_manager")]
    public partial class RoomManager
    {
        public RoomManager()
        {
            Activities = new HashSet<Activity>();
            Rooms = new HashSet<Room>();
        }

        [Key]
        [Column("user_id")]
        public string UserId { get; set; } = null!;
        [Column("is_super_manager")]
        public sbyte IsSuperManager { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("RoomManager")]
        public virtual User User { get; set; } = null!;
        [InverseProperty("ManagerUser")]
        public virtual ICollection<Activity> Activities { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("UsersNavigation")]
        public virtual ICollection<Room> Rooms { get; set; }
    }
}
