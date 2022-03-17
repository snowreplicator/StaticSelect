using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ru.zorro.static_select
{
    [Table("LessonSource", Schema = "multi_d_cases")]
    public partial class LessonSource
    {
        [Key]
        [Column("LessonSource_id")]
        public int LessonSourceId { get; set; }
        [Required]
        [Column("code")]
        public string Code { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
