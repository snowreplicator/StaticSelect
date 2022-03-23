using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ru.zorro.static_select
{
    [Table("DatatypeTest", Schema = "multi_d_cases")]
    public partial class DatatypeTest
    {
        [Key]
        [Column("datatypetest_id")]
        public int DatatypetestId { get; set; }
        [Required]
        [Column("datatype_string")]
        public string DatatypeString { get; set; }
        [Column("datatype_int")]
        public int DatatypeInt { get; set; }
        [Column("datatype_double")]
        public double DatatypeDouble { get; set; }
        [Column("datatype_bool")]
        public bool DatatypeBool { get; set; }
        [Column("datatype_date")]
        public DateTime DatatypeDate { get; set; }
    }
}
