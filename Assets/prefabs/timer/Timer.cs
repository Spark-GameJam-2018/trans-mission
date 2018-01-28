using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Author: Frj
public class Timer : MonoBehaviour {

    public Text timeText;

    private float time;

    // Use this for initialization
    void Start() {
        time = 0.0F;
        updateTimeText();
    }
    
    // Update is called once per frame
    void Update() {
        time += Time.deltaTime;
        updateTimeText();
    }

    void updateTimeText() {
        // Round time to nearest hundredth
        timeText.text = Math.Round(time, 2).ToString();
    }
}
