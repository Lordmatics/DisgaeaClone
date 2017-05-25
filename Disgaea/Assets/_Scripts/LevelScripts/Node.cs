using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Node {

    public int gridX;
    public int gridY;
    public int tileHeight;
    public int moveDist;
    public Vector2 worldCoOrdinates
    {
        get
        {
            return new Vector2(gridX, gridY);
        }
    }

    public Vector3 worldPosition;

    public bool isWalkable;
    public bool isMoveable;

    public Image geoTile;
    public Image combatTile;
    //public Character character;

    public Node(int x, int z, Vector3 worldPos, bool walkable, int _tileHeight, Image _geoTile, Image _combatTile)
    {
        gridX = x;
        gridY = z;
        worldPosition = worldPos;
        isWalkable = walkable;
        geoTile = _geoTile;
        combatTile = _combatTile;
        tileHeight = _tileHeight;
    }

    public Node(int x, int z, Vector3 worldPos, bool walkable)
    {
        gridX = x;
        gridY = z;
        worldPosition = worldPos;
        isWalkable = walkable;
    }

    public void SetCombatTile(int tileTypeIndex)
    {
        combatTile.enabled = true;
        switch(tileTypeIndex)
        {
            case 0:
                combatTile.color = new Vector4(1, 0, 0, 0.8f);
                break;
            case 1:
                combatTile.color = new Vector4(0, 0, 1, 0.8f);
                break;
            case 2:
                combatTile.color = new Vector4(1, 1, 0, 0.8f);
                break;
        }
    }

    public void ResetNode()
    {
        moveDist = 0;
        combatTile.enabled = false;
        isMoveable = false;
    }
}
