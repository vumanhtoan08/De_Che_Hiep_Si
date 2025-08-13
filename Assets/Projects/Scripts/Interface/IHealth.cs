public interface IHealth
{
    float MaxHealth { get; }
    float CurrentHealth { get; }

    void TakeDamage(float amount);     // Gọi khi bị tấn công
    void Heal(float amount);           // Gọi khi được hồi máu
    void Die();                        // Gọi khi máu về 0
    bool IsDead { get; }               // Kiểm tra trạng thái sống/chết
}