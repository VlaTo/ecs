namespace ClassLibrary3
{
    public abstract class WaterState : IState
    {
        public abstract void Heat(Context<WaterState> context);

        public abstract void Cooldown(Context<WaterState> context);
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class SolidStateWater : WaterState
    {
        public SolidStateWater()
        {
        }

        public override void Heat(Context<WaterState> context)
        {
            context.State = new LiquidWaterState();
        }

        public override void Cooldown(Context<WaterState> context)
        {
            throw new System.NotSupportedException();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class LiquidWaterState : WaterState
    {
        public LiquidWaterState()
        {
        }

        public override void Heat(Context<WaterState> context)
        {
            context.State = new VaporizedWaterState();
        }

        public override void Cooldown(Context<WaterState> context)
        {
            context.State = new SolidStateWater();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class VaporizedWaterState : WaterState
    {
        public VaporizedWaterState()
        {
        }

        public override void Heat(Context<WaterState> context)
        {
            throw new System.NotSupportedException();
        }

        public override void Cooldown(Context<WaterState> context)
        {
            context.State = new LiquidWaterState();
        }
    }
}