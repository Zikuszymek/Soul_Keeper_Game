using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager {

    public readonly static int NO_SOUL = -1;

    private GameObject[][] soulsGrid;
    private ReflectedArray reflectedArray;
    private SoulCreator soulCreator;
    private int width;
    private int height;

    delegate void ActionsInLoop();

    public GridManager(GameObject[][] soulsGrid, SoulCreator soulCreator) {
        this.soulsGrid = soulsGrid;
        this.soulCreator = soulCreator;
        width = soulsGrid.Length;
        height = soulsGrid[0].Length;
        this.reflectedArray = new ReflectedArray(instantiateArray(), soulCreator.spriteArray.Length);
        reflectedArray.CreateFirstSoulsGrid();
    }

    public void ReflectTheGridOnArray() {
        reflectedArray.reflectTheGridOnArray(reflectCurrentGrid());
    }

    private int[][] reflectCurrentGrid() {
        int[][] reflectedArray = instantiateArray();
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                reflectedArray[x][y] = GetSoulTypeOnPosition(x, y);
            }
        }
        return reflectedArray;
    }

    private int[][] instantiateArray() {
        int[][] reflectedGrid = new int[width][];
        for (int i = 0; i < width; i++) {
            reflectedGrid[i] = new int[height];
        }
        return reflectedGrid;
    }

    private int GetSoulTypeOnPosition(int x, int y) {
        if (soulsGrid[x][y].transform.childCount > 0) {
            return soulsGrid[x][y].transform.GetChild(0).GetComponent<Soul>().getSoulType();
        }
        return NO_SOUL;
    }

    public bool DoesAreAnyMovesInGame() {
        return reflectedArray.DoesAreAnyMovesInGame();
    }

    public void MixTheArray() {
        List<GameObject> soulsList = new List<GameObject>();
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                soulsList.Add(soulsGrid[x][y].transform.GetChild(0).gameObject);
            }
        }
        MixTheSoulsInGrid(GetRandomizedSoulList(soulsList));
    }

    private List<GameObject> GetRandomizedSoulList(List<GameObject> regularList) {
        List<GameObject> randomList = new List<GameObject>();
        int randomInt;
        while (regularList.Count > 0) {
            randomInt = Random.Range(0, regularList.Count);
            randomList.Add(regularList[randomInt]);
            regularList.RemoveAt(randomInt);
        }
        return randomList;
    }

    private void MixTheSoulsInGrid(List<GameObject> soulList) {
        int listCount = 0;
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                GameObject parent = soulsGrid[x][y];
                GameObject soulObject = soulList[listCount];
                Soul soul = soulObject.GetComponent<Soul>();
                ChangeSoulPosition(soulObject, soul, parent);
                listCount++;
            }
        }
    }

    private void ChangeSoulPosition(GameObject soulObject, Soul soul, GameObject parent) {
        soulObject.transform.parent = parent.transform;
        soul.setTargetPosition(parent.transform.position, 1f);
    }

    public List<GameObject> GetAllMachingSouls() {
        List<Position> positionList = reflectedArray.GetAllMachingSouls();
        List<GameObject> objectToDestroy = new List<GameObject>();
        foreach(Position position in positionList) {
            objectToDestroy.Add(soulsGrid[position.getX()][position.getY()].transform.GetChild(0).gameObject);
        }
        return objectToDestroy;
    }

    /*DRAG SOULS DOWN AND FILL EMPTY*/

    public void DragSoulsDown() {
        for (int x = 0; x < width; x++) {
            SearchForFirstExistingSoulInY(x, 0);
        }
    }

    private void SearchForFirstExistingSoulInY(int positionX, int positionY) {
        GameObject gridSquare = soulsGrid[positionX][positionY];
        int childCount = gridSquare.transform.childCount;
        if (childCount == 0) {
            GameObject existingSoul = GetFirsExistingSoulInY(positionX, positionY + 1);
            if (existingSoul != null) {
                Soul soul = existingSoul.GetComponent<Soul>();
                existingSoul.transform.parent = gridSquare.transform;
                ChangeSoulPosition(existingSoul, soul, gridSquare);
                if (positionY + 1 < height) {
                    SearchForFirstExistingSoulInY(positionX, positionY + 1);
                }
            } else {
                soulCreator.InstantiateRandomSoulOnPosition(soulsGrid[positionX][positionY]);
                FillAllAbove(positionX, positionY + 1);
            }
        } else {
            if (positionY + 1 < height) {
                SearchForFirstExistingSoulInY(positionX, positionY + 1);
            }
        }

    }

    private GameObject GetFirsExistingSoulInY(int positionX, int positionY) {
        for (int y = positionY; y < height; y++) {
            GameObject gridSquare = soulsGrid[positionX][y];
            int childCount = gridSquare.transform.childCount;
            if (childCount != 0) {
                return gridSquare.transform.GetChild(0).gameObject;
            }
        }
        return null;
    }

    private void FillAllAbove(int positionX, int positionY) {
        for (int y = positionY; y < height; y++) {
            soulCreator.InstantiateRandomSoulOnPosition(soulsGrid[positionX][y]);
        }
    }

    /*ADDITIONALS*/

    public void GetStartGrid() {
        int[][] soulsArray = reflectedArray.getReflectedArray();
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                soulCreator.InstantiateSoulOnPosition(soulsGrid[x][y], soulsArray[x][y]);
            }
        }
    }

    public bool DoesAnySoulMove() {
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                if (soulsGrid[x][y].transform.childCount > 0) {
                    Soul soul = soulsGrid[x][y].transform.GetChild(0).gameObject.GetComponent<Soul>();
                    if (soul.isSoulMowing) {
                        return true;
                    }
                } else {
                    return true;
                }
            }
        }
        return false;
    }

    public bool IsFirstSoulMoving() {
        if (soulsGrid[0][0].transform.childCount > 0) {
            Soul soul = soulsGrid[0][0].transform.GetChild(0).gameObject.GetComponent<Soul>();
            if (soul.isSoulMowing) {
                return true;
            }
        } else {
            return true;
        }
        return false;

    }

}
