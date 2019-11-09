namespace LibraProgramming.Ecs
{
    public interface IEntityCreator
    {
        EntityBase Instantiate(EntityState state);
    }
}