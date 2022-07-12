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
                "Entrance-Task-Goal",
                "Entrance-Task-Task-Goal",
                "Entrance-Task-Task-Task-Goal"
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
                "ExtendedMovement-fight",
                "BasicMovement-ExtendedMovement-fight",
                "ExtendedMovement-BasicMovement-fight",
                "BasicMovement-ExtendedMovement-ExtendedMovement-fight"
        };

        Rule AddMovement2 = new Rule();
        AddMovement2.LeftHand = new String[] { "Key" };
        AddMovement2.RightHand = new String[]
        {
                "BasicMovement-key",
                "ExtendedMovement-key",
                "BasicMovement-ExtendedMovement-key",
                "ExtendedMovement-BasicMovement-key",
                "BasicMovement-ExtendedMovement-ExtendedMovement-key"
        };

        Rule AddMovement3 = new Rule();
        AddMovement3.LeftHand = new String[] { "Lock" };
        AddMovement3.RightHand = new String[]
        {
                "BasicMovement-lock",
                "ExtendedMovement-lock",
                "BasicMovement-ExtendedMovement-lock",
                "ExtendedMovement-BasicMovement-lock",
                "BasicMovement-ExtendedMovement-ExtendedMovement-lock"
        };

        Rule BasicMovement = new Rule();
        BasicMovement.LeftHand = new String[] { "BasicMovement" };
        BasicMovement.RightHand = new String[]
        {

                "left-jump",
                "right-jump",
        };

        Rule ExtendedMovement = new Rule();
        ExtendedMovement.LeftHand = new String[] { "ExtendedMovement" };
        ExtendedMovement.RightHand = new String[]
        {
                "dash",
                "glide",
                "extraJump"
        };

        Rule DoubleJump1 = new Rule();
        DoubleJump1.LeftHand = new String[] { "jump-extraJump" };
        DoubleJump1.RightHand = new String[] { "doubleJump" };

        Rule DoubleJump2 = new Rule();
        DoubleJump2.LeftHand = new String[] { "extraJump-jump" };
        DoubleJump2.RightHand = new String[] { "doubleJump" };

        Rule DoubleDash = new Rule();
        DoubleDash.LeftHand = new String[] { "dash-dash" };
        DoubleDash.RightHand = new String[] { "doubleDash" };


        ProductionRules = new Rule[] { Start, AddTask, AddEndTask, AddMovement1, AddMovement2, AddMovement3, BasicMovement, ExtendedMovement, DoubleJump1, DoubleJump2, DoubleDash };
    }
}
