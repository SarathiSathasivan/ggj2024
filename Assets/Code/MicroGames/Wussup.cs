using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = System.Random;

public class Wussup : MicroGame {
    [SerializeField] private TMP_Text messageText;
    [SerializeField] private TMP_Text replyText;
    [SerializeField] private List<Transform> keyPositions;
    [SerializeField] private List<char> keys;
    [SerializeField] private Transform key;
    [SerializeField] private Transform sendButton;

    private char[] _response;
    private int _responseIndex;

    private readonly string[] _messageTexts = {
        "Hey, I just don't know if I can do this anymore. I'm so tired of trying to be someone I'm not. You know? " +
        "Like my stomach hurts all the time",
        "Dude the craziest thing just happened to me, I was walking down the street and this guy just came up to me " +
        "and told me I looked like Chris Evans",
        "Have you ever thought about how weird it is that we're all just floating in space on a giant rock? Like " +
        "what's the point of it all?"
    };

    private readonly string[] _responses = {
        "k", "wow", "ok", "lol", "hah", "dam", "wld"
    };

    private void Start() {
        sendButton.gameObject.SetActive(false);
        Random random = new();
        messageText.text = _messageTexts[random.Next(0, _messageTexts.Length)];
        replyText.text = "";
        _response = _responses[random.Next(0, _responses.Length)].ToCharArray();
        _responseIndex = 0;
        NextKey();
    }

    public void NextKey() {
        if (_responseIndex > 0) {
            replyText.text += _response[_responseIndex - 1];
            GameManager.Instance.PlayProgressSound();
        }
        if (_responseIndex < _response.Length) {
            key.position = keyPositions[keys.IndexOf(_response[_responseIndex++])].position;
        } else {
            key.gameObject.SetActive(false);
            sendButton.gameObject.SetActive(true);
        }
    }

    public void Send() {
        GameManager.Instance.FinishMicroGame(true);
    }
}
