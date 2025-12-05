using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Model.Cooperative.Model
{
    public class ApplicationUserImage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        [Required]
        public byte[] Data { get; set; }

        [ForeignKey("Id")]
        public string AspUserId { get; set; }
        public ApplicationUser AspUser { get; set; }
    }
}
