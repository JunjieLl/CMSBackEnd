using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CMS
{
    [Table("user")]
    public partial class User
    {
        public User()
        {
            Activities = new HashSet<Activity>();
            ModifyRecords = new HashSet<ModifyRecord>();
        }

        [Key]
        [Column("user_id")]
        public string UserId { get; set; } = null!;
        [Column("user_name")]
        [StringLength(255)]
        public string UserName { get; set; } = null!;
        [Column("email_address")]
        [StringLength(255)]
        public string EmailAddress { get; set; } = null!;
        [Column("password")]
        [StringLength(255)]
        public string? Password { get; set; }
        [Column("identity")]
        [StringLength(10)]
        public string Identity { get; set; } = null!;
        /// <summary>
        /// 1代表正常，0代表禁用
        /// </summary>
        [Column("activity_status")]
        [StringLength(10)]
        public string? ActivityStatus { get; set; } = "1";
        [Column("avatar")]
        [StringLength(255)]
        public string? Avatar { get; set; }
        [Column("wechat_id")]
        [StringLength(255)]
        public string? WechatId { get; set; }

        [InverseProperty("User")]
        public virtual CommonUser CommonUser { get; set; } = null!;
        [InverseProperty("User")]
        public virtual RoomManager RoomManager { get; set; } = null!;
        [InverseProperty("CommonUser")]
        public virtual ICollection<Activity> Activities { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<ModifyRecord> ModifyRecords { get; set; }
    }
}
