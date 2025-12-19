using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Model.Cooperative;
using Model.Cooperative.Model;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

public class SocialAssistanceSqlTests
{
    [Fact]
    //public async Task Cannot_insert_two_non_repaid_social_assistances_for_same_member()
    //{
    //    var dbName = SqlServerTestDb.NewDbName();
    //    await SqlServerTestDb.CreateDatabaseAsync(dbName);

    //    try
    //    {
    //        var coopOptions = SqlServerTestDb.CoopOptions(dbName);
    //        var identityOptions = SqlServerTestDb.IdentityOptions(dbName);

    //        await using var ctx = new CooperativeContext(coopOptions);
    //        await using var idCtx = new ApplicationDbContext(identityOptions);


    //        await ctx.Database.EnsureDeletedAsync();

    //        // 1) Create DB + Identity tables
    //        await idCtx.Database.EnsureCreatedAsync();

    //        // 2) Create ONLY the Cooperative tables in the same DB
    //        var creator = (IRelationalDatabaseCreator)ctx.Database.GetService<IDatabaseCreator>();
    //        await creator.CreateTablesAsync();

    //        // 1) Create Coop
    //        var testCoop = new Coop { CoopName = "C1", Budget = 0m };
    //        ctx.Coop.Add(testCoop);
    //        await ctx.SaveChangesAsync();

    //        var pers = new Person
    //        {
    //            FirstName = "A",
    //            LastName = "B",
    //            City = "X",
    //            Country = "Y",
    //            PhoneNumber = "1",
    //            PersonImageUrl = "" // if NOT NULL in DB
               
    //        };
    //        // Identity tables too
    //        // 2) Create ApplicationUser (MUST satisfy non-null constraints like City)
    //        var user = new ApplicationUser
    //        {
    //            Id = Guid.NewGuid().ToString(),
    //            UserName = "membre@test.be",
    //            FirstName = "A",
    //            LastName = "B",
    //            IdNumber = "123456789",
    //            NormalizedUserName = "MEMBRE@TEST.BE",
    //            Email = "membre@test.be",
    //            NormalizedEmail = "MEMBRE@TEST.BE",
    //            City = "X",
                 
    //            Country = "Y",
    //            Fees = 10
    //        };

    //        idCtx.Users.Add(user);
    //        await idCtx.SaveChangesAsync();
      

          

    //        // 3) Create ConnectedMember linked to ApplicationUser (FK Person.CoopUserId)
    //        var connected = new ConnectedMember
    //        {
    //            FirstName = "A",
    //            LastName = "B",
    //            City = "X",
    //            Country = "Y",
    //            PhoneNumber = "1",
    //            PersonImageUrl = "", // if NOT NULL in DB
    //            CoopUser = user
    //        };

    //        // 4) Create Membre (this must insert into dbo.Membre)
    //        var membre = new Membre
    //        {
    //            MyCoop = testCoop,
    //            Person = connected,
    //            FeesPerYear = 10m,
    //            City = "X",
    //            Country = "Y",
    //            Town = "T",
    //            GrandparentTag = "GP1",
    //            PhoneNumber = "1"


    //        };

    //        ctx.Membre.Add(membre);
    //        await ctx.SaveChangesAsync();

    //        Assert.True(membre.MembreId > 0);

    //        // 5) First assistance OK
    //        var a1 = new SocialAssistance(membre.MembreId, 50m, true);
    //        a1.MarkAsValidated();
    //        ctx.SocialAssistances.Add(a1);
    //        await ctx.SaveChangesAsync();

    //        // 6) Second active assistance must fail (filtered unique index)
    //        var a2 = new SocialAssistance(membre.MembreId, 25m, true);
    //        a2.MarkAsValidated();
    //        ctx.SocialAssistances.Add(a2);

    //        await Assert.ThrowsAsync<DbUpdateException>(() => ctx.SaveChangesAsync());
    //    }
    //    catch (Exception ex)
    //    {
    //        Console.WriteLine(ex.ToString());
    //        throw;
    //    }
    //    finally
    //    {
    //        await SqlServerTestDb.DropDatabaseAsync(dbName);
    //    }
    //}



}
