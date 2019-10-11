namespace LibraProgramming.Ecs.Core.Path
{
    internal sealed class SegmentToken : PathToken
    {
        public string Segment
        {
            get;
        }

        public SegmentToken(string segment)
        {
            Segment = segment;
        }
    }
}