using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundController : MonoBehaviour
{
    public static BackgroundController Instance;
    [SerializeField]private Slider energySlider;
    [SerializeField]private TMP_Text energyText;

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

    public void UpdateEnergySlider(float current,float max)
    {
        energySlider.value = Mathf.RoundToInt(current);
        energySlider.maxValue = max;
        energyText.text =  energySlider.value + "/" + energySlider.maxValue;
    }
}
