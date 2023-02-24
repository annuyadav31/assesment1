namespace ClubSystemWebApp.Models.Membership
{
    public class UpdateMembershipModel
    {
            public string Type { get; set; }
            public int AccountBalance { get; set; }
            public Guid PersonId { get; set; } //foreign key
            public Guid Id { get; set; } //primary key
    }

}
