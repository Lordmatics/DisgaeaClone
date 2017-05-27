using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class SavedMap
{
    public Vector2 gridWorldSize;
    public LayerMask floorLayermask;
    [Range(0, 1)]
    public float outlinePercent;

    public List<GameObject> spawnedTiles = new List<GameObject>();

}

[AddComponentMenu("Scripts/LevelScripts/Grid")]
public class Grid : MonoBehaviour
{
    public int mapIndex;

    //public LayerMask floorLayermask;
    //public Vector2 gridWorldSize;

    // Danes grid int
    int gridSizeX, gridSizeY;
    public int MaxSize
    {
        get
        {
            return gridSizeX * gridSizeY;
        }
    }
    // This can be static maybe? nah
    public float nodeRadius;
    Node[,] grid;

    //[Range(0,1)]
    //public float outlinePercent;

    SavedMap currentMap;
    public SavedMap[] maps;

    // I can maybe make this dynamic during edit time if you like
    // + Do we want a scene for each level - as in game level
    void Awake()
    {
        currentMap = maps[0]; // Just have 1 map atm.
        CreateGrid();
    }

    public void GenerateVisualGrid()
    {
        currentMap = maps[mapIndex];

        //CreateGrid();
        string mapContainer = "Generated Map";
        if(transform.Find(mapContainer))
        {
            if(currentMap.spawnedTiles.Count > 0)
            {
                for (int i = currentMap.spawnedTiles.Count - 1; i >= 0; i--)
                {
                    GameObject temp = currentMap.spawnedTiles[i];
                    currentMap.spawnedTiles.Remove(temp);
                }
            }
            DestroyImmediate(transform.Find(mapContainer).gameObject);
        }

        Transform mapHolder = new GameObject(mapContainer).transform;
        mapHolder.parent = transform;

        // Not sure how to make this work using the property approach for grid size and maxsize etc
        // You might be able to figure it out.
        // Editor script works by calling this function every frame, whilst the script is selected in editor
        // Therefore the string check + Destroy immediate is neccessary to prevent infinite spawning
        for (int i = 0; i < currentMap.gridWorldSize.x; i++)
        {
            for (int j = 0; j < currentMap.gridWorldSize.y; j++)
            {
                Vector3 tilePosition = new Vector3(-currentMap.gridWorldSize.x / 2 + 0.5f + i, 0.0f, -currentMap.gridWorldSize.y / 2 + 0.5f + j);
                // Quaternion.Euler(Vector3.right * 90) use that if using Quads
                GameObject newTile = (GameObject)Instantiate(Resources.Load("GridPrefabs/GridNode"),tilePosition,Quaternion.identity);
                newTile.transform.localScale = Vector3.one * (1 - currentMap.outlinePercent);
                newTile.transform.parent = mapHolder;
                //Tile script = newTile.GetComponent<Tile>();
                currentMap.spawnedTiles.Add(newTile);
            }
        }
    }

    public void CreateGrid()
    {
        //Debug.Log("Grid");
        float nodeDiameter = nodeRadius * 2;
        Vector3 worldBottomLeft = transform.position - (Vector3.right * currentMap.gridWorldSize.x / 2) - (Vector3.forward * currentMap.gridWorldSize.y / 2);
        gridSizeX = Mathf.RoundToInt(currentMap.gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(currentMap.gridWorldSize.y / nodeDiameter);
        grid = new Node[gridSizeX, gridSizeY];
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                //Transform tile = ShootRaycastTransform(worldPoint + (Vector3.up * 20), Vector3.down, 25, floorLayermask);
                //Transform tile = Utility.ShootRaycastTransform(worldPoint + (Vector3.up * 20), Vector3.down, 25, currentMap.floorLayermask);
                /*if (tile != null)
                {
                    Transform canvas = tile.parent.Find("Canvas");
                    int height = GetTileHeight(tile.position.y);                   
                    grid[x, y] = new Node(new Coord(x, y), worldPoint, true, height, canvas.GetChild(0).GetComponent<UnityEngine.UI.Image>(), canvas.GetChild(1).GetComponent<UnityEngine.UI.Image>());
                }*/
                //else
                //{
                    grid[x, y] = new Node(new Coord(gridSizeX, gridSizeY), worldPoint, false);
                //}
            }
        }
    }

    int GetTileHeight(float tileY)
    {
        int height = (int)((0.1f + tileY) * 10); // the extra 0.1 is to make it so that 0 world height = 1 height. {(0.1 + 0) * 10 = 1};
        return height;
    }

    public Node GetNodeFromWorldCoOrdinate(Coord coOrdinate)
    {
        int x = coOrdinate.x;
        int z = coOrdinate.z;
        if (x > (int)currentMap.gridWorldSize.x - 1|| x < 0 || z > (int)currentMap.gridWorldSize.y - 1 || z < 0)
        {
            return null;
        }
        return grid[(x), (z)];
    }

    public Coord GetWorldCoOrdinates(Vector3 position)
    {
        int gridRadius = (((int)currentMap.gridWorldSize.x - 1) / 2);
        int x = (int)position.x + gridRadius;
        int z = (int)position.z + gridRadius;

        return new Coord(x, z);
    }

    public bool InBorderCheck(Coord check)
    {
        int gridRadius = (((int)currentMap.gridWorldSize.x - 1) / 2);
        if (check.x <= gridRadius &&
            check.z <= gridRadius &&
            check.x >= -gridRadius &&
            check.z >= -gridRadius)
        {
            return true;
        }
        return false;
    }

    public Vector2 AlternateBorderCheck(Coord check, Vector2 dir)
    {
        print(check.x +"_"+ check.z);
        int gridRadius = (((int)currentMap.gridWorldSize.x - 1) / 2);
        /*
        if (check.x <= gridRadius && // if x is within the bounds by z isn't.
            check.x >= -gridRadius &&
            check.z > gridRadius ||
            check.x <= gridRadius && // if x is within the bounds by z isn't.
            check.x >= -gridRadius &&
            check.z < -gridRadius)
        {
            return new Vector2(1 * Mathf.Sign(check.x), 0);
        }
        else if (check.z <= gridRadius && // if z is within the bounds by x isn't.
                 check.z >= -gridRadius &&
                 check.x > gridRadius ||
                 check.z <= gridRadius && // if z is within the bounds by x isn't.
                 check.z >= -gridRadius &&
                 check.x < -gridRadius)
        {
            return new Vector2(0, 1 * Mathf.Sign(check.x));
        }*/
        if(check.x <= gridRadius && check.x >= -gridRadius)
        {
            if (check.z > gridRadius || check.z < gridRadius)
                return new Vector2(1 * Mathf.Sign(dir.x), 0);
        }
        else if (check.z <= gridRadius && check.z >= -gridRadius)
        {
            if (check.x > gridRadius || check.x < gridRadius)
                return new Vector2(0, 1 * Mathf.Sign(dir.y));
        }
        return Vector2.zero;
    }
}

