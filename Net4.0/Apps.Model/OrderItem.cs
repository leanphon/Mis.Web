using System.Collections.Generic;
namespace Apps.Model
{
    /// <summary>
    /// 订单条目
    /// </summary>
    public class OrderItem
    {
        public long id { get; set; }
        public double price { get; set; }

        public long orderId { get; set; }
        public virtual Order ownerOrder { get; set; }
        public long productId { get; set; }
        public virtual Product product { get; set; }
        public virtual ICollection<Promotion> promotionList { get; set; }

    }
}