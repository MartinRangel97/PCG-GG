using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{
    [Header("Core")]
    public GameObject StartingZone;
    public GameObject Section;
    public GameObject Goal;
    public GameObject Platform;
    public GameObject Platform2;
    public GameObject Platform3Left;
    public GameObject Platform3Right;
    public GameObject Split;
    public GameObject BackToSplitLeft;
    public GameObject BackToSplitRight;

    [Header("Walk")]
    public GameObject WalkLeft;
    public GameObject WalkRight;
    public GameObject WalkLeft2;
    public GameObject WalkRight2;
    public GameObject WalkLeft3;
    public GameObject WalkRight3;
    public GameObject WalkLeft4;
    public GameObject WalkRight4;

    [Header("Dash")]
    public GameObject LeftDashToLand;
    public GameObject RightDashToLand;
    public GameObject LeftDashToDash;
    public GameObject RightDashToDash;
    public GameObject LeftDash;
    public GameObject RightDash;
    public GameObject HJumpToLeftDash;
    public GameObject HJumpToRightDash;
    public GameObject VJumpToLeftDash;
    public GameObject VJumpToRightDash;

    [Header("Jump")]
    public GameObject HorizontalJumpWalls;
    public GameObject HorizontalJumpLeftWalls;
    public GameObject HorizontalJumpRightWalls;
    public GameObject LeftJumpWall;
    public GameObject RightJumpWall;
    public GameObject AerialLeftJumpWall;
    public GameObject AerialRightJumpWall;
    public GameObject AerialLeftJumpWall2;
    public GameObject AerialRightJumpWall2;

    public GameObject Objective;

    [Header("Rooms")]
    public GameObject Lock;
    public GameObject Key;
    public GameObject FightRoom;

    public List<Node> GraphNodes;

    private Vector3 currentPosition;
    private string direction = "right";
    private string previousDirection = "right";
    private string previousNode = "";
    private bool incomingSplit = false;


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
                        Instantiate(FightRoom, currentPosition + new Vector3(-23.5f, 0, 0), Quaternion.identity, fightSection.transform);
                        currentPosition += new Vector3(-45, 0, 0);
                    }
                    else if (direction.Equals("right"))
                    {
                        Instantiate(FightRoom, currentPosition + new Vector3(23.5f, 0, 0), Quaternion.identity, fightSection.transform);
                        currentPosition += new Vector3(45, 0, 0);
                    }
                    break;
                case "key":
                    GameObject keySection = Instantiate(Section, currentPosition, Quaternion.identity, gameObject.transform);
                    keySection.name = "key";
                    GenerateSectionContent(GraphNodes[i].MovementGraph, keySection);
                    Instantiate(Key, currentPosition + new Vector3(0, 2, 0), Quaternion.identity, keySection.transform);
                    break;
                case "lock":
                    GameObject lockSection = Instantiate(Section, currentPosition, Quaternion.identity, gameObject.transform);
                    lockSection.name = "lock";
                    GenerateSectionContent(GraphNodes[i].MovementGraph, lockSection);
                    if (direction.Equals("left"))
                    {
                        Instantiate(Lock, currentPosition + new Vector3(-6.5f, 0, 0), Quaternion.identity, lockSection.transform);
                        currentPosition += new Vector3(-11, 0, 0);
                        Instantiate(Objective, currentPosition, Quaternion.identity, lockSection.transform);
                        //GraphNodes[i + 1].MovementGraph[1] = "left";
                    }
                    else if (direction.Equals("right"))
                    {
                        Instantiate(Lock, currentPosition + new Vector3(6.5f, 0, 0), Quaternion.identity, lockSection.transform);
                        currentPosition += new Vector3(11, 0, 0);
                        Instantiate(Objective, currentPosition, Quaternion.identity, lockSection.transform);
                        //GraphNodes[i + 1].MovementGraph[1] = "right";
                    }
                    previousNode = "Lock";
                    break;
                case "split":
                    GameObject splitSection = Instantiate(Section, currentPosition, Quaternion.identity, gameObject.transform);
                    splitSection.name = GraphNodes[i].Id.ToString();
                    GenerateSectionContent(GraphNodes[i].MovementGraph, splitSection);
                    GameObject split = Instantiate(Split, currentPosition, Quaternion.identity, splitSection.transform);
                    split.GetComponent<Split>().FirstPathDirection = GraphNodes[i + 1].MovementGraph[1];
                    if (split.GetComponent<Split>().FirstPathDirection.Equals("left"))
                    {
                        direction = "left";
                        split.GetComponent<Split>().SecondPathDirection = "right";
                    }
                    else
                    {
                        direction = "right";
                        split.GetComponent<Split>().SecondPathDirection = "left";
                    }
                    break;
                case "backToSplit":

                    GameObject backToSplitSection = Instantiate(Section, currentPosition, Quaternion.identity, gameObject.transform);
                    backToSplitSection.name = "backToSplit";

                    if (direction.Equals("left"))
                        Instantiate(BackToSplitLeft, currentPosition, Quaternion.identity, gameObject.transform);
                    else
                        Instantiate(BackToSplitRight, currentPosition, Quaternion.identity, gameObject.transform);

                    foreach (Transform child in transform)
                    {
                        if (child.name.Equals(GraphNodes[i].Id.ToString()))
                        {
                            currentPosition = child.Find("Split(Clone)").position;

                            direction = child.Find("Split(Clone)").GetComponent<Split>().SecondPathDirection;
                            GraphNodes[i + 1].MovementGraph[1] = child.Find("Split(Clone)").GetComponent<Split>().SecondPathDirection;
                            break;
                        }
                    }



                    //currentPosition = splitSection.transform.Find("Split(Clone)").position;
                    break;
                case "boss":
                    GameObject bossSection = Instantiate(Section, currentPosition, Quaternion.identity, gameObject.transform);
                    bossSection.name = "boss";
                    GenerateSectionContent(GraphNodes[i].MovementGraph, bossSection);
                    break;
                case "goal":
                    if (direction.Equals("left"))
                    {
                        Instantiate(Goal, currentPosition + new Vector3(-11, -0.25f, 0), Quaternion.identity, gameObject.transform);
                    }
                    else if (direction.Equals("right"))
                    {
                        Instantiate(Goal, currentPosition + new Vector3(11, -0.25f, 0), Quaternion.identity, gameObject.transform);
                    }
                    break;
            }
            if (i < GraphNodes.Count - 1 && GraphNodes[i + 1].LeftHand.Equals("split"))
                incomingSplit = true;
        }
    }

    private void GenerateSectionContent(List<string> graph, GameObject section)
    {
        //Vector3 currentPosition = section.transform.Find("Starting Section").position;

        string platformName = "";
        for (var i = 0; i < graph.Count; i++)
        {
            if (i == 4)
            {
                previousDirection = direction;
            }
            switch (graph[i])
            {
                case "left":
                    previousDirection = direction;
                    direction = "left";
                    break;
                case "right":
                    previousDirection = direction;
                    direction = "right";
                    break;
                case "directionalJump":
                    if (direction.Equals("left"))
                    {
                        GenerateDirectionalJumpWalls(i, graph, section);
                        currentPosition += new Vector3(-5, 4.5f, 0);
                        Instantiate(Objective, currentPosition, Quaternion.identity, section.transform);
                        platformName += "leftJump ";
                    }
                    else if (direction.Equals("right"))
                    {
                        GenerateDirectionalJumpWalls(i, graph, section);
                        currentPosition += new Vector3(5, 4.5f, 0);
                        Instantiate(Objective, currentPosition, Quaternion.identity, section.transform);
                        platformName += "rightJump ";
                    }
                    previousNode = "directionalJump";
                    break;
                case "jump":
                    currentPosition += new Vector3(0, 4.5f, 0);
                    Instantiate(Objective, currentPosition, Quaternion.identity, section.transform);
                    platformName += "Jump ";
                    GenerateJumpWalls(i, graph, section);
                    previousNode = "jump";
                    break;
                case "Dash":
                    GenerateDashObstacle(i, graph, section);
                    previousNode = "Dash";
                    break;
                case "Glide":
                    if (direction.Equals("left"))
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
                    previousNode = "Glide";
                    break;
                case "walk":
                    GenerateWalkFloors(i, graph, section);
                    previousNode = "walk";
                    break;
                case "land":
                    if (i == graph.Count - 1)
                    {
                        if (graph[i - 1].Equals("jump") && incomingSplit == false)
                        {
                            if (direction.Equals("left"))
                            {
                                GameObject platform = Instantiate(Platform3Left, currentPosition, Quaternion.identity, section.transform);
                                platform.name = platformName;
                                platformName = "";

                            }
                            else if (direction.Equals("right"))
                            {
                                GameObject platform = Instantiate(Platform3Right, currentPosition, Quaternion.identity, section.transform);
                                platform.name = platformName;
                                platformName = "";
                            }
                        }
                        else
                        {
                            GameObject platform = Instantiate(Platform2, currentPosition, Quaternion.identity, section.transform);
                            platform.name = platformName;
                            platformName = "";
                        }
                        incomingSplit = false;
                    }
                    else if (graph[i + 1].Equals("jump") || graph[i + 1].Equals("directionalJump"))
                    {
                        GameObject platform = Instantiate(Platform, currentPosition, Quaternion.identity, section.transform);
                        platform.name = platformName;
                        platformName = "";
                    }
                    else if (graph[i - 1].Equals("jump"))
                    {
                        if (direction.Equals("left"))
                        {
                            GameObject platform = Instantiate(Platform3Left, currentPosition, Quaternion.identity, section.transform);
                            platform.name = platformName;
                            platformName = "";
                        }
                        else if (direction.Equals("right"))
                        {
                            GameObject platform = Instantiate(Platform3Right, currentPosition, Quaternion.identity, section.transform);
                            platform.name = platformName;
                            platformName = "";
                        }
                    }
                    else
                    {
                        GameObject platform = Instantiate(Platform2, currentPosition, Quaternion.identity, section.transform);
                        platform.name = platformName;
                        platformName = "";
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
                JumpDistance = new Vector3((float)xRJRange, (float)yRJRange, 0);
                break;
        }
        Debug.Log(JumpDistance);
        return JumpDistance;
    }

    private void GenerateJumpWalls(int index, List<string> graph, GameObject parent)
    {
        if (previousNode.Equals("walk") || previousNode.Equals("directionalJump") || previousNode.Equals("Dash") || previousNode.Equals("Lock"))
        {
            if (direction.Equals("left"))
            {
                if (previousDirection.Equals("left"))
                {
                    Instantiate(HorizontalJumpLeftWalls, currentPosition, Quaternion.identity, parent.transform);

                }
                else
                {
                    Instantiate(HorizontalJumpRightWalls, currentPosition, Quaternion.identity, parent.transform);
                }
            }
            else if (direction.Equals("right"))
            {
                if (previousDirection.Equals("right"))
                {
                    Instantiate(HorizontalJumpRightWalls, currentPosition, Quaternion.identity, parent.transform);

                }
                else
                {
                    Instantiate(HorizontalJumpLeftWalls, currentPosition, Quaternion.identity, parent.transform);
                }
            }
        }
        else
        {
            Instantiate(HorizontalJumpWalls, currentPosition, Quaternion.identity, parent.transform);
        }


        //switch (direction)
        //{
        //    case "left":
        //        if (previousDirection.Equals("left"))
        //        {
        //            GameObject LeftJumpWall = Instantiate(HorizontalJumpRightWalls, currentPosition + new Vector3(), Quaternion.identity, parent.transform);
        //            //LeftJumpWall.transform.Find("Wall").transform.localScale = new Vector3(0.5f, 5, 0);
        //            //LeftJumpWall.transform.Find("Wall").transform.localPosition = new Vector3(-1.25f, -2.25f, 0);
        //        } 
        //        else
        //        {
        //            GameObject LeftJumpWall = Instantiate(HorizontalJumpRightWalls, currentPosition + new Vector3(), Quaternion.identity, parent.transform);
        //        }
        //        break;

        //    case "right":
        //        if (previousDirection.Equals("right"))
        //        {
        //            GameObject RightJumpWall = Instantiate(HorizontalJumpRightWalls, currentPosition + new Vector3(), Quaternion.identity, parent.transform);
        //            //RightJumpWall.transform.Find("Wall").transform.localScale = new Vector3(0.5f, 5, 0);
        //            //RightJumpWall.transform.Find("Wall").transform.localPosition = new Vector3(1.25f, -2.25f, 0);
        //        }
        //        else
        //        {
        //            GameObject RightJumpWall = Instantiate(HorizontalJumpLeftWalls, currentPosition + new Vector3(), Quaternion.identity, parent.transform);
        //        }
        //        break;
        //}
    }

    private void GenerateDirectionalJumpWalls(int index, List<string> graph, GameObject parent)
    {
        switch (previousNode)
        {
            case "directionalJump":
                if (direction.Equals("left"))
                {
                    if (previousDirection.Equals("left"))
                    {
                        Instantiate(LeftJumpWall, currentPosition, Quaternion.identity, parent.transform);
                        Instantiate(AerialLeftJumpWall, currentPosition, Quaternion.identity, parent.transform);
                    }
                    else
                    {
                        Instantiate(AerialRightJumpWall, currentPosition, Quaternion.identity, parent.transform);
                    }
                }
                else if (direction.Equals("right"))
                {
                    if (previousDirection.Equals("right"))
                    {
                        Instantiate(RightJumpWall, currentPosition, Quaternion.identity, parent.transform);
                        Instantiate(AerialRightJumpWall, currentPosition, Quaternion.identity, parent.transform);
                    }
                    else
                    {
                        Instantiate(AerialLeftJumpWall, currentPosition, Quaternion.identity, parent.transform);
                    }
                }
                break;
            case "walk":
                if (direction.Equals("left"))
                {
                    if (previousDirection.Equals("left"))
                    {
                        Instantiate(LeftJumpWall, currentPosition, Quaternion.identity, parent.transform);
                        Instantiate(AerialLeftJumpWall, currentPosition, Quaternion.identity, parent.transform);
                    }
                    else
                        Instantiate(AerialLeftJumpWall2, currentPosition, Quaternion.identity, parent.transform);
                }
                else if (direction.Equals("right"))
                {
                    if (previousDirection.Equals("right"))
                    {
                        Instantiate(RightJumpWall, currentPosition, Quaternion.identity, parent.transform);
                        Instantiate(AerialRightJumpWall, currentPosition, Quaternion.identity, parent.transform);
                    }
                    else
                        Instantiate(AerialRightJumpWall2, currentPosition, Quaternion.identity, parent.transform);
                }
                break;
            case "jump":
                if (direction.Equals("left"))
                {
                    if (previousDirection.Equals("left"))
                    {
                        Instantiate(LeftJumpWall, currentPosition, Quaternion.identity, parent.transform);
                        Instantiate(AerialLeftJumpWall2, currentPosition, Quaternion.identity, parent.transform);
                    }
                    else
                        Instantiate(AerialLeftJumpWall2, currentPosition, Quaternion.identity, parent.transform);
                }
                else if (direction.Equals("right"))
                {
                    if (previousDirection.Equals("right"))
                    {
                        Instantiate(RightJumpWall, currentPosition, Quaternion.identity, parent.transform);
                        Instantiate(AerialRightJumpWall2, currentPosition, Quaternion.identity, parent.transform);
                    }
                    else
                        Instantiate(AerialRightJumpWall2, currentPosition, Quaternion.identity, parent.transform);
                }
                break;
            case "land":
                if (direction.Equals("left"))
                {
                    Instantiate(LeftJumpWall, currentPosition, Quaternion.identity, parent.transform);
                    Instantiate(AerialLeftJumpWall2, currentPosition, Quaternion.identity, parent.transform);
                }
                else if (direction.Equals("right"))
                {
                    Instantiate(RightJumpWall, currentPosition, Quaternion.identity, parent.transform);
                    Instantiate(AerialRightJumpWall2, currentPosition, Quaternion.identity, parent.transform);
                }
                break;
            case "Dash":
                if (direction.Equals("left"))
                {
                    if (previousDirection.Equals("left"))
                    {
                        Instantiate(LeftJumpWall, currentPosition, Quaternion.identity, parent.transform);
                        Instantiate(AerialLeftJumpWall, currentPosition, Quaternion.identity, parent.transform);
                    }
                    else
                        Instantiate(AerialLeftJumpWall2, currentPosition, Quaternion.identity, parent.transform);
                }
                else if (direction.Equals("right"))
                {
                    if (previousDirection.Equals("right"))
                    {
                        Instantiate(RightJumpWall, currentPosition, Quaternion.identity, parent.transform);
                        Instantiate(AerialRightJumpWall, currentPosition, Quaternion.identity, parent.transform);
                    }
                    else
                        Instantiate(AerialRightJumpWall2, currentPosition, Quaternion.identity, parent.transform);
                }
                break;
            case "Lock":
                if (direction.Equals("left"))
                {
                    if (previousDirection.Equals("left"))
                    {
                        Instantiate(LeftJumpWall, currentPosition, Quaternion.identity, parent.transform);
                        Instantiate(AerialLeftJumpWall, currentPosition, Quaternion.identity, parent.transform);
                    }
                    else
                    {
                        Instantiate(AerialRightJumpWall, currentPosition, Quaternion.identity, parent.transform);
                    }
                }
                else if (direction.Equals("right"))
                {
                    if (previousDirection.Equals("right"))
                    {
                        Instantiate(RightJumpWall, currentPosition, Quaternion.identity, parent.transform);
                        Instantiate(AerialRightJumpWall, currentPosition, Quaternion.identity, parent.transform);
                    }
                    else
                    {
                        Instantiate(AerialLeftJumpWall, currentPosition, Quaternion.identity, parent.transform);
                    }
                }
                break;
        }
    }

    private void GenerateDashObstacle(int index, List<string> graph, GameObject parent)
    {
        //------------------first half of the obstacle-------------------------------

        if (graph[index - 1].Equals("directionalJump"))
        {
            if (direction.Equals("left"))
            {
                Instantiate(HJumpToLeftDash, currentPosition, Quaternion.identity, parent.transform);
            }
            else if (direction.Equals("right"))
            {
                Instantiate(HJumpToRightDash, currentPosition, Quaternion.identity, parent.transform);
            }
        }
        else if (graph[index - 1].Equals("jump"))
        {
            if (direction.Equals("left"))
            {
                Instantiate(VJumpToLeftDash, currentPosition, Quaternion.identity, parent.transform);
            }
            else if (direction.Equals("right"))
            {
                Instantiate(VJumpToRightDash, currentPosition, Quaternion.identity, parent.transform);
            }
        }
        else if (graph[index - 1].Equals("land") || graph[index - 1].Equals("walk") || graph[index - 1].Equals("Dash"))
        {
            if (direction.Equals("left"))
            {
                Instantiate(LeftDashToLand, currentPosition, Quaternion.identity, parent.transform);
            }
            else if (direction.Equals("right"))
            {
                Instantiate(RightDashToLand, currentPosition, Quaternion.identity, parent.transform);
            }
        }

        if (direction.Equals("left"))
        {
            currentPosition += new Vector3(-5, 0, 0);
        }
        else if (direction.Equals("right"))
        {
            currentPosition += new Vector3(5, 0, 0);
        }

        //------------------Second half of the obstacle-------------------------------

        if (graph[index + 1].Equals("jump") || graph[index + 1].Equals("directionalJump"))
        {
            if (direction.Equals("left"))
            {
                Instantiate(LeftDash, currentPosition, Quaternion.identity, parent.transform);
            }
            else if (direction.Equals("right"))
            {
                Instantiate(RightDash, currentPosition, Quaternion.identity, parent.transform);
            }
        }
        else if (graph[index + 1].Equals("Dash"))
        {
            if (direction.Equals("left"))
            {
                Instantiate(LeftDashToDash, currentPosition, Quaternion.identity, parent.transform);
            }
            else if (direction.Equals("right"))
            {
                Instantiate(RightDashToDash, currentPosition, Quaternion.identity, parent.transform);
            }
        }

        else if (graph[index + 1].Equals("land") || graph[index + 1].Equals("walk"))
        {
            if (direction.Equals("left"))
            {
                Instantiate(LeftDashToLand, currentPosition, Quaternion.identity, parent.transform);
            }
            else if (direction.Equals("right"))
            {
                Instantiate(RightDashToLand, currentPosition, Quaternion.identity, parent.transform);
            }
        }

        if (direction.Equals("left"))
        {
            currentPosition += new Vector3(-5, 0, 0);
        }
        else if (direction.Equals("right"))
        {
            currentPosition += new Vector3(5, 0, 0);
        }
    }

    private void GenerateWalkFloors(int index, List<string> graph, GameObject parent)
    {
        if (direction.Equals("left"))
        {
            switch (graph[index + 1])
            {
                case "left":
                    Instantiate(WalkLeft2, currentPosition, Quaternion.identity, parent.transform);
                    break;
                case "right":
                    if (graph[index + 2].Equals("jump"))
                    {
                        if (graph[index + 4].Equals("directionalJump"))
                            Instantiate(WalkLeft3, currentPosition, Quaternion.identity, parent.transform);
                        else
                            Instantiate(WalkLeft2, currentPosition, Quaternion.identity, parent.transform);
                    }
                    else if (graph[index + 2].Equals("directionalJump"))
                    {
                        if (graph[index + 4].Equals("directionalJump"))
                            Instantiate(WalkLeft4, currentPosition, Quaternion.identity, parent.transform);
                        else
                            Instantiate(WalkLeft3, currentPosition, Quaternion.identity, parent.transform);
                    }
                    else
                    {
                        Instantiate(WalkLeft2, currentPosition, Quaternion.identity, parent.transform);
                    }
                    break;
                case "jump":
                    Instantiate(WalkLeft2, currentPosition, Quaternion.identity, parent.transform);
                    break;
                case "walk":
                    Instantiate(WalkLeft, currentPosition, Quaternion.identity, parent.transform);
                    break;
            }
            currentPosition += new Vector3(-18, 0, 0);
            Instantiate(Objective, currentPosition, Quaternion.identity, parent.transform);

        }
        else if (direction.Equals("right"))
        {
            switch (graph[index + 1])
            {
                case "right":
                    Instantiate(WalkRight2, currentPosition, Quaternion.identity, parent.transform);
                    break;
                case "left":
                    if (graph[index + 2].Equals("jump"))
                    {
                        if (graph[index + 4].Equals("directionalJump"))
                            Instantiate(WalkRight3, currentPosition, Quaternion.identity, parent.transform);
                        else
                            Instantiate(WalkRight2, currentPosition, Quaternion.identity, parent.transform);
                    }
                    else if (graph[index + 2].Equals("directionalJump"))
                    {
                        if (graph[index + 4].Equals("directionalJump"))
                            Instantiate(WalkRight4, currentPosition, Quaternion.identity, parent.transform);
                        else
                            Instantiate(WalkRight3, currentPosition, Quaternion.identity, parent.transform);
                    }
                    else
                    {
                        Instantiate(WalkRight2, currentPosition, Quaternion.identity, parent.transform);
                    }
                    break;
                case "jump":
                    Instantiate(WalkRight2, currentPosition, Quaternion.identity, parent.transform);
                    break;
                case "walk":
                    Instantiate(WalkRight, currentPosition, Quaternion.identity, parent.transform);
                    break;
            }
            currentPosition += new Vector3(18, 0, 0);
            Instantiate(Objective, currentPosition, Quaternion.identity, parent.transform);

        }
    }
}
