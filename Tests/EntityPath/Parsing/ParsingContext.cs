namespace Tests.EntityPath.Parsing
{
    public abstract class ParsingContext : MethodContextBase
    {
        protected LibraProgramming.Ecs.Core.Path.EntityPath EntityPath
        {
            get; 
            set;
        }

        public override void Arrange()
        {
            EntityPath = LibraProgramming.Ecs.Core.Path.EntityPath.Empty;
        }
    }
}