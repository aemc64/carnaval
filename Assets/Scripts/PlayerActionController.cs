public class PlayerActionController : ActionController
{
    protected override ActionController GetTarget()
    {
        return null;
    }

    protected override bool CanMove(bool onBeat)
    {
        return base.CanMove(onBeat) && onBeat;
    }
}
