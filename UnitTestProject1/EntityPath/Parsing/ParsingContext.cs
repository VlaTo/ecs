using EntityPath1 = LibraProgramming.Ecs.Core.Path.EntityPath;

namespace UnitTestProject1.EntityPath.Parsing
{
    public abstract class ParsingContext : MethodContextBase
    {
        protected EntityPath1 EntityPath
        {
            get; 
            set;
        } 

        public override void Arrange()
        {
            base.Arrange();
        }

        public override void Act()
        {
            base.Act();
        }

        public override void Cleanup()
        {
            base.Cleanup();
        }
    }
}