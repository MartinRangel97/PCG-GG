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

    public GameObject Player;

    private int numberOfNodes = 1;
    private System.Random rnd = new System.Random();
    private string graph;

    private int jumps;
    private int dashes;
    private int glides;

    public string movementGraph;

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

    private void ExpandNode()
    {
        string tempStringGraph = NewTempGraph();

        List<Node> tempGraphNode = new List<Node>();

        ProductionNode pick = Array.Find(productionNodeList.productionNodes, findNode => tempStringGraph.Contains(findNode.LeftHand));
        //got the possible production nodes

        if (pick != null)
        {
            if (pick.RightHand.Length > 1)
            {
                int r = rnd.Next(pick.RightHand.Length);
                PlotNodes(pick.LeftHand, pick.RightHand[r]);
                ExpandNode();
            }
            else
            {
                PlotNodes(pick.LeftHand, pick.RightHand[0]);
                ExpandNode();
            }
        }
        else
        {
            //Debug.Log(graph);
        }
    }

    private void PlotNodes(string pickLeftHand, string pickRightHand)
    {
        string[] splitPickLeftHand = pickLeftHand.Split('-');
        string[] splitPickRightHand = pickRightHand.Split('-');
        bool leftHandMatched = false;
        int splitNodeId = numberOfNodes;

        for (var i = 0; i < GraphNodes.Count; i++)
        {
            if(splitPickLeftHand.Length > 1)
            {

                for(var j = 0; j < splitPickLeftHand.Length; j++) //Checks if the lefthand matches
                {
                    if(GraphNodes[i + j].LeftHand.Equals(splitPickLeftHand[j]))
                        leftHandMatched = true;
                    else
                    {
                        leftHandMatched = false;
                        break;
                    }              
                }

                if (leftHandMatched)
                {
                    for(var j = 1; j < splitPickLeftHand.Length; j++) // will delete 
                        GraphNodes.RemoveAt(i + j);

                    for (var k = 0; k < splitPickRightHand.Length; k++) //reverse for loop
                    {
                        numberOfNodes++;
                        if (splitPickRightHand[k].Equals("split"))
                            splitNodeId = numberOfNodes;

                        if (k == 0)
                        {
                            if (splitPickRightHand[k].Equals("backToSplit"))
                            {
                                Debug.Log(numberOfNodes);
                                GraphNodes.Insert(i + 1 + k, new Node(splitPickRightHand[k], splitNodeId, GraphNodes[i].PreviousNodes));
                            }
                            else 
                                GraphNodes.Insert(i + 1 + k, new Node(splitPickRightHand[k], numberOfNodes, GraphNodes[i].PreviousNodes));
                            AddMovementNode(GraphNodes[i + 1]);
                        }
                        else
                        {
                            if(splitPickRightHand[k].Equals("backToSplit"))
                                GraphNodes.Insert(i + 1 + k, new Node(splitPickRightHand[k], splitNodeId)   );
                            else
                                GraphNodes.Insert(i + 1 + k, new Node(splitPickRightHand[k], numberOfNodes));
                            AddMovementNode(GraphNodes[i + 1 + k]);
                        }
                    }
                    GraphNodes.RemoveAt(i);
                    i = GraphNodes.Count; //force ends for loop
                }
            }
            else
            {
                if (GraphNodes[i].LeftHand.Equals(pickLeftHand))
                {
                    for (var j = 0; j < splitPickRightHand.Length; j++)
                    {
                        numberOfNodes++;
                        if (splitPickRightHand[j].Equals("split"))
                            splitNodeId = numberOfNodes;

                        if (j == 0)
                        {
                            if (splitPickRightHand[j].Equals("backToSplit"))
                                GraphNodes.Insert(i + 1 + j, new Node(splitPickRightHand[j], splitNodeId, GraphNodes[i].PreviousNodes));
                            else
                                GraphNodes.Insert(i + 1 + j, new Node(splitPickRightHand[j], numberOfNodes, GraphNodes[i].PreviousNodes));
                            AddMovementNode(GraphNodes[i + 1]);
                        }
                        else
                        {
                            if (splitPickRightHand[j].Equals("backToSplit"))
                                GraphNodes.Insert(i + 1 + j, new Node(splitPickRightHand[j], splitNodeId));
                            else
                                GraphNodes.Insert(i + 1 + j, new Node(splitPickRightHand[j], numberOfNodes));
                            AddMovementNode(GraphNodes[i + 1 + j]);
                        }
                    }
                    GraphNodes.RemoveAt(i);
                    i = GraphNodes.Count; //force ends for loop
                }
            }
        }
    }

    private string NewTempGraph()
    {
        string tempGraph = "";
        foreach (Node node in GraphNodes)
        {
            tempGraph += node.LeftHand + "-";
        }
        return tempGraph;
    }

    private void AddMovementNode(Node node)
    {
        if (node.MovementGraph.Count.Equals(0))
        {
            switch (node.LeftHand)
            {
                case "Task":
                    string[] splitMovementGraph = movementGraph.Split('-');
                    foreach(string MNode in splitMovementGraph)
                        node.MovementGraph.Add(MNode);
                    ExpandMovementNode(node);
                    break;
                case "key":
                    node.MovementGraph.Add(movementGraph);
                    ExpandMovementNode(node);
                    break;
                case "lock":
                    node.MovementGraph.Add(movementGraph);
                    ExpandMovementNode(node);
                    break;
                case "fight":
                    node.MovementGraph.Add(movementGraph);
                    ExpandMovementNode(node);
                    break;
                case "boss":
                    node.MovementGraph.Add(movementGraph);
                    ExpandMovementNode(node);
                    break;
                case "split":
                    node.MovementGraph.Add("tunnel");
                    node.MovementGraph.Add("jump");
                    node.MovementGraph.Add("tunnel");
                    node.MovementGraph.Add("jump");
                    node.MovementGraph.Add("land");
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
                    if (pick.RightHand.Length > 1)
                    {
                        int r = rnd.Next(0, pick.RightHand.Length);
                        String[] splitRightHand = pick.RightHand[r].Split('-');
                        for(var j = 0; j < splitRightHand.Length; j++)
                        {
                            if (splitRightHand[j].Equals("Glide"))
                            {
                                if (tempMovementGraph.LastOrDefault() != "walk")
                                {
                                    tempMovementGraph.Add(splitRightHand[j]);
                                }
                            }
                            else
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
            node.MovementGraph = checkMovementGraph(tempMovementGraph);
            Logging(node.MovementGraph);
            ExpandMovementNode(node);
        }
        else
        {
            //Debug.Log(node.MovementGraph);
        }
    }

    private void Logging(List<string> movementGraph)
    {
        string graph = "";
        foreach (string graphNode in movementGraph)
            graph = graph + "-" + graphNode;
        Debug.Log(graph);
    }

    private List<string> checkMovementGraph(List<string> graph)
    {
        for(var i = 0; i < graph.Count; i++)
        {
            switch (graph[i])
            {
                case "resetJump":
                    jumps--;
                    break;
                case "jump":
                    jumps++;
                    if (jumps > Player.GetComponent<PlayerStats>().Jumps)
                    {
                        graph[i] = "EM";
                        jumps--;
                    }
                    break;
                case "directionalJump":
                    jumps++;
                    if (jumps > Player.GetComponent<PlayerStats>().Jumps)
                    {
                        graph[i] = "EM";
                        jumps--;
                    }
                    break;
                case "Dash":
                    dashes++;
                    if (dashes > Player.GetComponent<PlayerStats>().Dashes)
                    {
                        graph[i] = "EM";
                        dashes--;
                    }
                    break;
                case "Glide":
                    glides++;
                    if (glides > Player.GetComponent<PlayerStats>().Glide)
                    {
                        graph[i] = "EM";
                        glides--;
                    }
                    break;
                case "land":
                    jumps = 0;
                    dashes = 0;
                    glides = 0;
                    break;
            }
        }

        return graph;
    }
}






