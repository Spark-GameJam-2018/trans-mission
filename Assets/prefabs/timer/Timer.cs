using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * Author: Frj
 *
 * See #triggerWin() method below.
 */
public class Timer : MonoBehaviour {

    public Text timeText;
    public Text winnerText;

    private float time;
    private bool win;

    // Use this for initialization
    void Start() {
        time = 0.0F;
        win = false;
        updateTimeText();
        winnerText.text = "";
    }
    
    // Update is called once per frame
    void Update() {
        time += Time.deltaTime;
        updateTimeText();
    }

    void updateTimeText() {
        if (!win) {
            // Round time to nearest hundredth
            timeText.text = Math.Round(time, 2).ToString() + " s";
        }
    }

    /**
     * This method should be called from another GameObject
     * once win criteria has been met.
     */
    void triggerWin() {
        win = true;
        winnerText.text = "YOU WIN! Time: " + timeText.text + " seconds.";
    }
}
