using System;
using System.Data;

namespace Apps.Model
{
    public enum OperateStatus
    {
        /// <summary>
        /// 成功
        /// </summary>
        Success = 0,

        /// <summary>
        /// 错误
        /// </summary>
        Error = 1,

    }

    /// <summary>
    /// 封装操作结果
    /// </summary>
    public class OperateResult
    {
        public OperateResult()
        {
            status = OperateStatus.Error;
            data = null;
        }


        /// <summary>
        /// 响应状态
        /// </summary>
        public OperateStatus status { get; set; }

        /// <summary>
        /// 信息内容
        /// </summary>
        public string content { get; set; }

        /// <summary>
        /// 辅助数据
        /// </summary>
        public object data { get; set; }
        
    }
    
}
