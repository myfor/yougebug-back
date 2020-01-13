using System.ComponentModel.DataAnnotations;

namespace DB.Tables
{
    public class File : Entity
    {
        /// <summary>
        /// 文件名
        /// </summary>
        [StringLength(50), Required]
        public string Name { get; set; } = "";
        /// <summary>
        /// 扩展名
        /// </summary>
        [StringLength(50), Required]
        public string ExtensionName { get; set; } = "";
        /// <summary>
        /// 文件大小, 字节
        /// </summary>
        [Required]
        public long Size { get; set; } = 0;
        /// <summary>
        /// 保存的相对路径
        /// </summary>
        [StringLength(256), Required]
        public string Path { get; set; } = "";
        /// <summary>
        /// 缩略图保存的相对路径
        /// </summary>
        [StringLength(256), Required]
        public string Thumbnail { get; set; } = "";
    }
}
