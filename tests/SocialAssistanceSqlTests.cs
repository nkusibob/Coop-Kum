using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Model.Cooperative;
using Model.Cooperative.Model;
using Xunit;

public class SocialAssistanceSqlTests
{
    [Fact]

    
    public async Task Cannot_insert_two_non_repaid_social_assistances_for_same_member()
    {
        var dbName = SqlServerTestDb.NewDbName();
        await SqlServerTestDb.CreateDatabaseAsync(dbName);

        try
        {
            var options = SqlServerTestDb.CoopOptions(dbName);
            await using var ctx = new CooperativeContext(options);

            await ctx.Database.EnsureDeletedAsync();
            await ctx.Database.EnsureCreatedAsync();

            // Coop (required for MyCoop)
            var coop = new Coop { CoopName = "C1", Budget = 0m };
            ctx.Coop.Add(coop);
            await ctx.SaveChangesAsync();

            // Person (MUST satisfy NOT NULL cols in dbo.Person)
            var person = new Person
            {
                FirstName = "A",
                LastName = "B",
                City = "X",
                Country = "Y",
                PhoneNumber = "1",
                PersonImageUrl = "" // IMPORTANT: if column is NOT NULL in DB
            };
            ctx.Person.Add(person);
            await ctx.SaveChangesAsync();

            // OfflineMember
            var offline = new OfflineMember
            {
                FeesPerYear = 10m,
                MyCoop = coop,
                Person = person
            };
            ctx.OfflineMember.Add(offline);
            await ctx.SaveChangesAsync();

            // First active assistance OK
            var a1 = new SocialAssistance(offline.MembreId, 50m, true);
            a1.MarkAsValidated();
            ctx.SocialAssistances.Add(a1);
            await ctx.SaveChangesAsync();

            // Second active assistance must fail (unique filtered index)
            var a2 = new SocialAssistance(offline.MembreId, 25m, true);
            a2.MarkAsValidated();
            ctx.SocialAssistances.Add(a2);

            await Assert.ThrowsAsync<DbUpdateException>(() => ctx.SaveChangesAsync());
        }
        finally
        {
            await SqlServerTestDb.DropDatabaseAsync(dbName);
        }
    }



}
