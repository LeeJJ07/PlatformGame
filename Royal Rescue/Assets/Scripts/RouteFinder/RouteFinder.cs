using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RouteFinder : MonoBehaviour
{
    [SerializeField] Transform StartPosi;
    [SerializeField] Transform EndPosi;
    [SerializeField] float NodeSize;
    [SerializeField] int NodeGap;
    List<RouteNode> nodeList = new List<RouteNode>();
    List<RouteNode> openSet = new List<RouteNode>();
    List<RouteNode> closeSet = new List<RouteNode>();
    int mapWidth;
    int mapHeight;


    /*
     * 1. 노드 생성
     * 2. target위치와 player의 위치를 노드위치로 변환 후 찾기
     * 3. AStar알고리즘 적용
     */
    void Awake()
    {
        mapWidth = (int)(EndPosi.position.x-StartPosi.position.x);
        mapHeight = (int)(EndPosi.position.y - StartPosi.position.y);
        CreateRouteNodes();
    }
    void CreateRouteNodes()
    {
        int widthIndex, heightIndex;
        for(widthIndex= NodeGap; widthIndex < mapWidth; widthIndex+= NodeGap)
        {
            for (heightIndex = NodeGap; heightIndex < mapHeight; heightIndex+= NodeGap)
            {
                Vector3 nodePosi = new Vector3(StartPosi.position.x + widthIndex, StartPosi.position.y + heightIndex, 0);
                Collider[] detectObjs = Physics.OverlapSphere(nodePosi, NodeSize);
                if (detectObjs.Length > 0)
                {
                    nodeList.Add(new RouteNode(nodePosi, null, false));
                }
                else
                {
                    nodeList.Add(new RouteNode(nodePosi, null, true));
                }
            }
        }
        for(int i =0; i<nodeList.Count; i++)
        {
            foreach(RouteNode node in nodeList)
            {
                if (nodeList[i] == node) continue;
                RouteNode neighbor = node.FindRouteNode(nodeList[i].posi, NodeSize);
                if(neighbor != null)
                    nodeList[i].neighborNodes.Add(neighbor);
            }
        }
    }
    public List<Vector3> FindRoute(Vector3 curPosition, Vector3 TargetPosition)
    {
        //1. 플레이어의 위치 찾기(노드 위치)
        //2. 플레이어 노드위치를 기준으로 이웃노드의 가중치+휴리스틱 값 구하기
        //3. 휴리스틱이 낮은 값을 찾아 closeSet으로 보내고 찾은 노드의 이웃노드들의 가중치+휴리스틱 값 구하기
        //4.반복
        //5. 반복 중 목적지까지 도착하면 parent노드들의 값을 리스트에 담아 반환하기
        RouteNode curNode =null;
        RouteNode endNode =null;
        List<Vector3> routes = new List<Vector3>();
        
        //플레이어의 위치로 노드 위치 구하기
        foreach(RouteNode node in nodeList)
        {
            curNode = node.FindRouteNode(curPosition, NodeSize);
            if (curNode!=null) break;
            
        } 
        foreach(RouteNode node in nodeList)
        {
            endNode = node.FindRouteNode(TargetPosition, NodeSize);
            if (curNode!=null) break;
            
        }
        if (curNode == null||endNode==null) return new List<Vector3>();
        //openSet
        while (true)
        {
            if (curNode == endNode) break;
            foreach (RouteNode neighbor in curNode.neighborNodes)
            {
                neighbor.gCost = Vector3.Distance(curNode.posi, neighbor.posi);
                neighbor.hCost = Huristic(curNode, neighbor);
                openSet.Add(neighbor);
            }
            RouteNode minNode=openSet[0];
            foreach(RouteNode node in openSet)
            {
                if((node.gCost+node.hCost)<(minNode.gCost+minNode.hCost))
                {
                    minNode = node;
                }
            }
            minNode.parent = curNode;
            curNode = minNode;
            closeSet.Add(minNode);
        }
        closeSet.Reverse();
        foreach(RouteNode pos in closeSet)
        {
            routes.Add(pos.parent.posi);
        }
        return routes;
    }
    float Huristic(RouteNode curPosi, RouteNode targetPosi)
    {
        if (curPosi.posi.y != targetPosi.posi.y)
            return 14 + targetPosi.gCost;
        else
            return 10 + targetPosi.gCost;
    }
    private void OnDrawGizmos()
    {
        if(nodeList.Count!=0)
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
