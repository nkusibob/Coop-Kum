using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Cooperative.Interfaces
{
    public interface ILivestockObserver
    {
        string NotifyLivestockBirth(string motherName, string offspringName, string offspringGender, DateTime birthDate);
        string NotifyLivestockDeath(DateTime birthDate, string name, string gender, DateTime deathDate);
        string NotifyLivestockSold(DateTime birthDate, string name, string gender, decimal price);
    }
}
