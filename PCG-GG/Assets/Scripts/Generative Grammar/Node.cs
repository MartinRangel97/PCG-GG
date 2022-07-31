using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


[Serializable]
public class Node
{
    
    public List<Node> PreviousNodes;
    public string LeftHand;
    public int Id;
    public string MovementGraph;

    private System.Random rnd = new System.Random();
    private List<string> TempGraph;

    public Node(string leftHand, int id)
    {
        this.LeftHand = leftHand;
        this.Id = id;
        this.PreviousNodes = new List<Node>();
        this.MovementGraph = "";
    }

    public Node(string leftHand, int id, List<Node> previousNodes )
    {
        this.LeftHand = leftHand;
        this.Id = id;
        this.PreviousNodes = previousNodes;
        this.MovementGraph = "";
    }

    private List<string> TempMovementGraph()
    {
        List<string> tempGraph = new List<string>();

        string[] splitTempGraph = MovementGraph.Split('-');

        foreach(string str in splitTempGraph)
        {
            tempGraph.Add(str);
        }

        return tempGraph;
    }
}






