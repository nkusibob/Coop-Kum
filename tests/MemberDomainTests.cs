using Model.Cooperative;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Tests.Domain
{
    public class MembreDomainTests
    {
        [Fact]
        public void Create_NegativeFees_Throws()
        {
            var coop = new Coop { CoopName = "C1" };
            var person = new ConnectedMember(); // minimal ici

            Assert.Throws<ArgumentOutOfRangeException>(() =>
                Membre.Create(person, coop, -1m));
        }

        [Fact]
        public void DefaultConstructor_IsNotPublic()
        {
            // Empêche de réintroduire un public Membre() plus tard
            var ctor = typeof(Membre).GetConstructor(Type.EmptyTypes);
            Assert.True(ctor == null || !ctor.IsPublic);
        }
    }
}
