namespace SWZRFI_Utils.EmailHelper.Models
{
    public class EmailCredentials
    {
        public string EmailAddress { get; set; }
        public string SmtpHost { get; set; }
        public int Port { get; set; }
        public string Password { get; set; }
    }
}
