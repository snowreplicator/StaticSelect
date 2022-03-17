using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ru.zorro.static_select
{
    [Table("classifierset", Schema = "classifiers")]
    public partial class Classifierset
    {
        [Key]
        [Column("classifierset_id")]
        public int ClassifiersetId { get; set; }
        [Required]
        [Column("name")]
        public string Name { get; set; }
        [Required]
        [Column("classnamepk")]
        public string Classnamepk { get; set; }
    }
}
