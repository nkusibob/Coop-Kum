using System;
using System.ComponentModel.DataAnnotations;

namespace Model.Cooperative.Model
{
    public class SocialAssistance
    {
        [Key]
        public int SocialAssistId { get; set; }   // PK

        // FK
        public int MembreId { get; set; }

        public Membre Membre { get; set; } = null!;

        public decimal Amount { get; set; }

        public bool IsValidated { get; private set; }
        public bool IsRepayable { get; set; }
        public bool IsRepaid { get; private set; }

        public DateTime DateReceived { get; private set; }
        public DateTime? DateValidated { get; private set; }
        public DateTime? DateRepaid { get; private set; }

        /* ================= BUSINESS METHODS ================= */

        public void MarkAsValidated()
        {
            if (IsValidated)
                return;

            IsValidated = true;
            DateValidated = DateTime.UtcNow;
        }

        public void MarkAsRepaid()
        {
            if (!IsRepayable || IsRepaid)
                return;

            IsRepaid = true;
            DateRepaid = DateTime.UtcNow;
        }
    }
}
