using System.Collections.Generic;
using UnityEngine;

public class NumpadController : MonoBehaviour
{
    private List<int> input = new();
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (var b in gameObject.GetComponentsInChildren<Button>())
        {
            b.OnClick = type => Input((int)type);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Input(int i)
    {
        input.Add(i);
    }
}
