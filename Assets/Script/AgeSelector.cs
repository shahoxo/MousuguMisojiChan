using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class AgeSelector : NetworkBehaviour {
    [SerializeField]
    Button[] buttons;
    [SyncVar]
    public int selectedAge;

    void Start()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].onClick.AddListener(() =>
            {
                selectedAge = i + 1;

            });
        }
    }


    
}
