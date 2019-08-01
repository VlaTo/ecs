namespace ClassLibrary1
{
    public interface IEntityCreator
    {
        EntityBase Instantiate(EntityState state);
    }
}