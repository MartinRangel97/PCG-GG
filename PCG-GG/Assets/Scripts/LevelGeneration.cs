using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{
    public GameObject StartingZone;
    public GameObject Section;
    public GameObject Goal;
    public GameObject Platform;
    public GameObject LeftJumpWall;
    public GameObject Walk;
    public GameObject Lock;

    public GameObject Objective;
    public GameObject FightRoom;

    public List<Node> GraphNodes;

    private Vector3 currentPosition;
    private string direction = "right";
    private void Start()
    {
        currentPosition = StartingZone.transform.Find("Starting Line").position;
        GraphNodes = gameObject.GetComponent<GenGra>().GraphNodes;
        for (var i = 0; i < GraphNodes.Count; i++)
        {
            switch (GraphNodes[i].LeftHand)
            {
                case "start":
                    Instantiate(StartingZone, new Vector3(i * 20, 0, 0), Quaternion.identity, gameObject.transform);
                    break;
                case "fight":
                    GameObject fightSection = Instantiate(Section, currentPosition, Quaternion.identity, gameObject.transform);
                    fightSection.name = "fight";
                    GenerateSectionContent(GraphNodes[i].MovementGraph, fightSection);
                    if (direction.Equals("left"))
                    {
                        Instantiate(FightRoom, currentPosition + new Vector3(-22.5f, 0, 0), Quaternion.identity, fightSection.transform);
                        currentPosition += new Vector3(-45, 0, 0);
                    }
                    else if (direction.Equals("right"))
                    {
                        Instantiate(FightRoom, currentPosition + new Vector3(22.5f, 0, 0), Quaternion.identity, fightSection.transform);
                        currentPosition += new Vector3(45, 0, 0);
                    }
                    break;
                case "key":
                    GameObject keySection = Instantiate(Section, currentPosition, Quaternion.identity, gameObject.transform);
                    keySection.name = "key";
                    GenerateSectionContent(GraphNodes[i].MovementGraph, keySection);
                    break;
                case "lock":
                    GameObject lockSection = Instantiate(Section, currentPosition, Quaternion.identity, gameObject.transform);
                    lockSection.name = "lock";
                    if (direction.Equals("left"))
                    {
                        Instantiate(Lock, currentPosition + new Vector3(-5.5f, 0, 0), Quaternion.identity, lockSection.transform);
                        currentPosition += new Vector3(-11, 0, 0);
                    }
                    else if (direction.Equals("right"))
                    {
                        Instantiate(Lock, currentPosition + new Vector3(5.5f, 0, 0), Quaternion.identity, lockSection.transform);
                        currentPosition += new Vector3(11, 0, 0);
                    }
                    GenerateSectionContent(GraphNodes[i].MovementGraph, lockSection);
                    break;
                case "boss":
                    GameObject bossSection = Instantiate(Section, currentPosition, Quaternion.identity, gameObject.transform);
                    bossSection.name = "boss";
                    GenerateSectionContent(GraphNodes[i].MovementGraph, bossSection);
                    break;
                case "goal":
                    if (direction.Equals("left"))
                    {
                        Instantiate(Goal, currentPosition + new Vector3(-11,0,0), Quaternion.identity, gameObject.transform);
                    }
                    else if (direction.Equals("right"))
                    {
                        Instantiate(Goal, currentPosition + new Vector3(11, 0, 0), Quaternion.identity, gameObject.transform);
                    }
                    break;
            }
        }
    }


    private void GenerateSectionContent(List<string> graph, GameObject section)
    {
        //Vector3 currentPosition = section.transform.Find("Starting Section").position;
        
        string platformName = "";
        for (var i = 0; i < graph.Count; i++)
        {
            switch (graph[i])
            {
                case "leftJump":
                    direction = "left";
                    //currentPosition += CalculateJumpDistance(direction);
                    currentPosition += new Vector3(-7, 4.5f, 0);
                    Instantiate(Objective, currentPosition, Quaternion.identity, section.transform);
                    platformName += "leftJump ";
                    
                    break;
                case "rightJump":
                    direction = "right";
                    //currentPosition += CalculateJumpDistance(direction);
                    currentPosition += new Vector3(7, 4.5f, 0);
                    Instantiate(Objective, currentPosition, Quaternion.identity, section.transform);
                    platformName += "rightJump ";
                    break;
                case "Jump":
                    currentPosition += new Vector3(0, 4.5f, 0);
                    Instantiate(Objective, currentPosition, Quaternion.identity, section.transform);
                    platformName += "Jump ";
                    break;
                //case "leftDash":
                //    direction = "left";
                //    currentPosition += new Vector3(-9, 0, 0);
                //    Instantiate(Objective, currentPosition, Quaternion.identity, section.transform);
                //    break;
                //case "rightDash":
                //    direction = "right";
                //    currentPosition += new Vector3(9, 0, 0);
                //    Instantiate(Objective, currentPosition, Quaternion.identity, section.transform);
                //    break;
                case "Dash":
                    if (direction.Equals("left"))
                    {
                        currentPosition += new Vector3(-9, 0, 0);
                        Instantiate(Objective, currentPosition, Quaternion.identity, section.transform);
                        platformName += "leftDash ";
                    }
                    else if (direction.Equals("right"))
                    {
                        currentPosition += new Vector3(9, 0, 0);
                        Instantiate(Objective, currentPosition, Quaternion.identity, section.transform);
                        platformName += "rightDash ";
                    }
                    //currentPosition += new Vector3(9, 0, 0);
                    //Instantiate(Objective, currentPosition, Quaternion.identity, section.transform);
                    break;
                case "Glide":
                    if(direction.Equals("left"))
                    {
                        currentPosition += new Vector3(-6, -1.25f, 0);
                        Instantiate(Objective, currentPosition, Quaternion.identity, section.transform);
                        platformName += "leftGlide ";
                    } 
                    else if (direction.Equals("right"))
                    {
                        currentPosition += new Vector3(6, -1.25f, 0);
                        Instantiate(Objective, currentPosition, Quaternion.identity, section.transform);
                        platformName += "rightGlide ";
                    }
                    break;
                case "leftWalk":
                    platformName += "leftWalk";
                    break;
                case "rightWalk":
                    platformName += "rightWalk";
                    break;
                case "walk":
                    Debug.Log("gets in here");
                    if (direction.Equals("left"))
                    {
                        GameObject walk = Instantiate(Walk, currentPosition + new Vector3(-9,0,0), Quaternion.identity, section.transform);
                        currentPosition += new Vector3(-18, 0, 0);
                        Instantiate(Objective, currentPosition, Quaternion.identity, section.transform);
                        
                    }
                    else if (direction.Equals("right"))
                    {
                        GameObject walk = Instantiate(Walk, currentPosition + new Vector3(9, 0, 0), Quaternion.identity, section.transform);
                        currentPosition += new Vector3(18, 0, 0);
                        Instantiate(Objective, currentPosition, Quaternion.identity, section.transform);
                    }
                    break;
                case "land":
                    if (graph[i - 1].Equals("walk"))
                    {
                        // Do nothing
                    }
                    else
                    {
                        GameObject platform = Instantiate(Platform, currentPosition, Quaternion.identity, section.transform);
                        platform.name = platformName;
                        platformName = "";
                        if (direction.Equals("left"))
                        {
                            currentPosition += new Vector3(-1, 0, 0);
                        } 
                        else if (direction.Equals("right"))
                        {
                            currentPosition += new Vector3(1, 0, 0);
                        }
                        
                    }
                    break;
            }
        }
    }

    private Vector3 CalculateJumpDistance(string direction)
    {
        Vector3 JumpDistance = new Vector3();
        System.Random random = new System.Random();
        double randomDouble = random.NextDouble();
        switch (direction)
        {
            case "left":
                double xLJRange = randomDouble * (-12 - -7) + -7;
                double yLJRange = randomDouble * (6 - 0) + 0;
                JumpDistance = new Vector3((float)xLJRange, (float)yLJRange, 0);
                break;
            case "right":
                double xRJRange = randomDouble * (12 - 7) + 7;
                double yRJRange = randomDouble * (6 - 0) + 0;
                JumpDistance = new Vector3( (float)xRJRange, (float)yRJRange, 0);
                break;
        }
        Debug.Log(JumpDistance);
        return JumpDistance;
    }
}
