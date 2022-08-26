using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Section : MonoBehaviour
{
    public float upBound;
    public float downBound;
    public float leftBound;
    public float rightBound;

    public int numberOfGlides;
    public bool changedDirection;

    public GameObject Floor;
    public GameObject Ceiling;
    public GameObject LeftWallBottom;
    public GameObject LeftWallTop;
    public GameObject RightWallBottom;
    public GameObject RightWallTop;

    public void GenerateSectionWalls(string Direction, string PreviousDirection)
    {
        float upBoundOffset = numberOfGlides * 1.25f;
        if (changedDirection)
        {
            if(Direction.Equals("left"))// went right first then left
            {
                Floor.transform.localScale = new Vector3(rightBound, 0.5f, 0);
                Floor.transform.localPosition = new Vector3((rightBound/2)+1, 0, 0);

                RightWallBottom.transform.localScale = new Vector3(0.5f, upBound + 5f, 0);
                RightWallBottom.transform.localPosition = new Vector3(rightBound + 1.25f, (upBound/2) + 2.25f, 0);

                Ceiling.transform.localScale = new Vector3(leftBound, 0.5f, 0);
                Ceiling.transform.localPosition = new Vector3( rightBound - (leftBound/2) +1, upBound + 4.5f, 0);
            }
            else  // went left first then right
            {
                Floor.transform.localScale = new Vector3(leftBound, 0.5f, 0);
                Floor.transform.localPosition = new Vector3((-leftBound / 2) - 1, 0, 0);

                LeftWallBottom.transform.localScale = new Vector3(0.5f, upBound + 5f, 0);
                LeftWallBottom.transform.localPosition = new Vector3(-leftBound - 1.25f, (upBound / 2) + 2.25f, 0);

                Ceiling.transform.localScale = new Vector3(rightBound, 0.5f, 0);
                Ceiling.transform.localPosition = new Vector3(-leftBound + (rightBound / 2) - 1, upBound + 4.5f, 0);
            }
        }
        else
        {
            if (leftBound == 0) //going right
            {
                Floor.transform.localScale = new Vector3(rightBound, 0.5f, 0);
                Floor.transform.localPosition = new Vector3((rightBound / 2) + 1, 0, 0);

                Ceiling.transform.localScale = new Vector3(rightBound, 0.5f, 0);
                Ceiling.transform.localPosition = new Vector3((rightBound / 2) + 1, upBound + 4.5f, 0);

                RightWallBottom.transform.localScale = new Vector3(0.5f, upBound - upBoundOffset, 0);
                RightWallBottom.transform.localPosition = new Vector3(rightBound + 1.25f, ((upBound - upBoundOffset) / 2) - 0.25f, 0);

                LeftWallTop.transform.localScale = new Vector3(0.5f, upBound, 0);
                LeftWallTop.transform.localPosition = new Vector3(1.25f, (upBound / 2) - 0.25f + 4.5f, 0);

                if (numberOfGlides > 0)
                {
                    RightWallTop.transform.localScale = new Vector3(0.5f, upBoundOffset + 0.5f, 0);
                    RightWallTop.transform.localPosition = new Vector3(rightBound + 1.25f, (upBound - upBoundOffset / 2) + 4.5f, 0);
                }
            }
            else if (rightBound == 0) // going left
            {
                Floor.transform.localScale = new Vector3(leftBound, 0.5f, 0);
                Floor.transform.localPosition = new Vector3((-leftBound / 2) - 1.25f, 0, 0);

                Ceiling.transform.localScale = new Vector3(leftBound, 0.5f, 0);
                Ceiling.transform.localPosition = new Vector3((-leftBound / 2) - 1, upBound + 4.5f, 0);

                LeftWallBottom.transform.localScale = new Vector3(0.5f, upBound - upBoundOffset, 0);
                LeftWallBottom.transform.localPosition = new Vector3(-leftBound - 1.25f, ((upBound - upBoundOffset) / 2) - 0.25f, 0);

                RightWallTop.transform.localScale = new Vector3(0.5f, upBound, 0);
                RightWallTop.transform.localPosition = new Vector3(-1.25f, (upBound / 2) - 0.25f + 4.5f, 0);

                if (numberOfGlides > 0)
                {
                    LeftWallTop.transform.localScale = new Vector3(0.5f, upBoundOffset + 0.5f, 0);
                    LeftWallTop.transform.localPosition = new Vector3(-leftBound - 1.25f, upBound - upBoundOffset / 2 + 4.5f, 0);
                }
            }
        }
    }
}
