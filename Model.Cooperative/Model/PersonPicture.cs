using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Model.Cooperative
{
    public class PersonPicture
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public byte[] Data { get; set; }
        [ForeignKey("PersonId")]

        public int PersonId { get; set; }
        public virtual Person Person { get; set; }

    }
}
