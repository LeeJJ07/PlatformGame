using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR;

public class RouteFinder : MonoBehaviour
{
    [SerializeField] Transform StartPosi;
    [SerializeField] Transform EndPosi;
    [SerializeField] float NodeSize;
    [SerializeField] int NodeGap;
    RouteNode[,] nodeList;
    
    List<RouteNode> openSet = new List<RouteNode>();
    List<RouteNode> closeSet = new List<RouteNode>();
    int mapWidth;
    int mapHeight;


    /*
     * 1. ��� ����
     * 2. target��ġ�� player�� ��ġ�� �����ġ�� ��ȯ �� ã��
     * 3. AStar�˰��� ����
     */
    void Awake()
    {
        mapWidth = (int)(EndPosi.position.x-StartPosi.position.x);
        mapHeight = (int)(EndPosi.position.y - StartPosi.position.y);
        CreateRouteNodes();
    }
    void CreateRouteNodes()
    {
        int xIndex, yIndex;
        nodeList = new RouteNode[mapWidth/ NodeGap, mapHeight/ NodeGap];

        /*
            for (widthIndex= NodeGap; widthIndex < mapWidth; widthIndex+= NodeGap)
            for (heightIndex = NodeGap; heightIndex < mapHeight; heightIndex+= NodeGap)
         */
        Debug.Log($"xIndex: {mapWidth / NodeGap}, yIndex: {mapHeight / NodeGap}");
        for (xIndex= 1; xIndex <= mapWidth / NodeGap; xIndex++)
        {
            for (yIndex = 1; yIndex <= mapHeight / NodeGap; yIndex++)
            {
                Vector3 nodePosi = new Vector3(StartPosi.position.x + (xIndex*NodeGap), StartPosi.position.y + (yIndex * NodeGap), 0);
                Collider[] detectObjs = Physics.OverlapSphere(nodePosi, NodeSize);
                if (detectObjs.Length > 0)
                {
                    nodeList[xIndex - 1, yIndex - 1] = new RouteNode(nodePosi, false, xIndex, yIndex);
                }
                else
                {
                    nodeList[xIndex - 1, yIndex - 1] = new RouteNode(nodePosi, true, xIndex, yIndex);
                }
            }
        }
    }
   public List<Vector3> FindRoute(Vector3 curPosition, Vector3 TargetPosition)
    {
        //1. �÷��̾��� ��ġ ã��(��� ��ġ)
        //2. �÷��̾� �����ġ�� �������� �̿������ ����ġ+�޸���ƽ �� ���ϱ�
        //3. �޸���ƽ�� ���� ���� ã�� closeSet���� ������ ã�� ����� �̿������� ����ġ+�޸���ƽ �� ���ϱ�
        //4.�ݺ�
        //5. �ݺ� �� ���������� �����ϸ� parent������ ���� ����Ʈ�� ��� ��ȯ�ϱ�
        RouteNode curNode =null;
        RouteNode endNode =null;
        List<Vector3> routes = new List<Vector3>();
        
        //�÷��̾��� ��ġ�� ��� ��ġ ���ϱ�
        curNode = FindRouteNode(curPosition);
        endNode = FindRouteNode(TargetPosition);

        if (curNode == null||endNode==null) return new List<Vector3>();
        curNode.parent = null;
        closeSet.Add(curNode);
        int count = 0;
        //openSet
        while (true)
        {
            if (count == 100) break;
            count++;
            if (curNode == endNode) break;
            foreach(RouteNode neighbor in FindNeighbors(curNode))
            {
                if (!neighbor.isAblePass) continue;
                float neighborCost = neighbor.gCost+Heuristic(curNode,neighbor);
                if (openSet.Contains(neighbor))
                {
                    foreach (RouteNode node in openSet)
                    {
                        if (neighbor != node) continue;
                        if ((node.gCost + Heuristic(curNode, node)) > neighborCost)
                        {
                            openSet.Remove(node);
                            openSet.Add(neighbor);
                        }
                    }
                }
                else
                {
                    openSet.Add(neighbor);
                }
            }
            RouteNode route=openSet[0];
            for(int i=1; i<openSet.Count; i++)
            {
                if ((openSet[i].gCost + Heuristic(curNode, openSet[i])<(route.gCost+Heuristic(curNode,route))))
                {
                    route = openSet[i];
                }
            }
            route.parent = curNode;
            curNode = route;
            closeSet.Add(route);
        }
        foreach(RouteNode pos in closeSet)
        {
            routes.Add(pos.parent.posi);
        }
        return routes;
    }

    RouteNode FindRouteNode(Vector3 posi)
    {
        float xPos = (posi.x + mapWidth / NodeSize) / mapWidth;
        float yPos = (posi.x + mapHeight / NodeSize) / mapHeight;
        xPos = Mathf.Clamp01(xPos);
        yPos = Mathf.Clamp01(yPos);
        int xIndex = Mathf.RoundToInt(xPos);
        int yIndex = Mathf.RoundToInt(yPos);
        return nodeList[xIndex,yIndex];
    }
    List<RouteNode> FindNeighbors(RouteNode node)
    {
        List<RouteNode> neighbors=new List<RouteNode>();
        int xPos, yPos;
        int xIndex, yIndex;
        for (xIndex = -1; xIndex <= 1; xIndex++) 
        {
            for (yIndex = -1; yIndex <= 1; yIndex++) 
            {
                if (xIndex == 0 && yIndex == 0) continue;
                xPos = node.xIndex + xIndex;
                yPos = node.yIndex + yIndex;
                if(xPos>=0&&xPos< (mapWidth / NodeGap) &&
                    yPos>=0&&yPos<(mapHeight / NodeGap))
                {
                    neighbors.Add(nodeList[xPos, yPos]);
                }
            }
        }
        return neighbors;
    }

    float Heuristic(RouteNode node, RouteNode neighborNode)
    {
        if (((neighborNode.xIndex- node.xIndex) != -1 || (neighborNode.xIndex- node.xIndex) !=1) 
            && ((neighborNode.yIndex- node.yIndex) != -1 || (neighborNode.yIndex- node.yIndex) != 1))
            return 10 + node.gCost;
        else
            return 14 + node.gCost;
    }
    
    private void OnDrawGizmos()
    {
        if(nodeList!=null)
        foreach(var node in nodeList)
        {
            if(node.isAblePass)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(node.posi, NodeSize);
            }
            else
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(node.posi, NodeSize);
            }
        }
    }
}
