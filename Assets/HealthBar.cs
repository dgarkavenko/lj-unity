using UnityEngine;
using System.Collections;

public class HealthBar : MonoBehaviour {

    [SerializeField]
    RectTransform _back;
    [SerializeField]

    RectTransform _filler;

    const int BORDER_WIDHT = 2;

    void Start() {

    }

    public void SetHP(float hp) {
        float w = Mathf.Clamp(_back.sizeDelta.x * hp - BORDER_WIDHT * 2, 0, _back.sizeDelta.x);
        _filler.sizeDelta = new Vector2(w, _filler.sizeDelta.y);
    }




}
