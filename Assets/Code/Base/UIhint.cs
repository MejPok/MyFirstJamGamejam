using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIhint : MonoBehaviour
{
    public TextMeshProUGUI hintText;

    public static UIhint instance;
    public bool showHint;

    public List<ChangeRoot> bases = new List<ChangeRoot>();

    void Awake()
    {
        instance = this;
    }

    public void AddToBases(ChangeRoot baseRoot)
    {
        bases.Add(baseRoot);
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

        hintText.text = "Press E to replant";
        hintText.enabled = showHint;
        Debug.Log(showHint);
        
    }
}
