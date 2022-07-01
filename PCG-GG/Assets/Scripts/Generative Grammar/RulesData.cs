using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RulesData : MonoBehaviour
{
    public static Rule[] ProductionRules;

    public static void Setup()
    {
        //Uppercase first letter = non terminal
        //Lowercase first letter = Terminal

        Rule Start = new Rule();
        Start.LeftHand = new String[] { "Start" };
        Start.RightHand = new String[]
        {
                "Entrance-Task-Goal"
        };

        Rule AddTask = new Rule();
        AddTask.LeftHand = new String[] { "Task" };
        AddTask.RightHand = new String[]
        {
                "Fight",
                "Key-Lock"
        };

        Rule AddEndTask = new Rule();
        AddEndTask.LeftHand = new String[] { "Task-Goal" };
        AddEndTask.RightHand = new String[]
        {
                "Fight-Goal",
                "Key-Lock-Goal"
        };

        //--------------------Movement Rules---------------------------

        Rule AddMovement1 = new Rule();
        AddMovement1.LeftHand = new String[] { "Fight" };
        AddMovement1.RightHand = new String[]
        {
                "BasicMovement-fight",
                "ExtendedMovement-fight"
        };

        Rule AddMovement2 = new Rule();
        AddMovement2.LeftHand = new String[] { "Key" };
        AddMovement2.RightHand = new String[]
        {
                "BasicMovement-key",
                "ExtendedMovement-key"
        };

        Rule AddMovement3 = new Rule();
        AddMovement3.LeftHand = new String[] { "Lock" };
        AddMovement3.RightHand = new String[]
        {
                "BasicMovement-lock",
                "ExtendedMovement-lock"
        };

        Rule BasicMovement = new Rule();
        BasicMovement.LeftHand = new String[] { "BasicMovement" };
        BasicMovement.RightHand = new String[]
        {
                "left-Jump",
                "right-Jump"
        };

        Rule ExtendedMovement = new Rule();
        ExtendedMovement.LeftHand = new String[] { "ExtendedMovement" };
        ExtendedMovement.RightHand = new String[]
        {
                "Dash",
                "DoubleDash",
                "DoubleJump",
                "Glide"
        };


        ProductionRules = new Rule[] { Start, AddTask, AddEndTask, AddMovement1, AddMovement2, AddMovement3, BasicMovement, ExtendedMovement };
    }

    
}
