using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager {
    public readonly static int NO_SOUL = -1;

    private GameObject[][] soulsGrid;
    private GameGrid gameGrid;
    private int width;
    private int height;

    delegate void ActionsInLoop();

    public GridManager(GameObject[][] soulsGrid, GameGrid gameGrid) {
        this.soulsGrid = soulsGrid;
        this.gameGrid = gameGrid;
        width = soulsGrid.Length;
        height = soulsGrid[0].Length;
    }

    /*SEARCH FOR MOVES*/

    public bool DoesAreAnyMovesInGame() {
        //Debug.Log("Checking moves");
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                int thisSoulType = GetSoulTypeOnPosition(x, y);
                if(CompareNextTwoGridsHorizontal(x, y, thisSoulType) || CompareSideGridsHorizontal(x, y, thisSoulType)
                    || CompareNextTwoGridsVertical(x, y, thisSoulType) || CompareSideGridsVertical(x, y, thisSoulType)) {
                    return true;
                }
            }
        }
        return false;
    }

    private bool CompareSideGridsHorizontal(int x, int y, int soulType) {
        if (x + 2 < width) {
            int soulTypeXPlus2 = GetSoulTypeOnPosition(x + 2, y);
            if (y + 1 < height) {
                int soulTypeYPlus1 = GetSoulTypeOnPosition(x + 1, y + 1);
                if (soulTypeXPlus2 == soulType && soulTypeYPlus1 == soulType) {
                    return true;
                }
            }
            if (y - 1 >= 0) {
                int soulTypeYMinus1 = GetSoulTypeOnPosition(x + 1, y - 1);
                if (soulTypeXPlus2 == soulType && soulTypeYMinus1 == soulType) {
                    return true;
                }
            }
        }
        return false;
    }

    private bool CompareSideGridsVertical(int x, int y, int soulType) {
        if (y + 2 < height) {
            int soulTypeXPlus2 = GetSoulTypeOnPosition(x, y + 2);
            if (x + 1 < height) {
                int soulTypeYPlus1 = GetSoulTypeOnPosition(x + 1, y + 1);
                if (soulTypeXPlus2 == soulType && soulTypeYPlus1 == soulType) {
                    return true;
                }
            }
            if (x -1 >= 0) {
                int soulTypeYMinus1 = GetSoulTypeOnPosition(x - 1, y + 1);
                if (soulTypeXPlus2 == soulType && soulTypeYMinus1 == soulType) {
                    return true;
                }
            }
        }
        return false;
    }

    private bool CompareNextTwoGridsHorizontal(int x, int y, int soulType) {
        if (x + 3 < width) {
            int soulTypeXPlus1 = GetSoulTypeOnPosition(x + 1, y);
            int soulTypeXPlus2 = GetSoulTypeOnPosition(x + 2, y);
            int soulTypeXPlus3 = GetSoulTypeOnPosition(x + 3, y);
            if ((soulTypeXPlus2 == soulType && soulTypeXPlus3 == soulType) || (soulTypeXPlus1 == soulType && soulTypeXPlus3 == soulType)) {
                return true;
            }
        }
        return false;
    }

    private bool CompareNextTwoGridsVertical(int x, int y, int soulType) {
        if (y + 3 < height) {
            int soulTypeYPlus1 = GetSoulTypeOnPosition(x, y + 1);
            int soulTypeYPlus2 = GetSoulTypeOnPosition(x, y + 2);
            int soulTypeYPlus3 = GetSoulTypeOnPosition(x, y + 3);
            if ((soulTypeYPlus2 == soulType && soulTypeYPlus3 == soulType) || (soulTypeYPlus1 == soulType && soulTypeYPlus3 == soulType)) {
                return true;
            }
        }
        return false;
    }

    private int GetSoulTypeOnPosition(int x, int y) {
        if (soulsGrid[x][y].transform.childCount > 0) {
            return soulsGrid[x][y].transform.GetChild(0).GetComponent<Soul>().getSoulType();
        }
        return NO_SOUL;
    }

    /*MIX THE GRID*/

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

    /*FIND AND DESTROY MACHES*/

    public List<GameObject> DestroyALlMachingSouls() {
        List<GameObject> soulsToDestroy = new List<GameObject>();
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                if (soulsGrid[x][y].transform.childCount > 0) {
                    List<GameObject> temporaryList = DestroyAllMachingSoulForGridPosition(soulsGrid[x][y]);
                    foreach(GameObject gameObject in temporaryList) {
                        if (!soulsToDestroy.Contains(gameObject)) {
                            soulsToDestroy.Add(gameObject);
                        }
                    }
                }
            }
        }
        return soulsToDestroy;
    }

    public List<GameObject> DestroyAllMachingSoulForGridPosition(GameObject gridObject) {
        List<GameObject> soulsToDestroy = new List<GameObject>();
        int x = (int)gridObject.transform.localPosition.x;
        int y = (int)gridObject.transform.localPosition.y;
        int soulType = gridObject.transform.GetChild(0).GetComponent<Soul>().getSoulType();
        List<GameObject> verticalList = GetVerticalMachesList(x, y, soulType);
        List<GameObject> horizontalList = GetHorizontalMachesList(x, y, soulType);

        if (verticalList.Count >= 2 || horizontalList.Count >= 2) {
            soulsToDestroy.Add(gridObject.transform.GetChild(0).gameObject);
        }

        if (verticalList.Count >= 2) {
            for (int i = 0; i < verticalList.Count; i++) {
                soulsToDestroy.Add(gridObject.transform.GetChild(0).gameObject);
            }
        }

        if (horizontalList.Count >= 2) {
            for (int i = 0; i < horizontalList.Count; i++) {
                soulsToDestroy.Add(gridObject.transform.GetChild(0).gameObject);
            }
        }
        return soulsToDestroy;
    }

    private List<GameObject> GetHorizontalMachesList(int x, int y, int soulType) {
        int originalY = y;
        List<GameObject> list = new List<GameObject>();
        y++;
        while (y < height) {
            if (soulType == GetSoulTypeOnPosition(x, y)) {
                list.Add(soulsGrid[x][y]);
            } else {
                break;
            }
            y++;
        }
        originalY--;
        while (originalY >= 0) {
            if (soulType == GetSoulTypeOnPosition(x, originalY)) {
                list.Add(soulsGrid[x][originalY]);
            } else {
                break;
            }
            originalY--;
        }
        return list;
    }

    private List<GameObject> GetVerticalMachesList(int x, int y, int soulType) {
        int originalX = x;
        List<GameObject> list = new List<GameObject>();
        x++;
        while (x < width) {
            if (soulType == GetSoulTypeOnPosition(x, y)) {
                list.Add(soulsGrid[x][y]);
            } else {
                break;
            }
            x++;
        }
        originalX--;
        while (originalX >= 0) {
            if (soulType == GetSoulTypeOnPosition(originalX, y)) {
                list.Add(soulsGrid[originalX][y]);
            } else {
                break;
            }
            originalX--;
        }
        return list;
    }


    /*DRAG SOULS DOWN AND FILL EMPTY*/

    public void DragSoulsDown() {
        //Debug.Log("Drag start");
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
                gameGrid.InstantiateSoulOnPosition(positionX, positionY);
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
            gameGrid.InstantiateSoulOnPosition(positionX, y);
        }
    }

    /*ADDITIONALS*/

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
