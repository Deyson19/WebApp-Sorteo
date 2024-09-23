namespace WebApp_Sorteo.Models
{
    public class PaymentResponse
    {
        public required string collection_id { get; set; }
        public required string collection_status { get; set; }
        public required string payment_id { get; set; }
        public required string status { get; set; }
        public required string external_reference { get; set; }
        public required string payment_type { get; set; }
        public required string merchant_order_id { get; set; }
        public required string preference_id { get; set; }
        public required string site_id { get; set; }
        public required string processing_mode { get; set; }
        public required string merchant_account_id { get; set; }
    }
}
