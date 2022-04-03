namespace Calendar.AgendaScheduler.WebApi.Configuration
{
    public class MongoConfig
    {
        public string DatabaseName { get; set; }
        public string Url { get; set; }
        public int ConnectTimeoutSeconds { get; set; } = 5;
        public Credentials? Credential { get; set; }

        public class Credentials
        {
            public bool Anonymous { get; set; }
            public string Database { get; set; }
            public string User { get; set; }
            public string Password { get; set; }
        }
    }
}