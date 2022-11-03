namespace API.Models
{
    public class Role
    {
        public Role(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public Role()
        {
        }


        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<User> Users { get; set; }

    }
}
