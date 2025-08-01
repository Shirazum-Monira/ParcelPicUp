namespace ParcelPicUp.Models
{
    public class ContactConfig : Common
    {
        public int Id { get; set; }
        public required string FormName { get; set; }
        public required string FormEmail { get; set; }
        public required string FormEmailPassword { get; set; }
        public required string ToName { get; set; }
        public required string ToEmail { get; set; }
        public required string SMTPHost { get; set; }
        public required int SMTPPort { get; set; }

    }
}
