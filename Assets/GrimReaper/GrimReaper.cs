using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrimReaper : MonoBehaviour {

    private bool running = false;
    private Vector3 targetPosition;
    private float speed = 0.1f;
    private IntroManager introManager;

    private void Start() {
        introManager = FindObjectOfType<IntroManager>();
    }

    void Update() {
        if (running) {
            Debug.Log("Runnings: " + targetPosition.x);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed);
            if(transform.position == targetPosition) {
                introManager.LoadGameLevel();
            }
        } 
    }

    public void Running(bool running) {
        targetPosition = new Vector3(transform.position.x + 9, transform.position.y, transform.position.x);
        this.running = running;
    }
}
