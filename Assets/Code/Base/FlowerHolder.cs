using UnityEngine;

public class FlowerHolder : MonoBehaviour
{
    public GameObject[] flowerPositions;

    public GameObject[] flowers;

    public float flowerAmount = 40f;

    

    public void ResetFlowers()
    {
        if(flowerPositions != null)
        {
            flowers = new GameObject[flowerPositions.Length];
            DestroyFlowers();
            CreateFlowers();    
        }
        

        
    }

    void DestroyFlowers()
    {
        for(int i = 0; i < flowers.Length; i++)
        {
            Destroy(flowers[i]);
        }
    }
    void CreateFlowers()
    {
        for(int i = 0; i < flowerPositions.Length; i++)
        {
            var flower = Instantiate(WorldControl.instance.flowerPrefab, flowerPositions[i].transform.position, Quaternion.identity);
            flower.GetComponent<Flower>().NutrientAmount = (int)flowerAmount;
            flowers[i] = flower;

        }
    }
}
