namespace Model.Cooperative
{
    public class OfflineMember : BasicMember
    {
        public OfflineMember()
        {
            Person = new Person();
            MyCoop = new Coop();
        }

        public virtual Person Person { get; set; }
        public virtual Coop MyCoop { get; set; }
    }
}