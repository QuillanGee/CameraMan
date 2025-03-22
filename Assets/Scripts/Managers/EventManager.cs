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
        DontDestroyOnLoad(gameObject);
    }

    public event Action OnInstantiateGamePlay;

    public void InstantiateGamePlay()
    {
        if (OnInstantiateGamePlay != null)
        {
            OnInstantiateGamePlay();
        }
    }
    
    public event EventHandler<bool> OnPauseGamePlay;

    public void PauseGamePlay(bool isPaused)
    {
        if (OnPauseGamePlay != null)
        {
            OnPauseGamePlay(this, isPaused);
        }
    }

    public event Action OnResetAlan2D;
    public void ResetAlan2D()
    {
        if (OnResetAlan2D != null)
        {
            OnResetAlan2D();
        }
    }
    
    public event Action OnResetAlan;
    public void ResetAlan()
    {
        if (OnResetAlan != null)
        {
            OnResetAlan();
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
    
    public event EventHandler<GameObject> OnUnlockDoor;

    public void UnlockDoor(GameObject door)
    {
        if (OnUnlockDoor != null)
        {
            OnUnlockDoor(this, door);
        }
    }
    
    public event EventHandler<GameObject> OnCloseDoor;

    public void CloseDoor(GameObject door)
    {
        if (OnCloseDoor != null)
        {
            OnCloseDoor(this, door);
        }
    }

    public event Action OnPostToggleFirstPerson;
    public void PostToggleFirstPerson()
    {
        if (OnPostToggleFirstPerson != null)
        {
            OnPostToggleFirstPerson();
        }
    }
    
    public event Action OnPostToggleTwoD;
    public void PostToggleTwoD()
    {
        if (OnPostToggleTwoD != null)
        {
            OnPostToggleTwoD();
        }
    }
    
    public event Action OnLoadScene;

    public void LoadScene()
    {
        if (OnLoadScene != null)
        {
            OnLoadScene();
        }
    }
    
    public event Action OnPauseMenu;

    public void PauseMenu()
    {
        if (OnPauseMenu != null)
        {
            OnPauseMenu();
        }
    }
    
    
    public event EventHandler<ZAxisEventArgs> OnSendZAxis;

    public void SendZAxis(float zAxis, bool isOnPlatform)
    {
        if (OnSendZAxis != null)
        {
            OnSendZAxis(this,new (zAxis,isOnPlatform));
        }
    }
}

public class ZAxisEventArgs : EventArgs
{
    public float ZAxis { get; }
    public bool IsOnPlatform { get; }

    public ZAxisEventArgs(float zAxis, bool isOnPlatform)
    {
        ZAxis = zAxis;
        IsOnPlatform = isOnPlatform;
    }
}
