namespace ClubSystemWebApp.Models.Membership
{
    public class AddMembershipModel
    {
        public string Type { get; set; }
        public int AccountBalance { get; set; }
        public Guid PersonId { get; set; } //foreign key
    }
}
