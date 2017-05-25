using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PlayerIcon : MonoBehaviour
{

    public Text UICoOrds;
    public Text UIHeight;
    public Text UIMoveDist;
    public int jump;
    public int move;

    Vector2 worldCoOrdinates;
    int currentHeight;
    Grid grid;

    //TurnManager turnManager;
    public delegate void Action();
    public Action action;

    void Start ()
    {
        //turnManager = FindObjectOfType<TurnManager>();
        grid = FindObjectOfType<Grid>();
        worldCoOrdinates = grid.GetWorldCoOrdinates(transform.position);
        SetUI();
        action = Test;
    }

    void Update()
    {
        MoveInput();
        CameraInput();
        if (Input.GetKeyDown(KeyCode.Return))
        {
            GenerateMoveZone();
        }
    }

    public void Test()
    {
        print("TestWorked");
    }

    void SetUI()
    {
        currentHeight = grid.GetNodeFromWorldCoOrdinate(worldCoOrdinates).tileHeight;
        UICoOrds.text = "Co-Ords: (" + (worldCoOrdinates.x + 1) + " , " + (worldCoOrdinates.y + 1) + ")";
        UIHeight.text = "Height: " + currentHeight;
        Node node = grid.GetNodeFromWorldCoOrdinate(worldCoOrdinates);
        UIMoveDist.text = "MoveDist: " + node.moveDist;
    }

    int rotation = 0;
    void CameraInput()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            rotation = SetRotation(--rotation);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            rotation = SetRotation(++rotation);
        }
    }

    int SetRotation(int _rotation)
    {
        if (_rotation > 3)
            return 0;
        else if (_rotation < 0)
            return 3;
        else
            return _rotation;
    }

   /* void FixedUpdate()
    {
        JoystickMoveIcon();
    }

    Coroutine up;
    Coroutine right;
    Coroutine down;
    Coroutine left;

    void JoystickMoveIcon()
    {
        if (Input.GetAxis("Vertical") > 0)
        {
            if(up == null)
                up = StartCoroutine(MoveIconIE(rotation, Input.GetAxis("Vertical")));
        }
        if (Input.GetAxis("Horizontal") > 0)
        {
            if(right == null)
                right = StartCoroutine(MoveIconIE(rotation + 1, Input.GetAxis("Vertical")));
        }
        if (Input.GetAxis("Vertical") < 0)
        {
            if(down == null)
                down = StartCoroutine(MoveIconIE(rotation + 2, Input.GetAxis("Vertical")));
        }
        if (Input.GetAxis("Horizontal") < 0)
        {
            if(left == null)
                left = StartCoroutine(MoveIconIE(rotation + 3, Input.GetAxis("Vertical")));
        }
    }

    IEnumerator MoveIconIE(int _rotation, float axis)
    {
        while(axis != 0)
        {
            if (_rotation > 3)
                _rotation -= 4;
            switch (_rotation)
            {
                case 0:
                    transform.position += Vector3.forward;
                    worldCoOrdinates += Vector2.up;
                    break;
                case 1:
                    transform.position += Vector3.right;
                    worldCoOrdinates += Vector2.right;
                    break;
                case 2:
                    transform.position += Vector3.back;
                    worldCoOrdinates += Vector2.down;
                    break;
                case 3:
                    transform.position += Vector3.left;
                    worldCoOrdinates += Vector2.left;
                    break;
            }
            SetUI();
            transform.position = new Vector3(transform.position.x, ((float)currentHeight / 10) - 0.1f, transform.position.z);
            yield return new WaitForSeconds(0.5f);
        }
        if(Input.GetAxis("Horizontal") == 0)
        {
            right = null;
            left = null;
        }
        if (Input.GetAxis("Vertical") == 0)
        {
            up = null;
            down = null;
        }
    }*/

    void MoveInput()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            MoveIcon(rotation);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            MoveIcon(rotation + 1);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            MoveIcon(rotation + 2);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            MoveIcon(rotation + 3);
        }
    }

    void MoveIcon(int _rotation)
    {
        if (_rotation > 3)
            _rotation -= 4;
        switch(_rotation)
        {
            case 0:
                transform.position += Vector3.forward;
                worldCoOrdinates += Vector2.up;
                break;
            case 1:
                transform.position += Vector3.right;
                worldCoOrdinates += Vector2.right;
                break;
            case 2:
                transform.position += Vector3.back;
                worldCoOrdinates += Vector2.down;
                break;
            case 3:
                transform.position += Vector3.left;
                worldCoOrdinates += Vector2.left;
                break;
        }
        SetUI();
        transform.position = new Vector3(transform.position.x, ((float)currentHeight / 10) - 0.1f, transform.position.z);
    }

    #region TEST CODE

    // To find the amount of tiles to check i found out how much is in 1 quarter by doing k + n, k + (n + 1), k + (n + 2) ... k + (n + move).
    // this gets the total tiles in 1 quadrant, you then simply times it by 4 and add 1 to include the center node. 
    int GetTotalMovementTiles(int move)
    {
        int toReturn = 0;
        for (int i = 1; i <= move; i++)
        {
            toReturn += i;
        }
        return toReturn * 4;
    }

    List<Node> moveableNodes = new List<Node>();
    // To generate a movement zone that takes height into consideration, you have to check the tiles from
    // the inside where the player is to the outside using the move distance as the generation limit.

    void RefreshTiles()
    {
        if(moveableNodes.Count > 0)
        {
            for (int x = 0; x < moveableNodes.Count; x++)
            {
                moveableNodes[x].ResetNode();
            }
        }
    }

    void GenerateMoveZone()
    {
        //TEMP
        RefreshTiles();
        List<Node> nodesNotInRadius = new List<Node>();
        List<Node> nodesToCheck = new List<Node>();
        Node nodeToCheck;
        Node currentNode = grid.GetNodeFromWorldCoOrdinate(worldCoOrdinates);
        int maxNodes = GetTotalMovementTiles(move); // calculates total tile count for current move distance.
        int tilesPerQuadrant = 1; // this is for a loop that checks a quadrant.
        Vector2 algorithmWorldCoords; // a temporary set of Vector2 coOrdinates so that the world coOdrinates don't change.
        int counter = 1;
        SetUpCurrentNode(grid.GetNodeFromWorldCoOrdinate(worldCoOrdinates));
        while (counter <= move) // while loop loops through each segment till it has worked up to move amount of tiles.
        {
            algorithmWorldCoords = worldCoOrdinates + ((Vector2.up) * counter); // adjust the starting point of the node checks.
            for (int i = 0; i < 4; i++) // 4 represents quadrants.
            {
                for (int j = 0; j < tilesPerQuadrant; j++)
                {
                    //print("Co-Ordinates: " + algorithmWorldCoords);
                    currentNode = grid.GetNodeFromWorldCoOrdinate(algorithmWorldCoords); // current node that needs to check between previously checked nodes adjacent to it.
                    if(!IsWorkingNode(currentNode)) 
                    {
                        algorithmWorldCoords = NewDirection(algorithmWorldCoords, i); // set the co-ords to the next location.
                        continue; // then continue onto the next loop to skip all the code underneath as it is not neccassary.
                    }
                    currentNode.moveDist = counter; // give the tile a moveDistance for reference.
                    if (j == 0 || counter == 1) // is a tile that is only adjacent to 1 tile to check.
                    {
                        //print("Single Check");
                        nodeToCheck = GetSideNode(currentNode.worldCoOrdinates, i); // i will always represent the correct index for finding adjacent nodes as i represents each quarter.
                        //print("I: " + i + "  curN Co-Ord: " + currentNode.worldCoOrdinates + "  chkN Co-Ord: " + nodeToCheck.worldCoOrdinates);
                        if(IsHeightCompatable(currentNode, nodeToCheck) == true)
                        {
                            //print("HeightCheckSuccessful");
                            SetUpCurrentNode(currentNode);
                            algorithmWorldCoords = NewDirection(algorithmWorldCoords, i); // only need to do this here so that the next node check is in the right direction
                            continue;
                        }
                    }
                    else // is a tile that is adjacent to 2 tiles to check
                    {
                        //print("Double Check");
                        nodesToCheck = GetSideNodes(currentNode.worldCoOrdinates, i); // need to check 2 tiles, so doing that through a loop and stopping when finding a valid node.
                        bool canLeave = false;
                        for (int k = 0; k < nodesToCheck.Count; k++) 
                        {
                            if (IsHeightCompatable(currentNode, nodesToCheck[k]) == true)
                            {
                                SetUpCurrentNode(currentNode);
                                algorithmWorldCoords = NewDirection(algorithmWorldCoords, i);
                                canLeave = true;
                                break;
                            }
                        }
                        if (canLeave)
                            continue; // had to use a temp bool here so that i used the continue keyword on the right for loop, (if i continued in the 'k' forloop, the 'j' forloop would still run, not what i want.
                    }
                    nodesNotInRadius.Add(currentNode); // this list is used to fix any gaps in the valid nodes that may have been missed because of some of the nodes.
                    algorithmWorldCoords = NewDirection(algorithmWorldCoords, i);
                }
            }
            tilesPerQuadrant++;
            counter++;
        }
        FindLeftoverTiles(nodesNotInRadius, true);
    }

    void FindLeftoverTiles(List<Node> nodes, bool run)
    {
        List<Node> nodesNotInRadius = new List<Node>();
        print(nodes.Count);
        for (int i = 0; i < nodes.Count; i++)
        {
            bool found = false;
            for (int j = 0; j < 4 ; j++)
            {
                Node nodeToCheck = grid.GetNodeFromWorldCoOrdinate(nodes[i].worldCoOrdinates + ChangeDirection(j));
                if (IsWorkingNode(nodeToCheck) == true)
                {
                    if (IsHeightCompatable(nodes[i], nodeToCheck))
                    {
                        if (nodeToCheck.moveDist < move)
                        {
                            found = true;
                            SetUpCurrentNode(nodes[i]);
                            nodes[i].moveDist = nodeToCheck.moveDist + 1;
                            moveableNodes.Add(nodes[i]);
                        }
                        break;
                    }
                }
            }
            if (found == false)
                nodesNotInRadius.Add(nodes[i]);
        }
        if (run == true)
            FindLeftoverTiles(nodesNotInRadius, false);//Final Check just to find any spaces left with gaps because of high walls etc.
    }

    Vector2 ChangeDirection(int index)
    {
        switch(index)
        {
            case 0:
                return Vector2.up;
            case 1:
                return Vector2.right;
            case 2:
                return Vector2.down;
            case 3:
                return Vector2.left;
        }
        return Vector2.zero;
    }

    void SetUpCurrentNode(Node currentNode)
    {
        currentNode.isMoveable = true;
        currentNode.SetCombatTile(0);
        moveableNodes.Add(currentNode);
    }

    bool IsWorkingNode(Node node) // checks to see if the node is null or not, and if it is not null, checks to see if it is a walkable node or not. returns whether or not the node is walkable or not.
    {
        //print(node);
        if(node != null)
        {
            //print("node is not null");
            if (node.isWalkable)
            {
                return true;
            }
            else
                return false;
        }
        else
            return false;
    }

    bool IsHeightCompatable(Node currentNode, Node nodeToCheck)
    {
        if (nodeToCheck.isMoveable)
        {
            //print("is moveable");
            if (currentNode.tileHeight < nodeToCheck.tileHeight + jump)
            {
                return true;
            }
            else
                return false;
        }
        else
            return false;
    }

    /*void GenerateMoveZone()
    {
        // nodes that are valid tiles that couldn't be set active the first time are added here to be checked a second times later.
        List<Node> nodesNotInRadius = new List<Node>();
        // comparison nodes to check for things like height or whether the surrounding tiles are walkable or not.
        List<Node> nodesToCheck = new List<Node>();
        //Node currNode = TGetNode(worldCoOrdinates, grid);
        Node currNode = grid.GetNodeFromWorldCoOrdinate(worldCoOrdinates);
        int maxNodes = GetTotalMovementTiles(6); // calculates toal tile count for current move distance.
        int loopCount = 4; // this just means each time the loop runs it checks 1 quadrant.
        int embeddedIndex = 2; // this is for a loop that checks a quadrant.
        Vector2 algorithmWorldCoords = worldCoOrdinates + Vector2.up;
        Vector2 direction = Vector2.zero;
        int dist = 1;
        for (int i = 0; i < loopCount; i++) // to generate the first ring of tiles.
        {
            currNode = grid.GetNodeFromWorldCoOrdinate(algorithmWorldCoords);
            if (currNode == null || !currNode.isWalkable)
            {
                algorithmWorldCoords = NewDirection(algorithmWorldCoords, i);
                continue;
            }
            // uses current co-ords and loop index to get a specific node adjacent to the current node.
            Node nodeToCheck = GetSideNode(algorithmWorldCoords, i);
            // calculates new position for the co-ords relevant for the algorithm.
            algorithmWorldCoords = NewDirection(algorithmWorldCoords, i);
            // tell the tile its moveDist = dist.
            currNode.moveDist = dist;

                currNode.isMoveable = true;
                currNode.SetCombatTile(0);
                moveableNodes.Add(currNode);
            }
            else // add to list for future checking.
            {
                nodesNotInRadius.Add(currNode);
                //currNode.blueTile.enabled = true;
            }
            // tile dist = 1 each;
        }
        int counter = 2; // this just allows the while loop to stop and never go on forever
        while (counter <= 6)
        {
            algorithmWorldCoords = worldCoOrdinates + ((Vector2.up) * counter); // set the startPos to be the next ring of tiles up.
            dist++; // as ech tile is 1 further away from the player, increase dist by 1.
            for (int i = 0; i < loopCount; i++)
            {
                for (int j = 0; j < embeddedIndex; j++) // <-- embedded index == 2 meaning j== 0 (1 node to check), j == 1 (2 node to check) end loop. 
                {
                    currNode = grid.GetNodeFromWorldCoOrdinate(algorithmWorldCoords);
                    if (currNode == null || !currNode.isWalkable)
                    {
                        algorithmWorldCoords = NewDirection(algorithmWorldCoords, i);
                        continue;
                    }
                    currNode.moveDist = dist;
                    if (j == 0)
                    {
                        Node nodeToCheck = GetSideNode(algorithmWorldCoords, i);
                        if (nodeToCheck == null)
                        {
                            nodesNotInRadius.Add(currNode);
                            //currNode.blueTile.enabled = true;
                            continue;
                        }
                        else if (!nodeToCheck.isWalkable)
                        {
                            //currNode.blueTile.enabled = true;
                            continue;
                        }
                        if (currNode.isWalkable)
                        {
                            if (currNode.tileHeight < nodeToCheck.tileHeight + jump && nodeToCheck.isMoveable)
                            {
                                currNode.isMoveable = true;
                                currNode.SetCombatTile(0);
                                moveableNodes.Add(currNode);
                            }
                            else
                            {
                                nodesNotInRadius.Add(currNode);
                                //currNode.blueTile.enabled = true;
                            }
                        }
                    }
                    else
                    {
                        nodesToCheck = GetSideNodes(algorithmWorldCoords, i);
                        for (int k = 0; k < 2; k++)
                        {
                            if (nodesToCheck[k] == null || !nodesToCheck[k].isWalkable)
                                continue;
                            if (currNode.isWalkable && nodesToCheck[k].isWalkable)
                            {
                                if (currNode.tileHeight < nodesToCheck[k].tileHeight + jump && nodesToCheck[k].isMoveable)
                                {
                                    currNode.isMoveable = true;
                                    currNode.SetCombatTile(0);
                                    moveableNodes.Add(currNode);
                                    break;
                                }
                                else
                                {
                                    if (!nodesNotInRadius.Contains(currNode))
                                    {
                                        nodesNotInRadius.Add(currNode);
                                        //currNode.blueTile.enabled = true;
                                    }
                                }
                            }
                        }
                    }
                    algorithmWorldCoords = NewDirection(algorithmWorldCoords, i); // get new co-ords in the current ring
                }
            }
            counter++;
            embeddedIndex++;
        }
        nodesNotInRadius.Reverse();
        for (int i = 0; i < nodesNotInRadius.Count; i++)
        {
            Node singleCheckNode;
            Vector2 nodeCoords = new Vector2(nodesNotInRadius[i].gridX, nodesNotInRadius[i].gridY);
            for (int j = 0; j < 4; j++)
            {
                singleCheckNode = GetSideNode(nodeCoords, j);
                if (singleCheckNode != null)
                {
                    if (singleCheckNode.isWalkable && singleCheckNode.isMoveable && singleCheckNode.moveDist <= 5) // 5 = move count - 1
                    {
                        if (nodesNotInRadius[i].tileHeight < singleCheckNode.tileHeight + jump)
                        {
                            nodesNotInRadius[i].isMoveable = true;
                            moveableNodes.Add(currNode);
                            nodesNotInRadius[i].SetCombatTile(0);
                            break;
                        }
                    }
                }
            }
        }
        //print("Stuff just happened");
    }*/

    Vector2 NewDirection(Vector2 dir, int index)
    {
        //print("DirectionIndexer: " + index);
        switch (index)
        {
            case 0:
                return dir + new Vector2(1, -1);
            case 1:
                return dir + new Vector2(-1, -1);
            case 2:
                return dir + new Vector2(-1, 1);
            case 3:
                return dir + new Vector2(1, 1);
            default:
                return dir;
        }
    }

    Node GetSideNode(Vector2 currNodePos, int index)
    {
        Node node;
        Vector2 pos;
        //print("Current Node Pos???: " + currNodePos);
        //print(currNodePos + new Vector2(0, -1));
        switch (index)
        {
            case 0:
                pos = currNodePos + new Vector2(0, -1);
                node = grid.GetNodeFromWorldCoOrdinate(pos);
                //print("results: " + node.worldCoOrdinates);
                return node;
            case 1:
                pos = currNodePos + new Vector2(-1, 0);
                node = grid.GetNodeFromWorldCoOrdinate(pos);
                return node;
            case 2:
                pos = currNodePos + new Vector2(0, 1);
                node = grid.GetNodeFromWorldCoOrdinate(pos);
                return node;
            case 3:
                pos = currNodePos + new Vector2(1, 0);
                node = grid.GetNodeFromWorldCoOrdinate(pos);
                return node;
            default:
                return null;
        }
    }

    List<Node> GetSideNodes(Vector2 currNodePos, int index)
    {
        List<Node> nodes = new List<Node>();
        Vector2 pos;
        switch (index)
        {
            case 0:
                pos = currNodePos + new Vector2(0, -1);
                nodes.Add(grid.GetNodeFromWorldCoOrdinate(pos));
                pos = currNodePos + new Vector2(-1, 0);
                nodes.Add(grid.GetNodeFromWorldCoOrdinate(pos));
                break;
            case 1:
                pos = currNodePos + new Vector2(-1, 0);
                nodes.Add(grid.GetNodeFromWorldCoOrdinate(pos));
                pos = currNodePos + new Vector2(0, 1);
                nodes.Add(grid.GetNodeFromWorldCoOrdinate(pos));
                break;
            case 2:
                pos = currNodePos + new Vector2(0, 1);
                nodes.Add(grid.GetNodeFromWorldCoOrdinate(pos));
                pos = currNodePos + new Vector2(1, 0);
                nodes.Add(grid.GetNodeFromWorldCoOrdinate(pos));
                break;
            case 3:
                pos = currNodePos + new Vector2(1, 0);
                nodes.Add(grid.GetNodeFromWorldCoOrdinate(pos));
                pos = currNodePos + new Vector2(0, -1);
                nodes.Add(grid.GetNodeFromWorldCoOrdinate(pos));
                break;
            default:
                return nodes;
        }
        return nodes;
    }
    #endregion
}
