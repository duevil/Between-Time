using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class Button : MonoBehaviour
{
    
    public enum ButtonType
    {
        _1,
        _2,
    }
    [SerializeField]
    public ButtonType type;
    public Action<ButtonType> OnClick;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       gameObject.GetComponent<XRSimpleInteractable>().selectEntered.AddListener(_ =>
       {
           OnClick?.Invoke(type);
       });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
