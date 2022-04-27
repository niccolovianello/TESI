using UnityEngine;
using UnityEngine.UI;

public class ManaManager : MonoBehaviour
{
    private Slider _slider;

    // Start is called before the first frame update
    public void Start()
    {
        _slider = GameObject.Find("/LevelWindowsGUI/ManaBar").GetComponent<Slider>();
    }

    public void SetMana(float mana)
    {
        if(_slider) _slider.value = mana;
    }

    public void SetMaxMana(float maxMana)
    {
        if(_slider) _slider.maxValue = maxMana;
    }
}
