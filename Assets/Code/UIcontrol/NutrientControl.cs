using UnityEngine;
using UnityEngine.UI;

public class NutrientControl : MonoBehaviour
{
    public static NutrientControl instance;
    public  NutrientBase root;
    void Start()
    {
        instance = this;
    }

    public Slider nutrients;

    void Update()
    {
        nutrients.maxValue = root.MaxNutrientAmount;
        nutrients.value = root.nutrientAmount;
        nutrients.minValue = 0;
    }

    public void NewNutrientBase(NutrientBase root)
    {
        this.root = root;
    }
}
