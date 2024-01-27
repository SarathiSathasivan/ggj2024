using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tick : MonoBehaviour {
    [SerializeField] private Sprite sprite1;
    [SerializeField] private Sprite sprite2;
    private Image _renderer;

    // Start is called before the first frame update
    private void Start() {
        _renderer = GetComponent<Image>();
        GameManager.Instance.OnTick += Toggle;
    }

    private void OnDestroy() {
        if (GameManager.Instance != null) {
            GameManager.Instance.OnTick -= Toggle;
        }
    }

    private void Toggle() {
        // Toggle between sprite1 and sprite2 every second
        _renderer.sprite = Time.time % 2 < 1 ? sprite2 : sprite1;
    }
}
