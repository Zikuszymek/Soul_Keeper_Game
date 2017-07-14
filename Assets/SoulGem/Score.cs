using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour {

    private static readonly string DESTROY = "Destroy";

    private float speed = 150f;

    private Vector3 targetPosition;
    private GameObject scores;
    private bool prepareToBeDestroyed = false;
    private Animator animator;
    private GameGrid gameGrid;
    //private Camera camera;

	void Start () {
        //camera = FindObjectOfType<Camera>();
        gameGrid = FindObjectOfType<GameGrid>();
        animator = GetComponent<Animator>();
        targetPosition = transform.position;
        scores = GameObject.FindGameObjectWithTag("Scores");
    }

	void Update () {
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
        if(prepareToBeDestroyed && transform.position == targetPosition) {
            animator.SetTrigger(DESTROY);
        }
    }

    public void GoToScores() {
        prepareToBeDestroyed = true;
        targetPosition = (scores.transform.position);

    }
    public void UpdateScores() {
        gameGrid.UpdateScores(10);
    }
    
    public void Destroy() {
        Destroy(transform.gameObject);
    }
}
