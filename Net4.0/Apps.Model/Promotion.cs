using System;

namespace Apps.Model
{
    /// <summary>
    /// 优惠活动
    /// </summary>
    public class Promotion
    {
        public int id { get; set; }
        public double price { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public DateTime beginTime { get; set; }
        public DateTime endTime { get; set; }
    }
}