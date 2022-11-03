using MessagePack;
using System.ComponentModel.DataAnnotations.Schema;
using KeyAttribute = System.ComponentModel.DataAnnotations.KeyAttribute;

namespace API.Models
{
    public class User
    {
                
        [Key]
        [ForeignKey("Employee")]
        public int Id { get; set; }
        public string Password { get; set; }

        [ForeignKey("Role")]        
        public int RoleId { get; set; }        
        public Role Role { get; set; }        
        public Employee Employee { get; set; }
    }
}
