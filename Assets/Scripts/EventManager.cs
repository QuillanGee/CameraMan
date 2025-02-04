using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    public static EventManager instance;

    private void Awake()
    {
        instance = this;
    }

    public event Action OnInstantiateGamePlay;

    public void InstantiateGamePlay()
    {
        if (OnInstantiateGamePlay != null)
        {
            OnInstantiateGamePlay();
        }
    }
    
    public event Action OnToggleFirstPerson;

    public void ToggleFirstPerson()
    {
        if (OnToggleFirstPerson != null)
        {
            OnToggleFirstPerson();
        }
    }
    
    public event Action OnToggleTwoD;
    
    public void ToggleTwoD()
    {
        if (OnToggleTwoD != null)
        {
            OnToggleTwoD();
        }
    }
    
    public event Action OnHoldingBlock;

    public void HoldingBlock()
    {
        if (OnHoldingBlock != null)
        {
            OnHoldingBlock();
        }
    }
    
    public event Action OnNotHoldingBlock;

    public void NotHoldingBlock()
    {
        if (OnNotHoldingBlock != null)
        {
            OnNotHoldingBlock();
        }
    }
}
