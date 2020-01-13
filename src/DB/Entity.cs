using System;
using System.ComponentModel.DataAnnotations;

namespace DB
{
    public class Entity
    {
        public int Id { get; set; }
        [Required]
        public int State { get; set; } = 1;
        [Required]
        public int CreatorId { get; set; } = 0;
        [Required]
        public DateTimeOffset CreateDate { get; set; } = DateTimeOffset.Now;
        [Required]
        public int ModifyId { get; set; } = 0;
        [Required]
        public DateTimeOffset ModifyDate { get; set; } = default;
    }
}
