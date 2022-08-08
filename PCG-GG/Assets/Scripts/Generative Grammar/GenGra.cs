using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GenGra : MonoBehaviour
{
    [Serializable]
    public class ProductionNode
    {
        public string LeftHand;
        public string[] RightHand;
    }

    [Serializable]
    public class ProductionNodeList
    {
        public ProductionNode[] productionNodes;
    }

    [Serializable]
    public class MovementNode
    {
        public string LeftHand;
        public string[] RightHand;
    }

    [Serializable]
    public class MovementNodeList
    {
        public MovementNode[] movementNodes;
    }

    public List<Node> GraphNodes = new List<Node>();

    string productionPath;
    string movementPath;

    public ProductionNodeList productionNodeList = new ProductionNodeList();

    public MovementNodeList movementNodeList = new MovementNodeList();

    private int numberOfNodes = 1;

    private System.Random rnd = new System.Random();

    private void Awake()
    {
        productionPath = File.ReadAllText(Application.streamingAssetsPath + "/ProductionNode.json");
        productionNodeList = JsonUtility.FromJson<ProductionNodeList>(productionPath);

        movementPath = File.ReadAllText(Application.streamingAssetsPath + "/MovementNode.json");
        movementNodeList = JsonUtility.FromJson<MovementNodeList>(movementPath);

        Node Start = new Node("Start", numberOfNodes);
        GraphNodes.Add(Start);
        ExpandNode();
    }

    // Start is called before the first frame update
    //void Start()
    //{
    //    productionPath = File.ReadAllText(Application.streamingAssetsPath + "/ProductionNode.json");
    //    productionNodeList = JsonUtility.FromJson<ProductionNodeList>(productionPath);

    //    movementPath = File.ReadAllText(Application.streamingAssetsPath + "/MovementNode.json");
    //    movementNodeList = JsonUtility.FromJson<MovementNodeList>(movementPath);

    //    Node Start = new Node("Start", numberOfNodes);
    //    GraphNodes.Add(Start);
    //    ExpandNode();
        
    //}

    private void ExpandNode()
    {
        //TODO
        //filter possible production nodes +++++++++++++++
        //loop though each node +++++++++++++++
        //check if node matches with production node +++++++++++++++++
        //if true, create nodes based on the corresponding node +++++++++++++++
        //establish connection between the new nodes ++++++++++++++++++++++
        //add previous node to the first node and connecting node to the last node from the node that is being replaced if any


        //var rnd = new System.Random();

        List<string> tempStringGraph = TempGraph();

        List<Node> tempGraphNode = new List<Node>();

        ProductionNode pick = Array.Find(productionNodeList.productionNodes, findNode => tempStringGraph.Contains(findNode.LeftHand)); 
        //got the possible production nodes

        if (pick != null)
        {
            for(var i = 0; i < GraphNodes.Count; i++)
            {
                if (pick.LeftHand.Equals(GraphNodes[i].LeftHand)) 
                {
                    if (pick.RightHand.Length > 1)
                    {
                        int r = rnd.Next(0, pick.RightHand.Length);
                        String[] splitGraph = pick.RightHand[r].Split('-');
                        for (var j = 0; j < splitGraph.Length; j++)
                        {
                            numberOfNodes++;

                            if (splitGraph.Length > 2)
                            {
                                if (j == 0) // at the start of the loop, inherit the previous nodes of the orignal node
                                {
                                    tempGraphNode.Add(new Node(splitGraph[j], numberOfNodes, GraphNodes[i].PreviousNodes));
                                    AddMovementNode(tempGraphNode.Last());
                                }
                                else
                                {
                                    tempGraphNode.Add(new Node(splitGraph[j], numberOfNodes));
                                    AddMovementNode(tempGraphNode.Last());
                                }
                            } 
                            else
                            {
                                tempGraphNode.Add(new Node(splitGraph[j], numberOfNodes));
                                AddMovementNode(tempGraphNode.Last());
                            }
                           
                        }
                        tempGraphNode = ConnectNodes(tempGraphNode, splitGraph.Length, i);
                    }
                    else
                    {
                        String[] splitGraph = pick.RightHand[0].Split('-');
                        for (var j = 0; j < splitGraph.Length; j++)
                        {
                            numberOfNodes++;
                            if(splitGraph.Length > 2)
                            {
                                if (j == 0) // at the start of the loop, inherit the previous nodes of the orignal node
                                {
                                    Debug.Log(splitGraph[j] + " : " + j.ToString());
                                    tempGraphNode.Add(new Node(splitGraph[j], numberOfNodes, GraphNodes[i].PreviousNodes));
                                    AddMovementNode(tempGraphNode.Last());
                                }
                                else
                                {
                                    tempGraphNode.Add(new Node(splitGraph[j], numberOfNodes));
                                    AddMovementNode(tempGraphNode.Last());
                                }
                            } 
                            else 
                            {
                                tempGraphNode.Add(new Node(splitGraph[j], numberOfNodes));
                                AddMovementNode(tempGraphNode.Last());
                            }
                            
                        }
                        tempGraphNode = ConnectNodes(tempGraphNode, splitGraph.Length, i);
                    }               
                }
                else
                {
                    tempGraphNode.Add(new Node(GraphNodes[i].LeftHand, GraphNodes[i].Id, GraphNodes[i].PreviousNodes)); // Remake the node in the temp graph
                    AddMovementNode(tempGraphNode.Last());
                }
            }
            GraphNodes = tempGraphNode; // the temp graph replaces the original graph
            //Logging();
            ExpandNode();
        } else
        {
            //Logging();
        }
    }

    private void Logging()
    {
        string logged = "";
        foreach (Node node in GraphNodes)
        {
            logged = logged + "-" + node.LeftHand;
        }
        Debug.Log(logged);
    }

    private List<string> TempGraph()
    {
        List<string> tempGraph = new List<string>();

        foreach(Node node in GraphNodes)
        {
            tempGraph.Add(node.LeftHand);
        }
        return tempGraph;
    }

    private List<Node> ConnectNodes(List<Node> tempGraphNode, int amountOfNodes, int insertIndex )
    {
        for(var i = insertIndex; i < amountOfNodes + insertIndex; i++)
        {
            //Connect previous nodes
            if(i - 1 >= 0) //Validity check
            {
                if(!tempGraphNode[i].PreviousNodes.Any(node => node.Id.Equals(tempGraphNode[i].Id) || node.Id.Equals(node.Id)))
                {
                    tempGraphNode[i].PreviousNodes.Add(tempGraphNode[i - 1]);
                }
            }
        }
        return tempGraphNode;
    }


    private void AddMovementNode(Node node)
    {
        if (node.MovementGraph.Count.Equals(0))
        {
            switch (node.LeftHand)
            {
                case "key":
                    node.MovementGraph.Add("Start");
                    ExpandMovementNode(node);
                    break;
                case "lock":
                    node.MovementGraph.Add("Start");
                    ExpandMovementNode(node);
                    break;
                case "fight":
                    node.MovementGraph.Add("Start");
                    ExpandMovementNode(node);
                    break;
                case "boss":
                    node.MovementGraph.Add("Start");
                    ExpandMovementNode(node);
                    break;
            }
        }
    }

    private void ExpandMovementNode(Node node)
    {
        
        MovementNode pick = Array.Find(movementNodeList.movementNodes, findNode => node.MovementGraph.Contains(findNode.LeftHand));
        List<string> tempMovementGraph = new List<string>();

        if (pick != null)
        {
            for (var i = 0; i < node.MovementGraph.Count; i++)
            {
                if (pick.LeftHand.Equals(node.MovementGraph[i]))
                {
                    //Debug.Log("got in here " + node.MovementGraph[i]);
                    if (pick.RightHand.Length > 1)
                    {
                        int r = rnd.Next(0, pick.RightHand.Length);
                        String[] splitRightHand = pick.RightHand[r].Split('-');
                        for(var j = 0; j < splitRightHand.Length; j++)
                        {
                            tempMovementGraph.Add(splitRightHand[j]);
                        }
                    }
                    else
                    {
                        tempMovementGraph.Add(pick.RightHand[0]);
                    }
                }
                else
                {
                    tempMovementGraph.Add(node.MovementGraph[i]);
                }
            }
            node.MovementGraph = tempMovementGraph;
            ExpandMovementNode(node);
        }
        else
        {
            Debug.Log(node.MovementGraph);
        }
    }
}






