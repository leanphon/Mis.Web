﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Apps.Model.Privilege
{
    public class Module : IEquatable<Module>
    {
        public long id { get; set; }

        [Display(Name = "模块名")]
        [Required(ErrorMessage = "{0}必须填写！")]
        [StringLength(15, ErrorMessage = "{0}的长度不能大于10！")]
        public string name { get; set; }

        [Display(Name = "父模块")]
        public long? parentId { get; set; }

        [ForeignKey("parentId")]
        public virtual Module parent { get; set; }

        [Display(Name = "模块url地址")]
        [StringLength(100, ErrorMessage = "{0}的长度在{1}个字符之内")]
        public string baseUrl { get; set; }


        [Display(Name = "显示顺序")]
        [Range(minimum:0, maximum:65536,ErrorMessage = "{0}须在指定数值范围内！")]
        public int showIndex { get; set; }

        [Display(Name = "root权限")]
        public int onlyRoot { get; set; }

        public bool Equals(Module other)
        {
            if (other == null || id != other.id)
            {
                return false;

            }
            return true;
        }
    }
}