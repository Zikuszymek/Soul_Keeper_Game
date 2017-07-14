using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulModel {

    private int soultType;
    private Sprite sprite;

    public SoulModel(Sprite sprite, int soulType)
    {
        this.soultType = soulType;
        this.sprite = sprite;
    }
}
