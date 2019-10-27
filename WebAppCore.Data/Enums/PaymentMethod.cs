using System.ComponentModel;

namespace WebAppCore.Data.Enums
{
    public enum PaymentMethod
    {
        [Description("Thanh Toán Khi Nhận Hàng")]
        CashOnDelivery,

        [Description("Chuyển khoản")]
        OnlinBanking,

        //[Description("Payment Gateway")]
        //PaymentGateway,

        //[Description("Visa")]
        //Visa,

        //[Description("Master Card")]
        //MasterCard,

        //[Description("PayPal")]
        //PayPal,

        //[Description("Atm")]
        //Atm
    }
}