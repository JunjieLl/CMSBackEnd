using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using ConverterLibrary;

using System.Text.Json.Serialization;

namespace CMS
{
    [Table("room")]
    public partial class Room
    {
        public Room()
        {
            Activities = new HashSet<Activity>();
            Favorites = new HashSet<Favorite>();
            Manages = new HashSet<Manage>();
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

        [JsonConverter(typeof(FloorConverter))]
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
        [InverseProperty("Room")]
        public virtual ICollection<Favorite> Favorites { get; set; }
        [InverseProperty("Room")]
        public virtual ICollection<Manage> Manages { get; set; }
    }
}
