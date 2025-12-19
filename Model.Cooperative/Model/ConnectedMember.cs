namespace Model.Cooperative
{
    public class ConnectedMember : Person
    {
        public ConnectedMember()
        {
            CoopUser = new ApplicationUser();
        }
        public string ? CoopUserId { get; set; }
        public virtual ApplicationUser? CoopUser { get; set; }

    }
}