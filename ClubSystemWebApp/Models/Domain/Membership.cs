namespace ClubSystemWebApp.Models.Domain
{
    public class Membership
    {
        public Guid Id { get; set; } //primary key
        public string Type { get; set; }
        public int AccountBalance { get; set; }
        public Guid PersonId { get; set; } //foreign key
    }
}
