using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Node
{
    public Coord gridCoOrd;
    //public int gridX;
    //public int gridY;
    public int tileHeight;
    public int moveDist;
    public Coord worldCoOrdinates
    {
        get
        {
            return new Coord(gridCoOrd.x, gridCoOrd.z);
        }
    }

    public Vector3 worldPosition;

    public bool isWalkable;
    public bool isMoveable;

    public Image geoTile;
    public Image combatTile;
    //public Character character;

    public Node(Coord coOrd, Vector3 worldPos, bool walkable, int _tileHeight, Image _geoTile, Image _combatTile)
    {
        gridCoOrd = coOrd;
        worldPosition = worldPos;
        isWalkable = walkable;
        geoTile = _geoTile;
        combatTile = _combatTile;
        tileHeight = _tileHeight;
    }

    public Node(Coord coOrd, Vector3 worldPos, bool walkable)
    {
        gridCoOrd = coOrd;
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
