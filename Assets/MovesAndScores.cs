using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovesAndScores : MonoBehaviour {

    public Text movesText;
    public Text scoresText;

    private int moves = 0;
    private int scores = 0;

    public void UpdateScores(int scores) {
        this.scores += scores;
        scoresText.text = this.scores.ToString();
    }

    public void UpdateMoves() {
        moves++;
        movesText.text = moves.ToString();
    }

}
