using UnityEngine;
using UnityEngine.UI;

public class ManaManager : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private float maxMana;
    
    private float currentMana;

    public float CurrentMana
    {
        get => currentMana;
        set => currentMana = value;
    }


    // Start is called before the first frame update
    void Start()
    {
        currentMana = maxMana;
        slider.maxValue = maxMana;
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = currentMana;
    }

    public void DecreaseMana(float cost)
    {
        currentMana -= cost;
    }
    
    public void IncreaseMana(float increment)
    {
        currentMana += increment;
    }
}
