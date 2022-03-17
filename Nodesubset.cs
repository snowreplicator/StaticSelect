using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ru.zorro.static_select
{
    [Table("nodesubset", Schema = "classifiers")]
    [Index(nameof(ClassifierId), nameof(Parentid), Name = "index_nodesubset_classifier_id_parentid", IsUnique = true)]
    public partial class Nodesubset
    {
        [Key]
        [Column("nodesubset_id")]
        public int NodesubsetId { get; set; }
        [Column("classifier_id")]
        public int ClassifierId { get; set; }
        [Column("parentid")]
        public int Parentid { get; set; }
        [Column("level")]
        public int Level { get; set; }
    }
}
