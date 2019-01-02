using Apps.Model.Privilege;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Apps.Model
{
    public class LogRecord : IEquatable<LogRecord>
    {
        public long id { get; set; }

        [Display(Name = "时间")]
        [Required(ErrorMessage = "{0}必须输入")]
        public DateTime time { get; set; }

        [Display(Name = "内容")]
        [Required(ErrorMessage = "{0}必须输入")]
        [StringLength(150, ErrorMessage = "{0}的长度在{1}个字符之内")]
        public string content { get; set; }

        [Display(Name = "类型")]
        [Required(ErrorMessage = "{0}必须输入")]
        [StringLength(20, ErrorMessage = "{0}的长度在{1}个字符之内")]
        public string type { get; set; }

        
        [Display(Name = "用户")]
        [Required(ErrorMessage = "{0}必须输入")]
        public long userId { get; set; }

        [ForeignKey("userId")]
        public virtual User user { get; set; }

        public bool Equals(LogRecord other)
        {
            if (other == null)
            {
                return false;
            }

            if (this.content == other.content && this.time == other.time
                                              && this.type == other.type && this.userId == other.userId)
            {
                return true;
            }


            return false;
        }
    }


    public class LogRecordExport
    {
        [Display(Name = "序号")]
        public long index { get; set; }

        [Display(Name = "时间")]
        [Required(ErrorMessage = "{0}必须输入")]
        public DateTime time { get; set; }

        [Display(Name = "内容")]
        [Required(ErrorMessage = "{0}必须输入")]
        [StringLength(150, ErrorMessage = "{0}的长度在{1}个字符之内")]
        public string content { get; set; }

        [Display(Name = "类型")]
        [Required(ErrorMessage = "{0}必须输入")]
        [StringLength(20, ErrorMessage = "{0}的长度在{1}个字符之内")]
        public string type { get; set; }

        
        [Display(Name = "用户")]
        public string username { get; set; }

    }
}
