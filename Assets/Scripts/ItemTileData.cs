/*
 * Created on 2022
 *
 * Copyright (c) 2022 dotmobstudio
 * Support : dotmobstudio@gmail.com
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
public class ItemTileData
{
    public int floorIndex;
    public Vector2Int posTile;
    public ItemData itemData;
    public int indexOnMap;
    public ItemTileData() {

    }

    public ItemTileData(int _floorIndex,int _indexOnMap, Vector2Int _posTile)
    {
        floorIndex = _floorIndex;
        indexOnMap = _indexOnMap;
        posTile = _posTile;
    }
}
