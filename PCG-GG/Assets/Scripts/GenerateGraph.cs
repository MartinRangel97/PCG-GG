using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GenerateGraph : MonoBehaviour
{
    private string MainGraph = "Start";

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
        Rule pick = Array.Find(Production, findrule => graph.Contains(findrule.LeftHand[0]));

        if (pick != null)
        {
            if (pick.RightHand.Length > 1)
            {
                System.Random rnd = new System.Random();
                int r = rnd.Next(pick.RightHand.Length);

                graph = graph.Replace(pick.LeftHand[0], pick.RightHand[r]);
                Debug.Log(graph);
                Expand(Production, graph);

            }
            else
            {
                graph = graph.Replace(pick.LeftHand[0], pick.RightHand[0]);
                Debug.Log(graph);
                Expand(Production, graph);
            }
        }
        else
        {
            Debug.Log(graph);
        }
    }
}
