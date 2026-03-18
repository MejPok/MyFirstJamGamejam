using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIhint : MonoBehaviour
{
    public TextMeshProUGUI hintText;

    public static UIhint instance;
    public bool showHint;

    public List<ChangeRoot> bases = new List<ChangeRoot>();
    public List<Flower> flowers = new List<Flower>();


    void Awake()
    {
        instance = this;
    }

    public void AddToBases(ChangeRoot baseRoot)
    {
        bases.Add(baseRoot);
    }
    public void AddToFlowers(Flower flower)
    {
        flowers.Add(flower);
    }
    public void SetHint(string hint)
    {
        hintText.text = hint;
    }

    void Update()
    {
        showHint = false;
        for(int i = 0; i < bases.Count; i++)
        {
            if(bases[i] == null)
            {
                bases.RemoveAt(i);
                continue;
            }

            if(bases[i].PlayerInside){
                showHint = true;
            }


        }

        for(int i = 0; i < flowers.Count; i++)
        {
            if(flowers[i] == null)
            {
                flowers.RemoveAt(i);
                continue;
            }

            if(flowers[i].isPlayerInside){
                showHint = true;
            }


        }
        hintText.enabled = showHint;
        Debug.Log(showHint);
        
    }
}
