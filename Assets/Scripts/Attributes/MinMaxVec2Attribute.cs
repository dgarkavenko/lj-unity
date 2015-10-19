using UnityEngine;

public class MinMaxVec2Attribute : PropertyAttribute
{
    public readonly float Min;
    public readonly float Max;

    public readonly float Round;
    public readonly float InvRound;
    public readonly bool UseRound;

    public MinMaxVec2Attribute(float min, float max, float round = -1)
    {
        Min = min;
        Max = max;

        UseRound = round > 0;
        Round = 1 / round;
        InvRound = round;
    }
}