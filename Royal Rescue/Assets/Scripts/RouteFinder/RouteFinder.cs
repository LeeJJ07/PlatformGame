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
        //1. �÷��̾��� ��ġ ã��(��� ��ġ)
        //2. �÷��̾� �����ġ�� �������� �̿������ ����ġ+�޸���ƽ �� ���ϱ�
        //3. �޸���ƽ�� ���� ���� ã�� closeSet���� ������ ã�� ����� �̿������� ����ġ+�޸���ƽ �� ���ϱ�
        //4.�ݺ�
        //5. �ݺ� �� ���������� �����ϸ� parent������ ���� ����Ʈ�� ��� ��ȯ�ϱ�
        RouteNode curNode =null;
        RouteNode endNode =null;
        List<Vector3> routes = new List<Vector3>();
        
        //�÷��̾��� ��ġ�� ��� ��ġ ���ϱ�
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
