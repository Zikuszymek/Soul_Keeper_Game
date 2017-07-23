using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameGrid : MonoBehaviour {

    public const int DROP_SOULS = 0, SWAP_BACK = 1, MIX = 2, MOVE_ALLOWED = 3, SOULS_SWAPED = 4, DESTROY_SOULS = 5;

    public static int currentGameSatus = DROP_SOULS;

    public int width;
    public int height;
    public GameObject gridPrefab;

    private GameObject[][] soulsGrid;
    private GridManager gridManager;
    private List<GameObject> soulListToDestroy;
    private AudioSource audioSource;
    private MovesAndScores movesAndScores;
    private SoulCreator soulCreator;

    void Start() {
        soulCreator = FindObjectOfType<SoulCreator>();
        audioSource = GetComponent<AudioSource>();
        movesAndScores = FindObjectOfType<MovesAndScores>();
        soulListToDestroy = new List<GameObject>();
        soulsGrid = new GameObject[width][];
        InstantiateAllGrids();
        CenterTheGrid();
        gridManager = new GridManager(soulsGrid, soulCreator);
        gridManager.GetStartGrid();
        ChangeStatus(DROP_SOULS);
    }

    private void Update() {
        switch (currentGameSatus) {
            case DROP_SOULS:
                if (!gridManager.DoesAnySoulMove()) {
                    DestroyMaches();
                }
                break;
            case DESTROY_SOULS:
                if (CheckSoulsToDestroy() == 0) {
                    DragSoulsDown();
                }
                break;

            case MIX:
                if (!gridManager.DoesAnySoulMove()) {
                    DestroyMaches();
                }
                break;

            case MOVE_ALLOWED:
                break;

            case SOULS_SWAPED:
                if (!gridManager.DoesAnySoulMove()) {
                    CheckSoulsAfterSwap();
                }
                break;

            case SWAP_BACK:
                if (!gridManager.DoesAnySoulMove()) {
                    ChangeStatus(MOVE_ALLOWED);
                }
                break;
        }
    }

    public void SwapSouls(int swapType) {
        Swap(GridSquare.selectedSoul1, GridSquare.selectedSoul2);
        ChangeStatus(swapType);
    }

    public void ChangeStatus(int currentGameSatus) {
        gridManager.ReflectTheGridOnArray();
        GameGrid.currentGameSatus = currentGameSatus;
    }

    public GameObject getGridSquareAt(float x, float y) {
        return soulsGrid[(int)x][(int)y];
    }

    private void InstantiateAllGrids() {
        for (int x = 0; x < width; x++) {
            soulsGrid[x] = new GameObject[height];
            for (int y = 0; y < height; y++) {
                Vector2 gridPosition = new Vector2(x, y);
                GameObject createdGridSpace = Instantiate(gridPrefab, gridPosition, Quaternion.identity);
                createdGridSpace.transform.parent = transform;
                soulsGrid[x][y] = createdGridSpace;
            }
        }
    }

    private void CenterTheGrid() {
        transform.position = new Vector3(-(width / 2), -(height / 2), transform.position.z);
    }

    private void DragSoulsDown() {
        gridManager.DragSoulsDown();
        ChangeStatus(DROP_SOULS);
        soulListToDestroy = new List<GameObject>();
    }

    private void CheckSoulsAfterSwap() {
        soulListToDestroy = gridManager.GetAllMachingSouls();
        if (CheckSoulsToDestroy() == 0) {
            PlayError();
            SwapSouls(SWAP_BACK);
        } else {
            movesAndScores.UpdateMoves();
            DestroyMaches();
        }
    }

    private void DestroyMaches() {
        soulListToDestroy = gridManager.GetAllMachingSouls();
        if (soulListToDestroy.Count > 0) {
            for (int i = 0; i < soulListToDestroy.Count; i++) {
                soulListToDestroy[i].GetComponent<Soul>().StartDestroyAnimations(i);
            }
            ChangeStatus(DESTROY_SOULS);
        } else {
            CheckForMoves();
        }
    }

    private void CheckForMoves() {
        if (gridManager.DoesAreAnyMovesInGame()) {
            ChangeStatus(MOVE_ALLOWED);
        } else {
            gridManager.MixTheArray();
            ChangeStatus(MIX);
        }
    }

    private int CheckSoulsToDestroy() {
        int i = 0;
        foreach (GameObject gameObject in soulListToDestroy) {
            if (gameObject != null) {
                i++;
            }
        }
        return i;
    }

    private void PlayError() {
        audioSource.Play();
    }

    private void Swap(GameObject soulOne, GameObject soulTwo) {
        Vector3 lazyposition = soulOne.transform.position;
        GameObject soulParent = soulOne.transform.parent.gameObject;
        GameObject soulParent2 = soulTwo.transform.parent.gameObject;
        Soul selectedSoulScript1 = soulOne.GetComponent<Soul>();
        Soul selectedSoulScript2 = soulTwo.GetComponent<Soul>();

        soulOne.transform.parent = soulParent2.transform;
        soulTwo.transform.parent = soulParent.transform;

        selectedSoulScript1.setTargetPosition(soulParent2.transform.position, 0.5f);
        selectedSoulScript2.setTargetPosition(lazyposition, 0.5f);
    }

}
