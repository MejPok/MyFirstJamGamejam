using System;
using UnityEngine;

public class NutrientBase : MonoBehaviour
{
    public float MaxNutrientAmount;
    public float nutrientAmount;
    public float nutrientAmountFlower;

    VineRibbon vine;

    void Start()
    {
        vine = GetComponent<VineRibbon>();
    }

    bool notifiedUi = false;
    void Update()
    {
        // Calculate available space for the nutrient bonus
        float baseValue = MaxNutrientAmount - vine.TotalDistance;
        nutrientAmountFlower = Mathf.Min(nutrientAmountFlower, MaxNutrientAmount - baseValue);
        
        nutrientAmount = Mathf.Clamp(MaxNutrientAmount - vine.TotalDistance + nutrientAmountFlower, 0, MaxNutrientAmount);

        if (!notifiedUi)
        {
            NutrientControl.instance.NewNutrientBase(this);
            notifiedUi = true;
        }
    }

    public void AddNutrients(float amount)
    {
        nutrientAmountFlower += amount;
        // Cap the bonus so the total never exceeds max
        float baseValue = MaxNutrientAmount - vine.TotalDistance;
        nutrientAmountFlower = Mathf.Min(nutrientAmountFlower, MaxNutrientAmount - baseValue);
    }



    

}
