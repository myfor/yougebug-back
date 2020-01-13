using System.ComponentModel.DataAnnotations;

namespace DB.Tables
{
    public class Tag : Entity
    {
        /// <summary>
        /// 
        /// </summary>
        [Required, StringLength(16)]
        public string Name { get; set; } = "";
    }
}
