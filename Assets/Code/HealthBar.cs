using UnityEngine;
using UnityEngine.UI;
public class HealthBar : MonoBehaviour
{
    public Image healthBarImage;
    public Health playerHealth;

    void Start()
    {
        playerHealth = GameObject.Find("Player").GetComponent<Health>();    
    }

    public void UpdateHealthBar()
    {
        healthBarImage.fillAmount = playerHealth.health/ playerHealth.maxHealth;
    }
}