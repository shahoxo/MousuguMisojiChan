using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class AgeIncrementorButton : MonoBehaviour
{
    public int age;
    public Button Button { get { return this.GetComponent<Button>(); } }
    IObservable<int> clickAsObservable;
    public IObservable<int> ClickAsObservable
    {
        get
        {
            if (clickAsObservable != null)
                return clickAsObservable;

            var button = this.GetComponent<Button>();
            clickAsObservable = button.OnClickAsObservable().Select(_ => this.age).Publish().RefCount();
            return clickAsObservable;
        }
    }
}
