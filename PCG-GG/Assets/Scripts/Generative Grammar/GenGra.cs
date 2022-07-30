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

    public List<Node> GraphNodes = new List<Node>();

    string path;
    string jsonString;

    public ProductionNodeList productionNodeList = new ProductionNodeList();

    private int numberOfNodes = 1;

    // Start is called before the first frame update
    void Start()
    {
        path = Application.streamingAssetsPath + "/ProductionNode.json";
        jsonString = File.ReadAllText(path);
        productionNodeList = JsonUtility.FromJson<ProductionNodeList>(jsonString);
        Node Start = new Node("Start", numberOfNodes);
        GraphNodes.Add(Start);
        ExpandNode();
        
    }

    private void ExpandNode()
    {
        //TODO
        //filter possible production nodes +++++++++++++++
        //loop though each node +++++++++++++++
        //check if node matches with production node +++++++++++++++++
        //if true, create nodes based on the corresponding node +++++++++++++++
        //establish connection between the new nodes ++++++++++++++++++++++
        //add previous node to the first node and connecting node to the last node from the node that is being replaced if any


        var rnd = new System.Random();
        var randomized = productionNodeList.productionNodes.OrderBy(x => rnd.Next()).ToArray();

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
                                }
                                else
                                {
                                    tempGraphNode.Add(new Node(splitGraph[j], numberOfNodes));
                                }
                            } 
                            else
                            {
                                //Debug.Log("exactly 1 " + GraphNodes[i].LeftHand);
                                tempGraphNode.Add(new Node(splitGraph[j], numberOfNodes));
                            }
                           
                        }
                        tempGraphNode = ConnectNodes(tempGraphNode, splitGraph.Length, i);
                        tempGraphNode = DeleteConnections(GraphNodes[i], tempGraphNode);
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
                                }
                                else
                                {
                                    Debug.Log("gets in here");
                                    tempGraphNode.Add(new Node(splitGraph[j], numberOfNodes));
                                }
                            } 
                            else 
                            {
                                tempGraphNode.Add(new Node(splitGraph[j], numberOfNodes));
                            }
                            
                        }
                        tempGraphNode = ConnectNodes(tempGraphNode, splitGraph.Length, i);
                        tempGraphNode = DeleteConnections(GraphNodes[i], tempGraphNode);
                    }               
                }
                else
                {
                    //Debug.Log("didnt change the node: " + GraphNodes[i].LeftHand);
                    tempGraphNode.Add(new Node(GraphNodes[i].LeftHand, GraphNodes[i].Id, GraphNodes[i].PreviousNodes)); // Remake the node in the temp graph
                    //tempGraphNode = ConnectNodes(tempGraphNode, 0, i, GraphNodes[i]);
                }
            }
            GraphNodes = tempGraphNode; // the temp graph replaces the original graph
            Logging();
            ExpandNode();
        } else
        {
            Logging();
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

    private List<Node> DeleteConnections(Node nodeToDelete, List<Node> tempGraphNode)
    {
         foreach(Node tempNode in tempGraphNode.ToList<Node>())
         {
            //Debug.Log("---------------------------------------------------------------------------------------------------");
            //Debug.Log(tempNode.Id + " : " + nodeToDelete.Id);

            foreach (Node previousNode in tempNode.PreviousNodes.ToList<Node>())
            {
                //Debug.Log(nodeToDelete.LeftHand + nodeToDelete.Id.ToString() + ":" + previousNode.LeftHand + previousNode.Id.ToString());
                if (nodeToDelete.Id.Equals(previousNode.Id))
                {
                    //Debug.Log("node deleted id:" + previousNode.Id);
                    tempNode.PreviousNodes.Remove(previousNode);
                }

                if (tempNode.Id.Equals(previousNode.Id))
                {

                    tempNode.PreviousNodes.Remove(previousNode);
                }
            }

        }
        return tempGraphNode;
    }
}






