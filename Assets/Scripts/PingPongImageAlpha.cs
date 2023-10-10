using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PingPongImageAlpha : MonoBehaviour {
    private Graphic graphic;
    [SerializeField] private float speed = 1f;

    private void Awake() {
        graphic = GetComponent<Graphic>();
    }

    private void Update() {
        PingPongAlpha();
    }
    
    private void PingPongAlpha() {
        Color color = graphic.color;
        color.a = Mathf.PingPong(Time.time * speed, 1.0f);
        graphic.color = color;
    }
}
