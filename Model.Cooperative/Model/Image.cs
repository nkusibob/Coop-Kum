using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Model.Cooperative
{
    public class Image
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ImageId { get; set; }

        [Required]
        public byte[] Data { get; set; }

        [ForeignKey("LivestockId")]
        public int LivestockId { get; set; }
        public virtual Livestock Livestock { get; set; }
    }
}
