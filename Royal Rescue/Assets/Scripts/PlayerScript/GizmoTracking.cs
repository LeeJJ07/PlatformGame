using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmoTracking : MonoBehaviour
{
    public Vector2Int minXAndY;
    public Vector2Int maxXAndY;
    public Color maincolor = new Color(0f, 1f, 1f, 1f);

    public Mesh titleMesh;
    [Range(0f, 10f)]
    public float titleXpos = 1.0f;

    [Range(-1f, 1f)]
    public float titleYpos = 1.0f;

    [Range(0f, 1f)]
    public float titleSize = 1.0f;

    private void OnDrawGizmos()
    {
        Color subColor = new Color(maincolor.r, maincolor.g, maincolor.b, maincolor.r + 0.1f);//보조컬러
        Vector3 titlePos = new Vector3(maxXAndY.x - titleXpos, maxXAndY.y - titleYpos, 0.0f);
        Vector3 titleScale = new Vector3(titleSize, titleSize, titleSize);

        Gizmos.DrawMesh(titleMesh, titlePos, transform.rotation, titleScale);
        for(int x = minXAndY.x; x <= maxXAndY.x; x++)
        {
            if(x - maxXAndY.x == 0 || x - minXAndY.x == 0)
            {
                Gizmos.color = maincolor;
            }
            else
            {
                Gizmos.color = subColor;
            }
            Vector3 pos1 = new Vector3(x, minXAndY.y, 0);
            Vector3 pos2 = new Vector3(x, maxXAndY.y, 0);
            Gizmos.DrawLine(pos1, pos2);
        }
        for (int y = minXAndY.y; y <= maxXAndY.y; y++)
        {
            if (y - maxXAndY.y == 0 || y - minXAndY.y == 0)
            {
                Gizmos.color = maincolor;
            }
            else
            {
                Gizmos.color = subColor;
            }
            Vector3 pos1 = new Vector3(minXAndY.x, y, 0);
            Vector3 pos2 = new Vector3(maxXAndY.x, y, 0);
            Gizmos.DrawLine(pos1, pos2);
        }
    }
}
