using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DBLibrary.EF.Models
{
    [Table("tblTestEntities")]
    public class TestEntity
    {
        [Key]
        public int Id { get; set; }
        public int Age { get; set; }

        [Required, StringLength(maximumLength: 255)]
        public string Name { get; set; }
        public bool Male { get; set; }
    }
}
