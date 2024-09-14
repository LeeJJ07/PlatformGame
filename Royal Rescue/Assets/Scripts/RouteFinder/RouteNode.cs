using UnityEngine;

public class RouteNode
{
    public Vector3 posi;
    public bool isAblePass = true;
    public RouteNode parent;
    public float hCost;
    public float gCost;
    public int xIndex, yIndex;
    

    public RouteNode(Vector3 posi, bool isAblePass,int xIndex, int yIndex)
    {
        this.posi = posi;
        this.isAblePass = isAblePass;
        this.xIndex = xIndex;
        this.yIndex = yIndex;
    }
 

} 
