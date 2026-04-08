using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundController : MonoBehaviour
{
    public static BackgroundController Instance;
    [SerializeField]private Slider energySlider;
    [SerializeField]private TMP_Text energyText;
     [SerializeField]private Slider healthSlider;
    [SerializeField]private TMP_Text healthText;
    [SerializeField] private Slider killSlider;
    [SerializeField] private TMP_Text killText;

    void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    // Update UI elements like health, energy and killcount
    public void UpdateEnergySlider(float current,float max)
    {
        energySlider.maxValue = max;
        energySlider.value = Mathf.RoundToInt(current);
        energyText.text =  energySlider.value + "/" + energySlider.maxValue;
    }

    public void UpdateHealthSlider(float current,float max)
    {
        healthSlider.maxValue = max;
        healthSlider.value = Mathf.RoundToInt(current);
        healthText.text =  healthSlider.value + "/" + healthSlider.maxValue;
    }

    public void UpdatekillSlider(int current,int max)
    {
        killSlider.maxValue = max;
        killSlider.value = current;
        killText.text = current + "/" +max;
    }
}
