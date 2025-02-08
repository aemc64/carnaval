using UnityEngine;

public class PlayerActionController : ActionController
{
    private LayerMask _enemyLayer;

    protected override void Awake()
    {
        base.Awake();

        _enemyLayer = 1 << LayerMask.NameToLayer("Enemy");
    }

    protected override ActionController GetTarget()
    {
        var movementDirection = GameUtils.Directions[CurrentDirection];
        
        var hit = Physics2D.Raycast(transform.position, movementDirection, GameUtils.TileSize, _enemyLayer.value);
        if (hit.collider != null)
        {
            if (hit.collider.TryGetComponent(out ActionController enemyActionController))
            {
                return enemyActionController;
            }
        }

        return null;
    }

    protected override bool CanMove(bool onBeat)
    {
        return base.CanMove(onBeat) && onBeat;
    }
}
