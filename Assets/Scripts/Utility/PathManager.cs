using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathManager : MonoSingleton<PathManager>
{
    public void FindPath(Agent agent, PathNode targetNode)
    {
        List<Edge> path = GetPath(agent.currentNode, targetNode);

        if (path == null) return;

        //停止上一次的寻路
        StopAllCoroutines();
        StartCoroutine(Move(agent, path));
    }

    private List<Edge> GetPath(PathNode currentNode, PathNode targetNode)
    {
        Dictionary<PathNode, bool> visitedDic = new Dictionary<PathNode, bool>();
        Queue<PathNode> q = new Queue<PathNode>();

        currentNode.fromEdge = null;
        currentNode.fromNode = null;
        targetNode.fromNode = null;
        targetNode.fromEdge = null;

        q.Enqueue(currentNode);
        visitedDic.Add(currentNode, true);

        while(q.Count > 0) {
            PathNode last = q.Dequeue();

            foreach(Edge edge in last.edgeList) {
                PathNode node = edge.toNode;

                if (!visitedDic.ContainsKey(node) && edge.isActive) {
                    node.fromNode = last;
                    node.fromEdge = edge;
                    q.Enqueue(node);
                    visitedDic.Add(node, true);
                }
            }
        }

        if(targetNode.fromNode == null) {
            return null;
        }

        List<Edge> path = new List<Edge>();

        BackTrace(path, targetNode);

        return path;
    }

    private void BackTrace(List<Edge> path, PathNode curNode)
    {
        if(curNode.fromNode != null) {
            BackTrace(path, curNode.fromNode);
            path.Add(curNode.fromEdge);
        }
    }

    private IEnumerator Move(Agent agent, List<Edge> path)
    {
        foreach(Edge edge in path){

            PathNode node = edge.toNode;

            //根据边长动态调整移动速度
            float speed = Vector3.Distance(node.transform.position, agent.currentNode.transform.position) * agent.moveSpeed;

            while (Vector3.Distance(agent.transform.position, node.transform.position) > 0.05f) {

                //寻路途中路径被禁用
                if (!edge.isActive) yield break;

                agent.transform.position = Vector3.MoveTowards(agent.transform.position, node.transform.position, speed * Time.deltaTime);

                yield return null;
            }

            agent.currentNode = node;
        }
    }
}
