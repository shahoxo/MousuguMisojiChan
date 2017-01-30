using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UniRx;
using System.Linq;

public class AgeSelector : NetworkBehaviour {
    [SerializeField]
    AgeIncrementorButton[] buttons;
    [SyncVar]
    public int selectedAge;

    void Start()
    {
        IObservable<int> clickAsObservable = null;
        buttons.ToList().ForEach(button =>
        {
            if (clickAsObservable == null)
                clickAsObservable = button.ClickAsObservable;
            else
                clickAsObservable = clickAsObservable.Merge(button.ClickAsObservable);
        });

        clickAsObservable.Subscribe(age =>
        {
            Debug.Log("age: " + age);
        }).AddTo(this);


    }
    
}
