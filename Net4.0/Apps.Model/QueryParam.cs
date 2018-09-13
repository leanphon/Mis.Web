using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Model
{
    public class FilterModel
    {
        public string key { get; set; }//过滤条件中使用的数据列
        public string action { get; set; }//过滤条件中的操作:==、!=等
        public string logic { get; set; }//过滤条件之间的逻辑关系：AND和OR
        public string value { get; set; }//过滤条件中的操作的值
        public string dataType { get; set; }//过滤条件中的操作的字段的类型
    }

    /// <summary>
    /// 封装查询参数
    /// 
    /// </summary>
    public class QueryParam
    {
        public Pager pager { get; set; }
        //public List<FilterModel> filterList { get; set; }
        public Dictionary<string, FilterModel> filters { get; set; }
    }

    public class Pager
    {
        public int page { get; set; }
        public int rows { get; set; }
    }
}
