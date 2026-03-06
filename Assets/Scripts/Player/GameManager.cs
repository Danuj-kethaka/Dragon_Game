using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public float worldSpeed;
    public int killCount;
    public int maxKills = 100;
    
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

    public void addkill()
    {
        killCount++;
        BackgroundController.Instance.UpdatekillSlider(killCount,maxKills);
    }
}
