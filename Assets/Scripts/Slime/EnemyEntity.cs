using UnityEngine;

public class EnemyEntity : MonoBehaviour
{
   [Header("Здоровье сущности")]
   [SerializeField] private int maxHealth = 5;
   private int _currentHealth;


   private void Start()
   {
      _currentHealth = maxHealth;
   }

   public void TakeDamage(int damage)
   {
      _currentHealth -= damage;

      DetectDeath();
   }
   
   public void DetectDeath()
   {
      if (_currentHealth <= 0)
      {
         Destroy(gameObject);
      }
   }
}
