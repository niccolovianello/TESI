using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private float maxHealth;
    private float currentHealth;
    
    private void Start()
    {
        currentHealth = maxHealth;
        slider.maxValue = maxHealth;
    }

    private void Update()
    {
        slider.value = currentHealth;
    }

    public void DecreaseHealth(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Debug.Log("Salute finita.");
            PlayerDeath();
            // GameManager unloada la scena e torna a GameMain 
        }
    }

    public void PlayerDeath()
    {
        UI_EnemyFight uiFight = FindObjectOfType<UI_EnemyFight>();
        uiFight.PlayerDefeatedByDemon();
    }
}
