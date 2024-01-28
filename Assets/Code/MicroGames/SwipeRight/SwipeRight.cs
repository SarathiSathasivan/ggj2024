using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = System.Random;

public class SwipeRight : MicroGame {
    [SerializeField] private GameObject[] cards;
    private GameObject _currentCard;
    private List<GameObject> _deck;

    private void Start() {
        _deck = new List<GameObject>(cards);
        NextCard();
    }

    public void SwipedRight() {
        if (_currentCard.gameObject.name.StartsWith("Ad")) {
            GameManager.Instance.FinishMicroGame(false);
        } else {
            NextCard();
            GameManager.Instance.PlayProgressSound();
        }
    }

    public void SwipedLeft() {
        if (!_currentCard.gameObject.name.StartsWith("Ad")) {
            GameManager.Instance.FinishMicroGame(false);
        } else {
            NextCard();
            GameManager.Instance.PlayProgressSound();
        }
    }


    private void NextCard() {
        if (_deck.Count == 0) {
            GameManager.Instance.FinishMicroGame(true);
            return;
        }
        if (_currentCard) {
            Destroy(_currentCard);
        }
        Random random = new();
        int index = random.Next(0, _deck.Count);
        _currentCard = Instantiate(_deck[index], transform);
        _deck.RemoveAt(index);
        _currentCard.GetComponent<Card>().Initialize(this);
    }
}
