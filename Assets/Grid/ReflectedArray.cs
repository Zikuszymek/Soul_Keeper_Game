using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectedArray : MonoBehaviour {

    private int[][] reflectedArray;
    private int width;
    private int height;
    private int typeOfSouls;

    public ReflectedArray(int[][] reflectedArray, int typeOfSouls) {
        this.reflectedArray = reflectedArray;
        this.width = reflectedArray.Length;
        this.height = reflectedArray[0].Length;
        this.typeOfSouls = typeOfSouls;
    }

    public void reflectTheGridOnArray(int[][] reflectedArray) {
        this.reflectedArray = reflectedArray;
    }

    public int[][] getReflectedArray() {
        return reflectedArray;
    }

    public void CreateFirstSoulsGrid() {
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                reflectedArray[x][y] = Random.Range(0, typeOfSouls);
            }
        }
        if (!DoesAreAnyMovesInGame() || GetAllMachingSouls().Count > 0) {
            CreateFirstSoulsGrid();
        }
    }

    /*SEARCH FOR MOVES*/

    public bool DoesAreAnyMovesInGame() {
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                int thisSoulType = reflectedArray[x][y];
                if (CompareNextTwoGridsHorizontal(x, y, thisSoulType) || CompareSideGridsHorizontal(x, y, thisSoulType)
                    || CompareNextTwoGridsVertical(x, y, thisSoulType) || CompareSideGridsVertical(x, y, thisSoulType)) {
                    return true;
                }
            }
        }
        return false;
    }

    private bool CompareSideGridsHorizontal(int x, int y, int soulType) {
        if (x + 2 < width) {
            int soulTypeXPlus2 = reflectedArray[x + 2][y];
            if (y + 1 < height) {
                int soulTypeYPlus1 = reflectedArray[x + 1][y + 1];
                if (soulTypeXPlus2 == soulType && soulTypeYPlus1 == soulType) {
                    return true;
                }
            }
            if (y - 1 >= 0) {
                int soulTypeYMinus1 = reflectedArray[x + 1][y - 1];
                if (soulTypeXPlus2 == soulType && soulTypeYMinus1 == soulType) {
                    return true;
                }
            }
        }
        return false;
    }

    private bool CompareSideGridsVertical(int x, int y, int soulType) {
        if (y + 2 < height) {
            int soulTypeXPlus2 = reflectedArray[x][y + 2];
            if (x + 1 < height) {
                int soulTypeYPlus1 = reflectedArray[x + 1][y + 1];
                if (soulTypeXPlus2 == soulType && soulTypeYPlus1 == soulType) {
                    return true;
                }
            }
            if (x - 1 >= 0) {
                int soulTypeYMinus1 = reflectedArray[x - 1][y + 1];
                if (soulTypeXPlus2 == soulType && soulTypeYMinus1 == soulType) {
                    return true;
                }
            }
        }
        return false;
    }

    private bool CompareNextTwoGridsHorizontal(int x, int y, int soulType) {
        if (x + 3 < width) {
            int soulTypeXPlus1 = reflectedArray[x + 1][y];
            int soulTypeXPlus2 = reflectedArray[x + 2][y];
            int soulTypeXPlus3 = reflectedArray[x + 3][y];
            if ((soulTypeXPlus2 == soulType && soulTypeXPlus3 == soulType) || (soulTypeXPlus1 == soulType && soulTypeXPlus3 == soulType)) {
                return true;
            }
        }
        return false;
    }

    private bool CompareNextTwoGridsVertical(int x, int y, int soulType) {
        if (y + 3 < height) {
            int soulTypeYPlus1 = reflectedArray[x][y + 1];
            int soulTypeYPlus2 = reflectedArray[x][y + 2];
            int soulTypeYPlus3 = reflectedArray[x][y + 3];
            if ((soulTypeYPlus2 == soulType && soulTypeYPlus3 == soulType) || (soulTypeYPlus1 == soulType && soulTypeYPlus3 == soulType)) {
                return true;
            }
        }
        return false;
    }

    /*FIND ALL MACHES*/

    public List<Position> GetAllMachingSouls() {
        List<Position> soulsToDestroy = new List<Position>();
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                List<Position> temporaryList = GetAllMachingSoulForGridPosition(x, y);
                foreach (Position gameObject in temporaryList) {
                    if (!soulsToDestroy.Contains(gameObject)) {
                        soulsToDestroy.Add(gameObject);
                    }
                }

            }
        }
        return soulsToDestroy;
    }

    public List<Position> GetAllMachingSoulForGridPosition(int x, int y) {
        List<Position> soulsToDestroy = new List<Position>();
        int soulType = reflectedArray[x][y];
        List<Position> verticalList = GetVerticalMachesList(x, y, soulType);
        List<Position> horizontalList = GetHorizontalMachesList(x, y, soulType);

        if (verticalList.Count >= 2 || horizontalList.Count >= 2) {
            soulsToDestroy.Add(new Position(x, y));
        }
        return soulsToDestroy;
    }

    public List<Position> GetHorizontalMachesList(int x, int y, int soulType) {
        int originalY = y;
        List<Position> list = new List<Position>();
        y++;
        while (y < height) {
            if (soulType == reflectedArray[x][y]) {
                list.Add(new Position(x, y));
            } else {
                break;
            }
            y++;
        }
        originalY--;
        while (originalY >= 0) {
            if (soulType == reflectedArray[x][originalY]) {
                list.Add(new Position(x, originalY));
            } else {
                break;
            }
            originalY--;
        }
        return list;
    }

    public List<Position> GetVerticalMachesList(int x, int y, int soulType) {
        int originalX = x;
        List<Position> list = new List<Position>();
        x++;
        while (x < width) {
            if (soulType == reflectedArray[x][y]) {
                list.Add(new Position(x, y));
            } else {
                break;
            }
            x++;
        }
        originalX--;
        while (originalX >= 0) {
            if (soulType == reflectedArray[originalX][y]) {
                list.Add(new Position(originalX, y));
            } else {
                break;
            }
            originalX--;
        }
        return list;
    }

}
