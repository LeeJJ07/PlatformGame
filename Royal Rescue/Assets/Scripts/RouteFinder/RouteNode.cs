using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class RouteNode
{
    public Vector3 posi;
    public bool isAblePass = true;
    public RouteNode parent;
    public List<RouteNode> neighborNodes;
    public float hCost;
    public float gCost;
    

    public RouteNode(Vector3 posi, List<RouteNode> neighborNodes, bool isAblePass)
    {
        this.posi = posi;
        this.neighborNodes = neighborNodes;
        this.isAblePass = isAblePass;
    }
    public RouteNode FindRouteNode(Vector3 position,float size)
    {
        if (position.x <=  posi.x+size&&position.x>=posi.x-size)
        {
            if(position.y<=posi.y+size&&position.y>=posi.y-size)
            {
                return this;
            }
        }
        return null;
    }

} 
