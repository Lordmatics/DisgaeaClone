using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NodeInformation : ScriptableObject {

    //public List<Node> nodes = new List<Node>();
    //i'll add later
    // this'll be an asset that is modified while in the editor and used in play time. i think that should work.
    public Coord gridSize;
    public Node[,] nodesData;
    public List<int> tileHeights;
    public List<Vector3> tileWorldPositionData;
}
