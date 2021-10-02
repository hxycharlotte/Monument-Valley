using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Edge
{
    public bool isActive;
    public PathNode toNode;
}

public class PathNode : MonoBehaviour
{
    //该结点邻接的边
    public List<Edge> edgeList;

    //PathManager 计算路径需要记录前驱结点与边
    [HideInInspector]
    public PathNode fromNode;
    [HideInInspector]
    public Edge fromEdge;

    private void OnDrawGizmosSelected()
    {
        if (edgeList != null) {
            int length = edgeList.Count;
            for (int i = 0; i < length; ++i) {
                if (edgeList[i].toNode != null && edgeList[i].isActive) {
                    Debug.DrawLine(transform.position, edgeList[i].toNode.transform.position, Color.red);
                }
            }
        }
        Gizmos.DrawCube(transform.position, new Vector3(0.2f, 0.2f, 0.2f));
    }
}
