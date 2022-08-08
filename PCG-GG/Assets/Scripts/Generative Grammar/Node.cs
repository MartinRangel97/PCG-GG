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
    public List<string> MovementGraph;

    private System.Random rnd = new System.Random();
    private List<string> TempGraph;

    public Node(string leftHand, int id)
    {
        this.LeftHand = leftHand;
        this.Id = id;
        this.PreviousNodes = new List<Node>();
        this.MovementGraph = new List<string>();
    }

    public Node(string leftHand, int id, List<Node> previousNodes )
    {
        this.LeftHand = leftHand;
        this.Id = id;
        this.PreviousNodes = previousNodes;
        this.MovementGraph = new List<string>();
    }
}






