﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Scripts/PlayerScripts/PlayerIcon")]
public class PlayerIcon : MonoBehaviour
{
    Grid grid;
    public float moveTimeIncrement;

    public Coord iconCoOrds;
    Camera_Follow cam;
    int currentRotationValue;
    int moveVal;

    int moveKeysPressed;
    public Vector3 direction;

    private void Awake()
    {
        cam = FindObjectOfType<Camera_Follow>();
        grid = FindObjectOfType<Grid>();
    }

    #region INPUT
    #region PRESSED
    void MoveKeyPressed(int val)
    {
        Vector3 dir = GetDirection(val);
        direction += dir;
        if (moveKeysPressed == 0)
        {
            StartCoroutine("MoveIcon");
            return;
        }
        moveKeysPressed++;
    }
    
    void WPressed()
    {
        MoveKeyPressed(GetDirDifference(0));
    }

    void SPressed()
    {
        MoveKeyPressed(GetDirDifference(2));
    }

    void APressed()
    {
        MoveKeyPressed(GetDirDifference(3));
    }

    void DPressed()
    {
        MoveKeyPressed(GetDirDifference(1));
    }
    #endregion
    #region HELD
    #endregion
    #region RELEASED
    void MoveKeyReleased(int val)
    {
        moveKeysPressed--;
        Vector3 dir = GetDirection(val);
        direction -= dir;
    }

    void WReleased()
    {
        int val = GetDirDifference(0);
        MoveKeyReleased(val);
    }

    void SReleased()
    {
        int val = GetDirDifference(2);
        MoveKeyReleased(val);
    }

    void AReleased()
    {
        int val = GetDirDifference(3);
        MoveKeyReleased(val);
    }

    void DReleased()
    {
        int val = GetDirDifference(1);
        MoveKeyReleased(val);
    }
    #endregion
    #endregion

    IEnumerator MoveIcon()
    {
        moveKeysPressed++;
        SetPlayerPos();
        while (moveKeysPressed > 0)
        {
            yield return new WaitForSeconds(moveTimeIncrement);
            SetPlayerPos();
        }
    }

    void SetPlayerPos()
    {
        Vector2 dir = new Vector2(direction.x, direction.z);
        bool canMove = grid.InBorderCheck(iconCoOrds + dir);
        if (canMove)
        {
            transform.position += direction;
            iconCoOrds += dir;
        }
        else if(moveKeysPressed >= 2)
        {
            Vector2 canMoveSingle = grid.AlternateBorderCheck(iconCoOrds + dir, dir);
            print(canMoveSingle);
            if (canMoveSingle != dir && canMoveSingle != Vector2.zero)
            {
                transform.position += new Vector3(canMoveSingle.x, 0, canMoveSingle.y);
                iconCoOrds += canMoveSingle;
            }
        }
    }

    int GetDirDifference(int dir)
    {
        int val = dir + currentRotationValue;
        if (val > 3)
            return val - 4;
        else
            return val;
    }

    Vector3 GetDirection(int dir)
    {

        switch(dir)
        {
            case 0:
                return Vector3.forward;
            case 2:
                return Vector3.back;
            case 3:
                return Vector3.left;
            case 1:
                return Vector3.right;
            default:
                return Vector3.zero;
        }
    }

    void QPressed()
    {
        currentRotationValue = Utility.ClampCycleInt(--currentRotationValue, 0, 3);
    }

    void EPressed()
    {
        currentRotationValue = Utility.ClampCycleInt(++currentRotationValue, 0, 3);
    }

    private void OnEnable()
    {
        InputManager.wPressed += WPressed;
        InputManager.sPressed += SPressed;
        InputManager.aPressed += APressed;
        InputManager.dPressed += DPressed;
        InputManager.qPressed += QPressed;
        InputManager.ePressed += EPressed;

        InputManager.wReleased += WReleased;
        InputManager.sReleased += SReleased;
        InputManager.aReleased += AReleased;
        InputManager.dReleased += DReleased;
    }

    private void OnDisable()
    {
        InputManager.wPressed -= WPressed;
        InputManager.sPressed -= SPressed;
        InputManager.aPressed -= APressed;
        InputManager.dPressed -= DPressed;
        InputManager.qPressed -= QPressed;
        InputManager.ePressed -= EPressed;

        InputManager.wReleased -= WReleased;
        InputManager.sReleased -= SReleased;
        InputManager.aReleased -= AReleased;
        InputManager.dReleased -= DReleased;
    }

















































































    /*public Text UICoOrds;
    public Text UIHeight;
    public Text UIMoveDist;
    public int jump;
    public int move;

    Coord worldCoOrdinates;
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
        UICoOrds.text = "Co-Ords: (" + (worldCoOrdinates.x + 1) + " , " + (worldCoOrdinates.z + 1) + ")";
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
    }*/

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

    /*void MoveInput()
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
            // Might overload += and -= for Coord
            case 0:
                transform.position += Vector3.forward;
                worldCoOrdinates += Coord.Up();
                //worldCoOrdinates.z += 1;
                //worldCoOrdinates += Vector2.up;
                break;
            case 1:
                transform.position += Vector3.right;
                worldCoOrdinates += Coord.Right();
                //worldCoOrdinates.x += 1;
                //worldCoOrdinates += Vector2.right;
                break;
            case 2:
                transform.position += Vector3.back;
                worldCoOrdinates += Coord.Down();
                //worldCoOrdinates.z -= 1;
                //worldCoOrdinates += Vector2.down;
                break;
            case 3:
                transform.position += Vector3.left;
                worldCoOrdinates += Coord.Left();
                //worldCoOrdinates.x -= 1;
                //worldCoOrdinates += Vector2.left;
                break;
        }
        SetUI();
        transform.position = new Vector3(transform.position.x, ((float)currentHeight / 10) - 0.1f, transform.position.z);
    }*/
    /*
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
        Coord algorithmWorldCoords; // a temporary set of Vector2 coOrdinates so that the world coOdrinates don't change.
        int counter = 1;
        SetUpCurrentNode(grid.GetNodeFromWorldCoOrdinate(worldCoOrdinates));
        while (counter <= move) // while loop loops through each segment till it has worked up to move amount of tiles.
        {
            algorithmWorldCoords = worldCoOrdinates + new Coord(0, counter);// ((Vector2.up) * counter); // adjust the starting point of the node checks.
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

    Coord ChangeDirection(int index)
    {
        switch(index)
        {
            case 0:
                return Coord.Up();
            case 1:
                return Coord.Right();
            case 2:
                return Coord.Down();
            case 3:
                return Coord.Left();
        }
        return Coord.Zero();
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

    Coord NewDirection(Coord dir, int index)
    {
        //print("DirectionIndexer: " + index);
        switch (index)
        {
            case 0:
                return dir + Coord.SE();
            case 1:
                return dir + Coord.SW();
            case 2:
                return dir + Coord.NW();
            case 3:
                return dir + Coord.NE();
            default:
                return dir;
        }
    }

    Node GetSideNode(Coord currNodePos, int index)
    {
        Node node;
        Coord pos;
        //print("Current Node Pos???: " + currNodePos);
        //print(currNodePos + new Vector2(0, -1));
        switch (index)
        {
            case 0:
                pos = currNodePos + Coord.Down();// new Vector2(0, -1);
                node = grid.GetNodeFromWorldCoOrdinate(pos);
                //print("results: " + node.worldCoOrdinates);
                return node;
            case 1:
                pos = currNodePos + Coord.Left();// new Vector2(-1, 0);
                node = grid.GetNodeFromWorldCoOrdinate(pos);
                return node;
            case 2:
                pos = currNodePos + Coord.Right();// new Vector2(0, 1);
                node = grid.GetNodeFromWorldCoOrdinate(pos);
                return node;
            case 3:
                pos = currNodePos + Coord.Up();// new Vector2(1, 0);
                node = grid.GetNodeFromWorldCoOrdinate(pos);
                return node;
            default:
                return null;
        }
    }

    List<Node> GetSideNodes(Coord currNodePos, int index)
    {
        List<Node> nodes = new List<Node>();
        Coord pos;
        switch (index)
        {
            case 0:
                pos = currNodePos + Coord.Down();// new Vector2(0, -1);
                nodes.Add(grid.GetNodeFromWorldCoOrdinate(pos));
                pos = currNodePos + Coord.Left();// new Vector2(-1, 0);
                nodes.Add(grid.GetNodeFromWorldCoOrdinate(pos));
                break;
            case 1:
                pos = currNodePos + Coord.Left();// new Vector2(-1, 0);
                nodes.Add(grid.GetNodeFromWorldCoOrdinate(pos));
                pos = currNodePos + Coord.Up();// new Vector2(0, 1);
                nodes.Add(grid.GetNodeFromWorldCoOrdinate(pos));
                break;
            case 2:
                pos = currNodePos + Coord.Up();// new Vector2(0, 1);
                nodes.Add(grid.GetNodeFromWorldCoOrdinate(pos));
                pos = currNodePos + new Vector2(1, 0);
                nodes.Add(grid.GetNodeFromWorldCoOrdinate(pos));
                break;
            case 3:
                pos = currNodePos + Coord.Right();// new Vector2(1, 0);
                nodes.Add(grid.GetNodeFromWorldCoOrdinate(pos));
                pos = currNodePos + Coord.Down();// new Vector2(0, -1);
                nodes.Add(grid.GetNodeFromWorldCoOrdinate(pos));
                break;
            default:
                return nodes;
        }
        return nodes;
    }
    #endregion
    */
}
