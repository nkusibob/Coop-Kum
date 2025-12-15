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
        public int ApplicationUserImageId { get; set; }   // int identity PK

        [Required]
        public byte[] Data { get; set; } = Array.Empty<byte>();

        [Required]
        public string AspUserId { get; set; } = null!;    // FK to ApplicationUser.Id

        public ApplicationUser AspUser { get; set; } = null!;
    }

}
