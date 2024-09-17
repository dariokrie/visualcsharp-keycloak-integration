namespace Showcasing.Keycloak.Configuration
{
    public class KeycloakSettings
    {
        public string Authority { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string ResponseType { get; set; }
        public bool SaveTokens { get; set; }
        public string Scope { get; set; }
        public string CallbackPath { get; set; }
        public string SignedOutCallbackPath { get; set; }
    }
}
