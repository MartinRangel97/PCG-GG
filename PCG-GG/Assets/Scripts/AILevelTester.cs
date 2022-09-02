using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Windows;
using InputSimulatorStandard;
using InputSimulatorStandard.Native;
public class AILevelTester : MonoBehaviour
{
    
    InputSimulator simulator = new InputSimulator();

    public GameObject Map;
    public float moveTime;
    public string currentMove;

    private int graphIndex = 0;
    private int movementIndex = 0;
    public bool jumped;
    private bool dashed;
    private string direction = "right";
    private string previousDirection = "right";
    private float movementDuration;

    private List<Node> GraphNodes;

    public List<string> graph;

    void Start()
    {
        GraphNodes = Map.GetComponent<GenGra>().GraphNodes;
        for (var i = 0; i < GraphNodes.Count; i++)
        {
            for (var j = 0; j < GraphNodes[i].MovementGraph.Count; j++)
            {
                graph.Add(GraphNodes[i].MovementGraph[j]);
            }
            graph.Add(GraphNodes[i].LeftHand);
        }


        //graph.Add("land");
        //graph.Add("land");
        //graph.Add("jump");
        //graph.Add("Glide");
        //graph.Add("Glide");
    }

    private void FixedUpdate()
    {
        switch (graph[graphIndex])
        {
            case "left":
                direction = "left";
                NextMove();
                break;
            case "right":
                direction = "right";
                NextMove();
                break;
            case "same":
                NextMove();
                break;
            case "walk":
                movementDuration = 3f;

                if (direction.Equals("left"))
                {
                    if (moveTime < movementDuration)
                    {
                        simulator.Keyboard.KeyPress(VirtualKeyCode.VK_A);
                        simulator.Keyboard.KeyDown(VirtualKeyCode.VK_A);
                    }
                    else
                    {
                        simulator.Keyboard.KeyUp(VirtualKeyCode.VK_A);
                        NextMove();
                    }
                }
                else
                {
                    if (moveTime < movementDuration)
                    {
                        simulator.Keyboard.KeyPress(VirtualKeyCode.VK_D);
                        simulator.Keyboard.KeyDown(VirtualKeyCode.VK_D);
                    }
                    else
                    {
                        simulator.Keyboard.KeyUp(VirtualKeyCode.VK_D);
                        NextMove();
                    }
                }
                break;
            case "tunnel":
                movementDuration = 3f;
                if (direction.Equals("left"))
                {
                    if (moveTime < movementDuration)
                    {
                        simulator.Keyboard.KeyPress(VirtualKeyCode.VK_A);
                        simulator.Keyboard.KeyDown(VirtualKeyCode.VK_A);
                    }
                    else
                    {
                        simulator.Keyboard.KeyUp(VirtualKeyCode.VK_A);
                        NextMove();
                    }
                }
                else
                {
                    if (moveTime < movementDuration)
                    {
                        simulator.Keyboard.KeyPress(VirtualKeyCode.VK_D);
                        simulator.Keyboard.KeyDown(VirtualKeyCode.VK_D);
                    }
                    else
                    {
                        simulator.Keyboard.KeyUp(VirtualKeyCode.VK_D);
                        NextMove();
                    }
                }
                break;
            case "jump":
                movementDuration = 0.5f;
                if (moveTime < movementDuration)
                {
                    if (jumped == false)
                    {
                        jumped = true;
                        if (graph[graphIndex + 1].Equals("Glide"))
                        {
                            simulator.Keyboard.KeyPress(VirtualKeyCode.SPACE);
                            simulator.Keyboard.KeyDown(VirtualKeyCode.SPACE);
                        }
                        else
                            simulator.Keyboard.KeyPress(VirtualKeyCode.SPACE);
                        jumped = true;
                    }
                }  
                else
                  NextMove();
                break;
            case "directionalJump":
                if (graph[graphIndex + 1].Equals("Dash"))
                    movementDuration = 0.7f;
                else
                    movementDuration = 0.825f;

                simulator.Keyboard.KeyUp(VirtualKeyCode.SPACE);

                if (direction.Equals("left"))
                {
                    if (moveTime < movementDuration)
                    {
                        if (jumped == false)
                        {
                            if (graph[graphIndex + 1].Equals("Glide"))
                            {
                                simulator.Keyboard.KeyPress(VirtualKeyCode.SPACE);
                                simulator.Keyboard.KeyDown(VirtualKeyCode.SPACE);
                            }
                            else
                                simulator.Keyboard.KeyPress(VirtualKeyCode.SPACE);
                            jumped = true;
                        }
                        simulator.Keyboard.KeyPress(VirtualKeyCode.VK_A);
                        simulator.Keyboard.KeyDown(VirtualKeyCode.VK_A);
                    }
                    else
                    {
                        simulator.Keyboard.KeyUp(VirtualKeyCode.VK_A);
                        NextMove();
                    }
                }
                else
                {
                    if (moveTime < movementDuration)
                    {
                        if (jumped == false)
                        {
                            if (graph[graphIndex + 1].Equals("Glide"))
                            {
                                simulator.Keyboard.KeyPress(VirtualKeyCode.SPACE);
                                simulator.Keyboard.KeyDown(VirtualKeyCode.SPACE);
                            }
                            else
                                simulator.Keyboard.KeyPress(VirtualKeyCode.SPACE);
                            jumped = true;
                        }
                        simulator.Keyboard.KeyPress(VirtualKeyCode.VK_D);
                        simulator.Keyboard.KeyDown(VirtualKeyCode.VK_D);
                    }
                    else
                    {
                        simulator.Keyboard.KeyUp(VirtualKeyCode.VK_D);
                        NextMove();
                    }
                }
                break;
            case "Dash":
                movementDuration = 0.9f;

                if (direction.Equals("left"))
                {
                    if (moveTime < movementDuration)
                    { 
                        if (dashed == false)
                        {
                            simulator.Keyboard.KeyPress(VirtualKeyCode.VK_Q);
                            dashed = true;
                        }

                        if (graph[graphIndex + 1].Equals("Glide") && jumped == false)
                        {
                            simulator.Keyboard.KeyPress(VirtualKeyCode.SPACE);
                            simulator.Keyboard.KeyDown(VirtualKeyCode.SPACE);
                            jumped = true;
                        }
                        else
                        {
                            simulator.Keyboard.KeyDown(VirtualKeyCode.SPACE);
                        }
                    }
                    else
                        NextMove();
                }
                else
                {
                    if (moveTime < movementDuration)
                    {
                        if (dashed == false)
                        {
                            simulator.Keyboard.KeyPress(VirtualKeyCode.VK_E);
                            dashed = true;
                        }

                        if (graph[graphIndex + 1].Equals("Glide") && jumped == false)
                        {
                            simulator.Keyboard.KeyPress(VirtualKeyCode.SPACE);
                            simulator.Keyboard.KeyDown(VirtualKeyCode.SPACE);
                            jumped = true;
                        }
                        else
                        {
                            simulator.Keyboard.KeyDown(VirtualKeyCode.SPACE);
                        }
                    }
                    else
                        NextMove();
                }
                break;
            case "Glide":
                movementDuration = 1f;

                if (direction.Equals("left"))
                {
                    if (moveTime < movementDuration)
                    {
                        simulator.Keyboard.KeyPress(VirtualKeyCode.VK_A);
                        simulator.Keyboard.KeyDown(VirtualKeyCode.VK_A);
                        if (graph[graphIndex - 1].Equals("Dash"))
                            Debug.Log("Dash to Glide");
                        else
                            simulator.Keyboard.KeyDown(VirtualKeyCode.SPACE);
                    }
                    else
                    {
                        simulator.Keyboard.KeyUp(VirtualKeyCode.VK_A);
                        if(graph[graphIndex + 1] != "Glide")
                            simulator.Keyboard.KeyUp(VirtualKeyCode.SPACE);
                        NextMove();
                    }
                } 
                else
                {
                    if (moveTime < movementDuration)
                    {
                        simulator.Keyboard.KeyPress(VirtualKeyCode.VK_D);
                        simulator.Keyboard.KeyDown(VirtualKeyCode.VK_D);
                        if (graph[graphIndex - 1].Equals("Dash"))
                            Debug.Log("Dash to Glide");
                        else
                            simulator.Keyboard.KeyDown(VirtualKeyCode.SPACE);
                    }
                    else
                    {
                        simulator.Keyboard.KeyUp(VirtualKeyCode.VK_D);
                        if (graph[graphIndex + 1] != "Glide")
                            simulator.Keyboard.KeyUp(VirtualKeyCode.SPACE);
                        NextMove();
                    }
                }
                break;
            case "land":
                movementDuration = 1;
                if (moveTime < movementDuration)
                {
                    simulator.Keyboard.KeyUp(VirtualKeyCode.SPACE);
                    simulator.Keyboard.KeyUp(VirtualKeyCode.VK_D);
                    simulator.Keyboard.KeyUp(VirtualKeyCode.VK_A);
                }
                else
                    NextMove();
                break;
            case "start":
                movementDuration = 0.5f;
                if (direction.Equals("left"))
                {

                    if (moveTime < movementDuration)
                    {
                        simulator.Keyboard.KeyPress(VirtualKeyCode.VK_A);
                        simulator.Keyboard.KeyDown(VirtualKeyCode.VK_A);
                    }
                    else
                    {
                        simulator.Keyboard.KeyUp(VirtualKeyCode.VK_A);
                        NextMove();
                    }
                }
                else
                {
                    if (moveTime < movementDuration)
                    {
                        simulator.Keyboard.KeyPress(VirtualKeyCode.VK_D);
                        simulator.Keyboard.KeyDown(VirtualKeyCode.VK_D);
                    }
                    else
                    {
                        simulator.Keyboard.KeyUp(VirtualKeyCode.VK_D);
                        NextMove();
                    }
                }
                break;
            case "fight":
                movementDuration = 7.7f;
                if (direction.Equals("left"))
                {

                    if (moveTime < movementDuration)
                    {
                        simulator.Keyboard.KeyPress(VirtualKeyCode.VK_A);
                        simulator.Keyboard.KeyDown(VirtualKeyCode.VK_A);
                    }
                    else
                    {
                        simulator.Keyboard.KeyUp(VirtualKeyCode.VK_A);
                        NextMove();
                    }
                }
                else
                {
                    if (moveTime < movementDuration)
                    {
                        simulator.Keyboard.KeyPress(VirtualKeyCode.VK_D);
                        simulator.Keyboard.KeyDown(VirtualKeyCode.VK_D);
                    }
                    else
                    {
                        simulator.Keyboard.KeyUp(VirtualKeyCode.VK_D);
                        NextMove();
                    }
                }
                break;
            case "key":
                NextMove();
                break;
            case "lock":
                movementDuration = 2;
                if (direction.Equals("left"))
                {
                    if (moveTime < movementDuration)
                    {
                        simulator.Keyboard.KeyPress(VirtualKeyCode.VK_A);
                        simulator.Keyboard.KeyDown(VirtualKeyCode.VK_A);
                    }
                    else
                    {
                        simulator.Keyboard.KeyUp(VirtualKeyCode.VK_A);
                        NextMove();
                    }
                }
                else
                {
                    if (moveTime < movementDuration)
                    {
                        simulator.Keyboard.KeyPress(VirtualKeyCode.VK_D);
                        simulator.Keyboard.KeyDown(VirtualKeyCode.VK_D);
                    }
                    else
                    {
                        simulator.Keyboard.KeyUp(VirtualKeyCode.VK_D);
                        NextMove();
                    }
                }
                break;
            case "split":
                if (graph[graphIndex + 2].Equals("left"))
                    direction = "left";
                else if (graph[graphIndex + 2].Equals("right"))
                    direction = "right";
                NextMove();
                break;
            case "backToSplit":
                movementDuration = 0.65f;
                if (direction.Equals("left"))
                {
                    if (moveTime < movementDuration)
                    {
                        simulator.Keyboard.KeyPress(VirtualKeyCode.VK_A);
                        simulator.Keyboard.KeyDown(VirtualKeyCode.VK_A);
                    }
                    else
                    {
                        simulator.Keyboard.KeyUp(VirtualKeyCode.VK_A);
                        if (direction.Equals("left"))
                            direction = "right";
                        else if (direction.Equals("right"))
                            direction = "left";
                        NextMove();
                    }
                }
                else
                {
                    if (moveTime < movementDuration)
                    {
                        simulator.Keyboard.KeyPress(VirtualKeyCode.VK_D);
                        simulator.Keyboard.KeyDown(VirtualKeyCode.VK_D);
                    }
                    else
                    {
                        simulator.Keyboard.KeyUp(VirtualKeyCode.VK_D);
                        if (direction.Equals("left"))
                            direction = "right";
                        else if (direction.Equals("right"))
                            direction = "left";
                        NextMove();
                    }
                }
                break;
            case "goal":
                movementDuration = 1.66f;
                if (direction.Equals("left"))
                {
                    if (moveTime < movementDuration)
                    {
                        simulator.Keyboard.KeyPress(VirtualKeyCode.VK_A);
                        simulator.Keyboard.KeyDown(VirtualKeyCode.VK_A);
                    }
                    else
                    {
                        simulator.Keyboard.KeyUp(VirtualKeyCode.VK_A);
                        if (direction.Equals("left"))
                            direction = "right";
                        else if (direction.Equals("right"))
                            direction = "left";
                        NextMove();
                    }
                }
                else
                {
                    if (moveTime < movementDuration)
                    {
                        simulator.Keyboard.KeyPress(VirtualKeyCode.VK_D);
                        simulator.Keyboard.KeyDown(VirtualKeyCode.VK_D);
                    }
                    else
                    {
                        simulator.Keyboard.KeyUp(VirtualKeyCode.VK_D);
                        if (direction.Equals("left"))
                            direction = "right";
                        else if (direction.Equals("right"))
                            direction = "left";
                        NextMove();
                    }
                }
                break;
            default:
                NextMove();
                break;
        }

        moveTime += Time.deltaTime;
    }

    private void NextMove()
    {
        if(graphIndex < graph.Count -1)
        {
            moveTime = 0; // reset time
            jumped = false;
            dashed = false;
            graphIndex++;
        }
        currentMove = graph[graphIndex];
    }
}
