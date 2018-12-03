using System.Collections.Generic;

namespace Apps.Model
{
    /// <summary>
    /// 订单状态
    /// </summary>
    public enum OrderState
    {

        Finish
    }
    public enum PayState
    {
        /// <summary>
        /// 未付款
        /// </summary>
        UnPay,
        /// <summary>
        /// 付款但未结清
        /// </summary>
        Paying,
        /// <summary>
        /// 结清
        /// </summary>
        Settle
    }

    /// <summary>
    /// 订单
    /// </summary>
    public class Order
    {
        public long id;
        public string orderSerial { get; set; }
        public OrderState OrderState { get; set; }
        public PayState payState { get; set; }
        public double price { get; set; }
        public ICollection<OrderItem> itemList { get; set; }

    }
}