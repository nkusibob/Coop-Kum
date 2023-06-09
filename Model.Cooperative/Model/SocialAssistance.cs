using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Model.Cooperative.Model
{
    public class SocialAssistance
    {
        [Key]
        public int socialAssistId { get; set; }

        public virtual Membre Member { get; set; }
        public decimal Amount { get; set; }
        public bool IsValidated { get; set; }
        public bool IsRepayable { get; set; }
        public bool IsRepaid { get; set; }
        public DateTime DateReceived { get; set; }
        public DateTime DateValidated { get; set; }
        public void MarkAsRepaid()
        {
            if (IsRepayable)
            {
                IsRepaid = true;
            }
            Member.FeesPerYear += Amount; 
            DateReceived = DateTime.Now;
        }
        public void MarkIsValidated()
        {
            if (IsValidated)
            {
                IsValidated = true;
            }
            DateValidated = DateTime.Now; // 
        }
    }
}
