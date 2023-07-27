using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Model.Cooperative
{
    public class StepProjectPicture
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public byte[] Data { get; set; }
        [ForeignKey("StepProjectId")]

        public int StepProjectId { get; set; }
        public virtual StepProject StepProject { get; set; }

    }
}
