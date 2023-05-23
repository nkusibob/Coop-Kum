namespace Model.Cooperative
{
    public class Membre : BasicMember
    {
        public Membre()
        {
            Person = new ConnectedMember();
            MyCoop = new Coop();
        }

        public virtual ConnectedMember Person { get; set; }
        public virtual Coop MyCoop { get; set; }
        public string GrandparentTag { get; set; }
        public string Country { get; set; }

        public string City { get; set; }
        public string Town { get; set; }


    }
}