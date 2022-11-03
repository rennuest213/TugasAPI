using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace API.Models
{
    public class Department
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }


        //1 Department memiliki banyak division
        public int DivisionId { get; set; }

        [ForeignKey("DivisionId")]
        [JsonIgnore]
        public Division? Divisions { get; set; }
    }
}
