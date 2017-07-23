using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulCreator : MonoBehaviour {

    public Sprite[] spriteArray;
    public GameObject soulPrefab;


    public void InstantiateSoulOnPosition(GameObject parent, int soulType) {
        GameObject soulObject = Instantiate(soulPrefab, parent.transform.position, Quaternion.identity);
        Soul soul = soulObject.GetComponent<Soul>();
        soul.setSoulType(soulType);
        soul.setSprite(spriteArray[soulType]);
        soulObject.transform.parent = parent.transform;
    }

    public void InstantiateRandomSoulOnPosition(GameObject parent) {
        int randomSoul = Random.Range(0, spriteArray.Length);
        InstantiateSoulOnPosition(parent , randomSoul);
    }

}