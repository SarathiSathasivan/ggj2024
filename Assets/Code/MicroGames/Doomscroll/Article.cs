using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Article : MonoBehaviour {
    [SerializeField] private TMP_Text body;

    public void Initialize(string bodyText) {
        body.text = bodyText;
    }
}
