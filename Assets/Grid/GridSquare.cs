using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSquare : MonoBehaviour {

    public static bool isDragAllowed = true;
    private static bool isSoulSelected = false;
    public static GameObject selectedSoul1;
    public static GameObject selectedSoul2;

    private ParticleSystem particleSystem;
    private Soul soul;
    private GameGrid gameGrid;

    private void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
        gameGrid = FindObjectOfType<GameGrid>();
    }

    public void setSoul(Soul soul)
    {
        this.soul = soul;
    }

    public Soul getSoul()
    {
        return soul;
    }

    private void OnMouseDown()
    {
        if (isDragAllowed && GameGrid.currentGameSatus == GameGrid.MOVE_ALLOWED)
        {
            isSoulSelected = true;
            selectedSoul1 = transform.GetChild(0).gameObject;
            isDragAllowed = false;
        }
    }

    private void OnMouseEnter()
    {
        if (isSoulSelected && selectedSoul1 != transform.GetChild(0).gameObject)
        {
            isDragAllowed = true;
            isSoulSelected = false;
            selectedSoul2 = GetSelectedOffsetSoul().transform.GetChild(0).gameObject;
            gameGrid.SwapSouls(GameGrid.SOULS_SWAPED);
        }
    }

    private GameObject GetSelectedOffsetSoul() {
        Vector3 selectedSoulTransform = selectedSoul1.transform.parent.transform.localPosition;
        float xSign = transform.localPosition.x - selectedSoulTransform.x;
        float ySign = transform.localPosition.y - selectedSoulTransform.y;

        if (Mathf.Abs(xSign) > Mathf.Abs(ySign)) {
            ySign = selectedSoulTransform.y;
            if (xSign < 0) {
                xSign = selectedSoulTransform.x - 1;
            } else {
                xSign = selectedSoulTransform.x + 1;
            }
        } else {
            xSign = selectedSoulTransform.x;
            if (ySign < 0) {
                ySign = selectedSoulTransform.y - 1;
            } else {
                ySign = selectedSoulTransform.y + 1;
            }
        }

        return gameGrid.getGridSquareAt(xSign, ySign);
    }

    public void EmpitParticles() {
        particleSystem.randomSeed = (uint)Random.Range(0, 9999999);
        particleSystem.Simulate(0, false, true);
        particleSystem.Emit(10);
    }

}
