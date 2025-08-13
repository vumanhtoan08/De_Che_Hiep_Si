using UnityEngine;

public abstract class Unit : MonoBehaviour, IHealth
{
    #region Health
    [SerializeField] protected float maxHealth;
    protected float currentHealth;
    protected bool isDead;

    public float MaxHealth => maxHealth;

    public float CurrentHealth => currentHealth;
    public bool IsDead => isDead;
    public virtual void TakeDamage(float amount)
    {
    }

    public virtual void Heal(float amount)
    {
    }

    public virtual void Die()
    {
    }

    #endregion

    #region Behaviours
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float attackDamage;

    public virtual void Move(Vector2 target)
    {
        transform.position = Vector2.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
    }

    public virtual void Attack(Unit target)
    {

    }
    #endregion
}

public enum UNIT_TYPE
{
    SOLIDIER, 
    GOBLIN
}