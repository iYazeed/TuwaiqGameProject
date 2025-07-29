using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    public Slider healthSlider; // «”Õ» «·⁄‰’— „‰ Canvas Â‰«

    void Start()
    {
        currentHealth = maxHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        healthSlider.value = currentHealth;
        Debug.Log("Player Health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player died!");
        // Â‰«  ﬁœ—  ÷Ì› √‰„Ì‘‰ «·„Ê  √Ê ≈⁄«œ…  ‘€Ì· «·„‘Âœ
    }
}