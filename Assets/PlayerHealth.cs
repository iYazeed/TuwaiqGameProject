using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    public Slider healthSlider;
    public GameObject gameOverUI;
    private Animator animator;

    void Start()
    {
        currentHealth = maxHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;

        animator = GetComponent<Animator>();
        gameOverUI.SetActive(false); // ‰Œ›Ì Ê«ÃÂ… «·‰Â«Ì… »«·»œ«Ì…
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
        animator.SetTrigger("Die"); //  ›⁄Ì· √‰„Ì‘‰ «·„Ê 
        gameOverUI.SetActive(true); // ⁄—÷ Ê«ÃÂ… «·‰Â«Ì…
    }

    // ÊŸ«∆› «·√“—«—
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Application.Quit(); // Ì‘ €· ›ﬁÿ ›Ì «·‹ Build
    }
}