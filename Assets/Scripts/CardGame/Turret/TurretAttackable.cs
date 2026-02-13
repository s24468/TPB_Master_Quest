using UnityEngine;

public class TurretAttackable : AttackableBase
{
    [SerializeField] private int hp = 10;

    public override void ReceiveAttack(string attackerId)
    {
        hp -= 1;
        Debug.Log($"Turret {name} hit by {attackerId}. HP now: {hp}");

        if (hp <= 0)
        {
            Debug.Log($"Turret {name} destroyed!");
            Destroy(gameObject);
        }
    }
}