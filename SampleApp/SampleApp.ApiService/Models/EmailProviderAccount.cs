namespace SampleApp.ApiService.Models
{
    public class EmailProviderAccount
    {
        public bool EnableSSL { get; set; }

        public string Hostname { get; set; }

        public int Port { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string SenderDisplayname { get; set; }

        public string SenderEmail { get; set; }
    }
}
