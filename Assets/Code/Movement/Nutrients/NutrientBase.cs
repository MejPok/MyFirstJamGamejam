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

    bool notifiedUi = false;
    void Update()
    {
        nutrientAmount = MaxNutrientAmount - vine.TotalDistance;
        if (!notifiedUi)
        {
            NutrientControl.instance.NewNutrientBase(this);
            notifiedUi = true;
        }
    }

}
