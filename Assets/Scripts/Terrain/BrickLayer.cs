using UnityEngine;
using System.Collections;

public enum BrickLayerCondition
{
    None,
    Core,
    Middle,
    BelowGroundLevel,
    GroundLevel
}


[System.Serializable]
public class BrickLayer
{
    public string name = "Unnamed BrickLayer";
    public BlockType brick;
    public float weight = 1;
    public BrickLayerCondition condition;

    public virtual float Bid(Vector3 pos, float noiseValue, BrickLayerCondition layer)
    {
        float bid = 0;

        //for(int a = 0; a < conditions.Length; a++)
        //{
        switch (layer)
        {
            case BrickLayerCondition.Core:
                bid++;
                break;
            case BrickLayerCondition.Middle:
                bid += 2;
                break;
            case BrickLayerCondition.BelowGroundLevel:
                bid += 3;
                break;
            case BrickLayerCondition.GroundLevel:
                bid += 4;
                break;
        }
        //}

        if (weight == 0) return bid;
        return bid * weight;
    }
}
