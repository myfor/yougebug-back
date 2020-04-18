using System.ComponentModel.DataAnnotations;

namespace DB.Tables
{
    public class Tag : Entity
    {
        /// <summary>
        /// 
        /// </summary>
        [Required, StringLength(32)]
        public string Name { get; set; } = "";
    }
}
