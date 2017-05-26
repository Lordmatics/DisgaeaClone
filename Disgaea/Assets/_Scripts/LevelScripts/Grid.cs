using UnityEngine;
using System.Collections;

public class Grid : MonoBehaviour
{

    public LayerMask floorLayermask;
    public Vector2 gridWorldSize;
    int gridSizeX, gridSizeY;
    public int MaxSize
    {
        get
        {
            return gridSizeX * gridSizeY;
        }
    }
    // This can be static maybe?
    public static float nodeRadius;
    Node[,] grid;

    // I can maybe make this dynamic during edit time if you like
    // + Do we want a scene for each level - as in game level
    void Awake()
    {
        CreateGrid();
    }

    public void CreateGrid()
    {
        //Debug.Log("Grid");
        float nodeDiameter = nodeRadius * 2;
        Vector3 worldBottomLeft = transform.position - (Vector3.right * gridWorldSize.x / 2) - (Vector3.forward * gridWorldSize.y / 2);
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        grid = new Node[gridSizeX, gridSizeY];
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                //Transform tile = ShootRaycastTransform(worldPoint + (Vector3.up * 20), Vector3.down, 25, floorLayermask);
                Transform tile = Utility.ShootRaycastTransform(worldPoint + (Vector3.up * 20), Vector3.down, 25, floorLayermask);
                if (tile != null)
                {
                    Transform canvas = tile.parent.FindChild("Canvas");
                    int height = GetTileHeight(tile.position.y);                   
                    grid[x, y] = new Node(new Coord(x, y), worldPoint, true, height, canvas.GetChild(0).GetComponent<UnityEngine.UI.Image>(), canvas.GetChild(1).GetComponent<UnityEngine.UI.Image>());
                }
                else
                {
                    grid[x, y] = new Node(new Coord(gridSizeX, gridSizeY), worldPoint, false);
                }
            }
        }
    }

    int GetTileHeight(float tileY)
    {
        int height = (int)((0.1f + tileY) * 10); // the extra 0.1 is to make it so that 0 world height = 1 height. {(0.1 + 0) * 10 = 1};
        return height;
    }

    // Can clean this class, by moving multi purpose logic into helper class
    //public Transform ShootRaycastTransform(Vector3 origin, Vector3 direction, int rayLength, LayerMask layermask) // all purpose single raycast method that either returns null or returns the hit object;
    //{
    //    RaycastHit hit;
    //    Physics.Raycast(origin, direction, out hit, rayLength, layermask);
    //    Debug.DrawRay(origin, direction * rayLength, Color.red, 5);
    //    if(hit.transform != null)
    //    {
    //        return hit.transform;
    //    }
    //    else
    //    {
    //        return null;
    //    }
    //}

    //public Vector3 ShootRaycastVector(Vector3 origin, Vector3 direction, int rayLength, LayerMask layermask) // all purpose single raycast method that either returns a Vector3.zero or returns the hit point world space vector;
    //{
    //    RaycastHit hit;
    //    Physics.Raycast(origin, direction, out hit, rayLength, layermask);
    //    Debug.DrawRay(origin, direction * rayLength, Color.red, 5);
    //    if (hit.transform != null)
    //    {
    //        return hit.point;
    //    }
    //    else
    //    {
    //        return Vector3.zero;
    //    }
    //}

    public Node GetNodeFromWorldCoOrdinate(Coord coOrdinate)
    {
        int x = coOrdinate.x;
        int z = coOrdinate.z;
        if (x > gridSizeX - 1|| x < 0 || z > gridSizeY - 1 || z < 0)
        {
            return null;
        }
        return grid[(x), (z)];
    }

    public Coord GetWorldCoOrdinates(Vector3 position)
    {
        int gridRadius = ((gridSizeX - 1) / 2);
        int x = (int)position.x + gridRadius;
        int z = (int)position.z + gridRadius;

        return new Coord(x, z);
    }
}

