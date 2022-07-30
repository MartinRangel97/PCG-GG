using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Node
{
    
    public List<Node> PreviousNodes;
    public String LeftHand;
    public int Id;

    public Node(string leftHand, int id)
    {
        this.LeftHand = leftHand;
        this.Id = id;
        this.PreviousNodes = new List<Node>();
    }

    public Node(string leftHand, int id, List<Node> previousNodes )
    {
        this.LeftHand = leftHand;
        this.Id = id;
        this.PreviousNodes = previousNodes;
    }
}
