using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, IDragHandler, IEndDragHandler {
    // Start is called before the first frame update
    private Vector3 _initialPosition;
    private SwipeRight _parent;
    private void Start() {
        _initialPosition = transform.position;
    }

    public void Initialize(SwipeRight parent) {
        _parent = parent;
    }

    public void OnDrag(PointerEventData eventData) {
        // Debug.Log(eventData.delta);
        transform.position += (Vector3) eventData.delta;
        switch (transform.position.x - _initialPosition.x) {
            case > 120:
                _parent.SwipedRight();
                break;
            case < -120:
                _parent.SwipedLeft();
                break;
        }
    }

    public void OnEndDrag(PointerEventData eventData) {
        transform.position = _initialPosition;
    }
}
