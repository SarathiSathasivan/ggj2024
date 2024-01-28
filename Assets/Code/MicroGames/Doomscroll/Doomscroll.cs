using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = System.Random;

public class Doomscroll : MicroGame, IDragHandler, IBeginDragHandler {
    [SerializeField] private GameObject articlePrefab;
    [SerializeField] private Transform parentTransform;

    private float _parentTransformInitialY;
    private float _padding = 400f;

    private void Start() {
        _padding = articlePrefab.GetComponent<RectTransform>().rect.height * .66f * Screen.height / 1080f;
        List<string> stories = new() {
            "The Surprising Link Between Chocolate and Longevity!",
            "Parallel Universes Exist--And You Have to Work in All of Them",
            "Lose 10 Pounds in a Week with the 'Air Only' Challenge!",
            "Celebrity Marriage on the Rocks",
            "Aliens exist, aren't actually that interesting.",
            "Ancient City Found Underneath the Amazon Rainforest!",
            "Superfood Showdown: Avocado vs. Kale",
            "Remember when Canada was on fire? It still is!",
            "World's First Time Travel Experiment: It failed!",
            "Will AI replace us? Hopefully!",
            "Dog explodes! Surprisingly unharmed.",
        };
        Random random = new();
        // For each spot in the array, pick
        // a random item to swap into that spot.
        Vector3 startPosition = parentTransform.position;
        _parentTransformInitialY = startPosition.y;
        for (int i = 0; i < 8; i++) {
            int j = random.Next(0, stories.Count);
            GameObject article = Instantiate(articlePrefab, startPosition, Quaternion.identity, parentTransform);
            article.GetComponent<Article>().Initialize(stories[j]);
            stories.RemoveAt(j);
            startPosition.y -= _padding;
        }
    }


    public void OnDrag(PointerEventData eventData) {
        Vector3 position = parentTransform.position;
        parentTransform.position = new Vector3(position.x, position.y + eventData.delta.y, position.z);
        if (parentTransform.position.y - _parentTransformInitialY > _padding * 7) {
            GameManager.Instance.FinishMicroGame(true);
        }
    }

    public void OnBeginDrag(PointerEventData eventData) {
        GameManager.Instance.PlayProgressSound();
    }
}
