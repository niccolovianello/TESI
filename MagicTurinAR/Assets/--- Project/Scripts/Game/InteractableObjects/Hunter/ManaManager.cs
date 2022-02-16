using UnityEngine;
using UnityEngine.UI;

public class ManaManager : MonoBehaviour
{
    private Slider slider;
    private float maxMana;
    
    private float currentMana;
    

    public float MaxMana
    {
        set => maxMana = value;
    }


    // Start is called before the first frame update
    void Start()
    {
        slider = GameObject.Find("ManaBar").GetComponent<Slider>();
    }

    public void SetMana(float mana)
    {
        currentMana = mana;
        slider.value = currentMana;
    }
}
