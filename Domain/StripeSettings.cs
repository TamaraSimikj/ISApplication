namespace Domain
{
    public class StripeSettings
    {
        public StripeSettings(string publishableKey, string secretKey)
        {
            PublishableKey = publishableKey;
            SecretKey = secretKey;
        }

        public string PublishableKey { get; set; }
        public string SecretKey { get; set; }
    }
}