using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSelectionEditor : MonoBehaviour {

    public List<Transform> tiles = new List<Transform>();
    public LayerMask layermask;

    public int startX;
    public int startZ;

    public int endX;
    public int endZ;

    public int amountX;
    public int amountZ;

    public float fstartX;
    public float fstartZ;

    public float fendX;
    public float fEndZ;

    private void UpdateTiles()
    {
        tiles.Clear();
        fstartX = transform.position.x - (transform.localScale.x / 2f);
        startX = Mathf.RoundToInt(fstartX);
        fstartZ = transform.position.z - (transform.localScale.z / 2f);
        startZ = Mathf.RoundToInt(fstartZ);

        fendX = transform.position.x + (transform.localScale.x / 2f);
        endX = Mathf.RoundToInt(fendX);
        fEndZ = transform.position.z + (transform.localScale.z / 2f);
        endZ = Mathf.RoundToInt(fEndZ);

        amountX = Mathf.RoundToInt(Vector3.Distance(new Vector3(startX, 0, 0), new Vector3(endX, 0, 0))) + 1;
        amountZ = Mathf.RoundToInt(Vector3.Distance(new Vector3(startZ, 0, 0), new Vector3(endZ, 0, 0))) + 1;
        int i = startX;
        int j = startZ;
        for (int x = 0; x < amountX; x++)
        {
            for (int y = 0; y < amountZ; y++)
            {
                RaycastHit hit;
                Ray ray = new Ray(new Vector3(i, transform.position.y - transform.localScale.y * 2, j), Vector3.up);
                Physics.Raycast(ray, out hit, 200, layermask);
                //Debug.DrawRay(ray.origin, ray.direction * 15f, Color.cyan, 0.1f);
                //print(hit.transform);
                if (hit.transform != null)
                {
                    if(hit.transform.GetComponent<Tile>().currentlySelected == false)
                        tiles.Add(hit.transform);
                }
                if (startZ > endZ)
                    j -= 1;
                else
                    j += 1;
            }
            j = startZ;
            if (startX > endX)
                i -= 1;
            else
                i += 1;
        }
    }

    public void SetPositionAndScale(Vector3 corner_01, Vector3 corner_02)
    {
        //print(corner_01 + "______==========______" + corner_02);


        float distanceX = Vector3.Distance(new Vector3(corner_01.x, 0, 0), new Vector3(corner_02.x, 0, 0));
        float distanceZ = Vector3.Distance(new Vector3(0, 0, corner_01.z), new Vector3(0, 0, corner_02.z));

        float posX;
        if (corner_01.x > corner_02.x)
            posX = corner_01.x - (distanceX / 2f);
        else
            posX = corner_02.x - (distanceX / 2f);

        float posZ;
        if (corner_01.z > corner_02.z)
            posZ = corner_01.z - (distanceZ / 2f);
        else
            posZ = corner_02.z - (distanceZ / 2f);

        transform.position = new Vector3(posX, 0.5f, posZ);

        transform.localScale = new Vector3(distanceX, 1f, distanceZ);
        if (corner_01 != corner_02)
            UpdateTiles();
        else
            tiles.Clear();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(new Vector3(startX, transform.position.y, startZ), 0.1f);
        Gizmos.DrawSphere(new Vector3(endX, transform.position.y, startZ), 0.1f);
        Gizmos.DrawSphere(new Vector3(startX, transform.position.y, endZ), 0.1f);
        Gizmos.DrawSphere(new Vector3(endX, transform.position.y, endZ), 0.1f);

        Gizmos.color = Color.red;

        foreach (var t in tiles)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(t.position + Vector3.up * 0.7f, new Vector3(0.8f, 0.2f, 0.8f));
        }
    }
}
