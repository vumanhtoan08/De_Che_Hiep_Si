using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceHealth : MonoBehaviour, IHealth
{
    #region Health

    [Header("Health")]
    [SerializeField] private float maxHealth;
    private float currentHealth;
    private bool isDead = false;

    public float MaxHealth => maxHealth;

    public float CurrentHealth => currentHealth;

    public bool IsDead => isDead;

    public void Init()
    {
        currentHealth = maxHealth;
        isDead = false;
    }

    public void Die()
    {
        if (IsDead) return;

        isDead = true;

        PlayAnim("Dead");

        StartCoroutine(WaitForDeadAnim());

    }

    public void Heal(float amount)
    {

    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            Die();
        }
        else
            PlayAnim("Hurt");
    }

    #endregion

    [Header("Drop")]
    #region Rớt ra tài nguyên của Resource

    [Header("Drop Settings")]
    [SerializeField] private List<DropItemData> dropItems = new List<DropItemData>();

    public List<DropItemData> DropItems => dropItems;

    public void Drop()
    {
        foreach (var item in dropItems)
        {
            Vector3 spawnPos = transform.position + Random.insideUnitSphere * 0.3f;
            Instantiate(item.itemPrefab, spawnPos, Quaternion.identity);
        }
    }

    #endregion

    #region Phát Anim
    [Header("Anim")]
    [SerializeField] private Animator anim;

    public void PlayAnim(string name)
    {
        anim.SetTrigger(name);
    }

    private IEnumerator WaitForDeadAnim()
    {
        yield return MyHelper.WaitForFrames(24);

        Drop();
        Destroy(gameObject);
    }

    #endregion
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TakeDamage(34);
        }
    }
}
