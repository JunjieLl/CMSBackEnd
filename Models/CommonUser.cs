using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CMS
{
    [Table("common_user")]
    public partial class CommonUser
    {
        public CommonUser()
        {
            Favorites = new HashSet<Favorite>();
        }

        [Key]
        [Column("user_id")]
        public string UserId { get; set; } = null!;
        [Column("time_limit")]
        public int TimeLimit { get; set; }
        [Column("count_limit")]
        public int CountLimit { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("CommonUser")]
        public virtual User User { get; set; } = null!;
        [InverseProperty("User")]
        public virtual ICollection<Favorite> Favorites { get; set; }
    }
}
