using System;

public class Rule
{
    public String LeftHand { get; set; }
    public String[] RightHand { get; set; }

    public Rule(String Left, String[] Right)
    {
        this.LeftHand = Left;
        this.RightHand = Right;
    }

    public Rule()
    {

    }

}
