using UnityEngine;
using UnityEngine.UI;

public class ManaManager : MonoBehaviour
{
    private Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        slider = GameObject.Find("/LevelWindowsGUI/ManaBar").GetComponent<Slider>();
    }

    public void SetMana(float mana)
    {
        slider.value = mana;
    }

    public void SetMaxMana(float maxMana)
    {
        slider.maxValue = maxMana;
    }
}
