using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonManager : MonoBehaviour
{

    [Header("Includes")]
    [Space(1)]
    public static SingletonManager Instance;
    public BoxController boxController;
    public UIManager uiManager;

    private void Awake()
    {
        Instance = this;
    }
}
