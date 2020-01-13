using System;
using System.ComponentModel.DataAnnotations;

namespace DB.Tables
{
    public class Question : Entity
    {
        /// <summary>
        /// 
        /// </summary>
        [Required, StringLength(64)]
        public string Title { get; set; } = "";
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string Description { get; set; } = "";
        /// <summary>
        /// 
        /// </summary>
        [Required, StringLength(64)]
        public string Tags { get; set; } = "";
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public int Weights { get; set; } = 0;
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public int Views { get; set; } = 0;
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public DateTimeOffset Actived { get; set; } = DateTimeOffset.Now;
        public User Creator { get; set; }
    }
}
