using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField]
    private int numberOfJumps = 2;

    [SerializeField]
    private int numberOfDashes = 2;

    [SerializeField]
    private float glideTimer = 1f;
    public int Jumps
    {
        get
        {
            return numberOfJumps;
        }
    }

    public int Dashes
    {
        get
        {
            return numberOfDashes;
        }
    }

    public float Glide
    {
        get
        {
            return glideTimer;
        }
    }
}
