using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ru.zorro.static_select
{
    [Table("classifier", Schema = "classifiers")]
    [Index(nameof(ClassifiersetId), nameof(Value), Name = "index_classifier_classifierset_id_value", IsUnique = true)]
    public partial class Classifier
    {
        [Key]
        [Column("classifier_id")]
        public int ClassifierId { get; set; }
        [Column("classifierset_id")]
        public int ClassifiersetId { get; set; }
        [Required]
        [Column("value")]
        public string Value { get; set; }
        [Column("deleted")]
        public bool Deleted { get; set; }
    }
}
