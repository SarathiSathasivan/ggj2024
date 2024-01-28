using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = System.Random;

public class Doomscroll : MicroGame, IDragHandler {
    [SerializeField] private GameObject articlePrefab;
    [SerializeField] private Transform parentTransform;

    private float _parentTransformInitialY;
    private const int Padding = 400;

    private void Start() {
        List<string> stories = new() {
            "The Surprising Link Between Chocolate and Longevity!",
            "Scientists Confirm Existence of Parallel Universes!",
            "Exclusive Interview with Alien Abductee",
            "Lose 10 Pounds in a Week with the 'Air Only' Challenge!",
            "Celebrity Marriage on the Rocks",
            "UFO Cover-Up Confirmed!",
            "Ancient City Found Underneath the Amazon Rainforest!",
            "Superfood Showdown: Avocado vs. Kale",
            "Celebrity Psychic Predicts 2024",
            "World's First Time Travel Experiment: It failed!",
            "Will AI replace us? Hopefully!",
            "Dog explodes! Surprisingly unharmed.",
        };
        _parentTransformInitialY = parentTransform.position.y;
        Random random = new();
        // For each spot in the array, pick
        // a random item to swap into that spot.
        Vector3 startPosition = parentTransform.position;
        for (int i = 0; i < 8; i++) {
            int j = random.Next(0, stories.Count);
            GameObject article = Instantiate(articlePrefab, startPosition, Quaternion.identity, parentTransform);
            article.GetComponent<Article>().Initialize(stories[j]);
            stories.RemoveAt(j);
            startPosition.y -= Padding;
        }
    }


    public void OnDrag(PointerEventData eventData) {
        Vector3 position = parentTransform.position;
        parentTransform.position = new Vector3(position.x, position.y + eventData.delta.y, position.z);
        if (parentTransform.position.y - _parentTransformInitialY > Padding * 7) {
            GameManager.Instance.FinishMicroGame(true);
        }
    }
}
