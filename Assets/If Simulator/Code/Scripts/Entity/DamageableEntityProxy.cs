using UnityEngine;

public class DamageableEntityProxy : MonoBehaviour, IDamageable
{
    [SerializeField] private DamageableEntity _forwardTo;

    public void Damage(float damage, Color color) => _forwardTo.Damage(damage, color);

    public void Heal(float heal) => _forwardTo.Heal(heal);
}
