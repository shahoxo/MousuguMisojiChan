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
    IObservable<int> clickAsObservable = null;
    public IObservable<int> ClickAsObservable
    {
        get
        {
            if (clickAsObservable != null)
                return clickAsObservable;
            
            buttons.ToList().ForEach(button =>
            {
                if (clickAsObservable == null)
                    clickAsObservable = button.ClickAsObservable;
                else
                    clickAsObservable = clickAsObservable.Merge(button.ClickAsObservable);
            });

            return clickAsObservable;
        }
    }
    
}
