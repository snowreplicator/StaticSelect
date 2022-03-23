using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ru.zorro.static_select
{
    [Table("OrganizationStructure", Schema = "org_struct")]
    public partial class OrganizationStructure
    {
        [Key]
        [Column("org_id")]
        public int OrgId { get; set; }
        [Column("parent_id")]
        public int ParentId { get; set; }
        [Required]
        [Column("code")]
        public string Code { get; set; }
        [Required]
        [Column("name")]
        public string Name { get; set; }
    }
}
