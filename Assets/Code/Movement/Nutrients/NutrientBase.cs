using UnityEngine;

public class NutrientBase : MonoBehaviour
{
    public float MaxNutrientAmount;
    public float nutrientAmount;

    VineRibbon vine;

    void Start()
    {
        vine = GetComponent<VineRibbon>();
    }

    void Update()
    {
        nutrientAmount = MaxNutrientAmount - vine.TotalDistance;
    }

}
