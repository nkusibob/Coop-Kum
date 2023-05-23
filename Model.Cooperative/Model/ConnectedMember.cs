namespace Model.Cooperative
{
    public class ConnectedMember : Person
    {
        public ConnectedMember()
        {
            CoopUser = new ApplicationUser();
        }
        public virtual ApplicationUser CoopUser { get; set; }

    }
}