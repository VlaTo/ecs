namespace ClassLibrary1.Core.Path
{
    /// <summary>
    /// Implements entity path match processor.
    /// </summary>
    /// <remarks>
    /// <list type="table">
    ///     <listheader>
    ///         <term>path</term>
    ///         <description>meaning</description>
    ///     </listheader>
    ///     <item>
    ///         <term>
    ///             <c>/*</c>
    ///         </term>
    ///         <description>all entities, staring from root.</description>
    ///     </item>
    ///     <item>
    ///         <term>
    ///             <c>/entity1</c>
    ///         </term>
    ///         <description>
    ///         all entities with key 'entity1', staring from root.
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <term>
    ///             <c>/entity1/entity2</c>
    ///         </term>
    ///         <description>
    ///         all entities with key 'entity2' wich is child of entity 'entity1', staring from root.
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <term>
    ///             <c>/entity1/*</c>
    ///         </term>
    ///         <description>
    ///             all entity with any key wich is child of entity 'entity1', staring from root.
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <term>
    ///             <c>*</c>
    ///         </term>
    ///         <description>
    ///             all entities which is child of current entity.
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <term>
    ///             <c>entity1</c>
    ///         </term>
    ///         <description>
    ///             all child entities with key 'entity1' from current.
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <term>
    ///             <c>entity1/entity2</c>
    ///         </term>
    ///         <description>
    ///             all child entities with key 'entity1' from current.
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <term>
    ///             <c>entity1/*</c>
    ///         </term>
    ///         <description>
    ///             all child entities with key 'entity1' from current.
    ///         </description>
    ///     </item>
    /// </list>
    /// </remarks>
    internal sealed class EntityPathMatch : ICondition<EntityBase>
    {
        public EntityPath Path
        {
            get;
        }

        public EntityPathMatch(EntityPath path)
        {
            Path = path;
        }

        public bool IsMet(EntityBase value) => Path.Equals(value.Path);
    }
}