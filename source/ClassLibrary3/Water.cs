namespace ClassLibrary3
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class Water : Context<WaterState>
    {
        public Water(WaterState state)
            : base(state)
        {
        }

        public void Heat()
        {
            State.Heat(this);
        }

        public void Cooldown()
        {
            State.Cooldown(this);
        }
    }
}