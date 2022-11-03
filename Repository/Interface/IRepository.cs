namespace API.Repository.Interface
{
    public interface IRepository<Entity,Key> where Entity : class
    {
        public IEnumerable<Entity> Get();
        public Entity GetById(Key id);
        public int Update(Entity entity);
        public int Delete(Key entity);
        
        public int Create(Entity entity);
    }
}
