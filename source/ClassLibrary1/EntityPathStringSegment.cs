namespace ClassLibrary1
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class EntityPathStringSegment
    {
        /// <summary>
        /// 
        /// </summary>
        public EntityPathStringSegment Next
        {
            get;
        }

        protected EntityPathStringSegment(EntityPathStringSegment next)
        {
            Next = next;
        }
    }
}