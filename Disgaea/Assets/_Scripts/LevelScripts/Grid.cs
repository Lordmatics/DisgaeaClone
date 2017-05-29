using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

/*[System.Serializable]
public class SavedMap
{
    public Vector2 gridWorldSize;
    public LayerMask floorLayermask;
    [Range(0, 1)]
    public float outlinePercent;

    List<GameObject> spawnedTiles = new List<GameObject>();

}*/

[AddComponentMenu("Scripts/LevelScripts/Grid")]
public class Grid : MonoBehaviour
{
    public int mapIndex;
    public NodeInformation nodeInformation;

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
    const float nodeRadius = 0.5f;
    Node[,] grid;

    [HideInInspector]
    public Coord gridWorldSize;
    public LayerMask floorLayermask;
    [Range(0, 1)]
    public float outlinePercent;

    List<GameObject> spawnedTiles = new List<GameObject>();

    // I can maybe make this dynamic during edit time if you like
    // + Do we want a scene for each level - as in game level
    void Awake()
    {

    }

    public void GenerateVisualGrid()
    {
        //CreateGrid();
        ClearTileMap();
        string mapContainer = "Generated Map";
        if(transform.Find(mapContainer))
        {
            if(spawnedTiles.Count > 0)
            {
                spawnedTiles.Clear();
            }
            DestroyImmediate(transform.Find(mapContainer).gameObject);
        }

        Transform mapHolder = new GameObject(mapContainer).transform;
        mapHolder.parent = transform;

        // Not sure how to make this work using the property approach for grid size and maxsize etc
        // You might be able to figure it out.
        // Editor script works by calling this function every frame, whilst the script is selected in editor
        // Therefore the string check + Destroy immediate is neccessary to prevent infinite spawning
        for (int i = 0; i < gridWorldSize.x; i++)
        {
            for (int j = 0; j < gridWorldSize.z; j++)
            {
                Vector3 tilePosition = new Vector3((-gridWorldSize.x + 1) / 2 + i, 0.0f, (-gridWorldSize.z + 1) / 2 + j);
                // Quaternion.Euler(Vector3.right * 90) use that if using Quads
                GameObject newTile = (GameObject)Instantiate(Resources.Load("GridPrefabs/GridNode"),tilePosition,Quaternion.identity);
                newTile.transform.localScale = Vector3.one * (1 - outlinePercent);
                newTile.transform.parent = mapHolder;
                //Tile script = newTile.GetComponent<Tile>();
                spawnedTiles.Add(newTile);
            }
        }
    }

    public void SaveTileMap()
    {
        ClearTileMap();
        nodeInformation = GetNodeInformation();
        Vector3 worldBottomLeft = transform.position - (Vector3.right * gridWorldSize.x / 2) - (Vector3.forward * gridWorldSize.z / 2);
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / (nodeRadius * 2));
        gridSizeY = Mathf.RoundToInt(gridWorldSize.z / (nodeRadius * 2));
        Node[,] nodes = new Node[gridSizeX, gridSizeY];
        nodeInformation.nodesData = new Node[gridSizeX, gridSizeY];
        nodeInformation.gridSize = new Coord(gridSizeX, gridSizeY);
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * (nodeRadius * 2) + nodeRadius) + Vector3.forward * (y * (nodeRadius * 2) + nodeRadius);
                Transform _tile = Utility.ShootRaycastTransform(worldPoint + Vector3.up * (Tile.maxWorldTileHeight + 0.5f), Vector3.down, Tile.maxWorldTileHeight + 0.6f, floorLayermask);
                if(_tile != null)
                {
                    Tile tile = _tile.GetComponent<Tile>();
                    nodeInformation.tileHeights.Add(tile.GetHeight());
                    nodeInformation.tileWorldPositionData.Add(tile.transform.position);
                    nodeInformation.nodesData[x, y] = new Node(new Coord(gridSizeX, gridSizeY), tile.GetHeightVector(), true, tile.GetHeight(), null, null);
                }
                else
                {
                    nodeInformation.tileHeights.Add(0);
                    nodeInformation.tileWorldPositionData.Add(Vector3.zero);
                    nodeInformation.nodesData[x, y] = new Node(new Coord(gridSizeX, gridSizeY), worldPoint - Vector3.up * 0.1f, false);
                }
            }
        }
    }

    public void LoadTileMap()
    {
        spawnedTiles.Clear();
        string mapContainer = "Generated Map";
        DestroyImmediate(transform.Find(mapContainer).gameObject);
        Transform mapHolder = new GameObject(mapContainer).transform;
        mapHolder.parent = transform;
        nodeInformation = GetNodeInformation();
        for (int x = 0; x < nodeInformation.tileHeights.Count; x++)
        {
            print(x);
            if (nodeInformation.tileWorldPositionData[x] != Vector3.zero)
            {
                GameObject newTile = (GameObject)Instantiate(Resources.Load("GridPrefabs/GridNode"), nodeInformation.tileWorldPositionData[x], Quaternion.identity);
                newTile.GetComponent<Tile>().SetTile(nodeInformation.tileHeights[x]);
                newTile.transform.parent = mapHolder;
                spawnedTiles.Add(newTile);
            }
        }
    }

    public void ClearTileMap()
    {
        nodeInformation = GetNodeInformation();
        nodeInformation.gridSize = Coord.zero();
        nodeInformation.nodesData = new Node[0, 0];
        nodeInformation.tileHeights.Clear();
        nodeInformation.tileWorldPositionData.Clear();
    }

    NodeInformation GetNodeInformation()
    {
        Scene scene = SceneManager.GetActiveScene();
        return(NodeInformation)Resources.Load("Scriptable Objects/LevelNodeData/WorldNodeData_" + scene.name);
    }

    /*int GetTileHeight(float tileY)
    {
        int height = (int)((0.1f + tileY) * 10); // the extra 0.1 is to make it so that 0 world height = 1 height. {(0.1 + 0) * 10 = 1};
        return height;
    }*/

    public Node GetNodeFromWorldCoOrdinate(Coord coOrdinate)
    {
        int x = coOrdinate.x;
        int z = coOrdinate.z;
        if (x > nodeInformation.gridSize.x - 1|| x < 0 || z > nodeInformation.gridSize.z - 1 || z < 0)
        {
            return null;
        }
        return grid[(x), (z)];
    }

    public Coord GetWorldCoOrdinates(Vector3 position)
    {
        int gridRadius = ((nodeInformation.gridSize.x - 1) / 2);
        int x = (int)position.x + gridRadius;
        int z = (int)position.z + gridRadius;

        return new Coord(x, z);
    }

    public bool InBorderCheck(Coord check)
    {
        int gridRadius = ((nodeInformation.gridSize.x - 1) / 2);
        if (check.x <= gridRadius &&
            check.z <= gridRadius &&
            check.x >= -gridRadius &&
            check.z >= -gridRadius)
        {
            return true;
        }
        return false;
    }
}

