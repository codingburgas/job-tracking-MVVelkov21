namespace JobTracking.Domain.Constants
{
    public static class Jwt
    {
        public const string Issuer = "JobTrackingApi";
        public const string Audience = "JobTrackingClients";
        public const string Key = "THIS_IS_A_VERY_STRONG_SECRET_KEY_FOR_JWT_SIGNING_DONT_USE_IN_PROD";
        public const int TokenExpirationMinutes = 60;
    }
}