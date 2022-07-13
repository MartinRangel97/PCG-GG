using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GenerateGraph : MonoBehaviour
{
    private string MainGraph = "Start";
    private System.Random rnd = new System.Random();

    private void Awake()
    {
        RulesData.Setup();
    }

    private void Start()
    {
        Debug.Log("Starting");
        Expand(RulesData.ProductionRules, MainGraph);
    }

    private void Expand(Rule[] Production, String graph)
    {
        Debug.Log("-----------------------New Evolution-----------------");
        Rule pick = Array.Find(Production, findrule => graph.Contains(findrule.LeftHand));
        string newGraph = "";

        if (pick != null)
        {
            String[] splitGraph = graph.Split('-');
            for (var i = 0; i < splitGraph.Length; i++)
            {
                if (pick.LeftHand.Equals(splitGraph[i]))
                {
                    if (pick.RightHand.Length > 1)
                    {
                        int r = rnd.Next(0, pick.RightHand.Length);
                        splitGraph[i] = pick.RightHand[r];
                        newGraph = newGraph + "-" + pick.RightHand[r];
                        Debug.Log(pick.LeftHand + " -> " + pick.RightHand[r]); 
                    }
                    else
                    {
                        newGraph = newGraph + "-" + pick.RightHand[0];
                        Debug.Log(pick.LeftHand + " -> " + pick.RightHand[0]);
                    }
                }
                else
                {
                    newGraph = newGraph + "-" + splitGraph[i];
                    Debug.Log(splitGraph[i]);
                }
                
            }

            
            graph = newGraph;
            Debug.Log(graph);
            Expand(Production, graph);
        }
        else
        {
            Debug.Log("Done");
            Debug.Log(graph);
        }
    }
}
