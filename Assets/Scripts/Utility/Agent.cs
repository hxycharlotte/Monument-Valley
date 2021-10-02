using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    public float moveSpeed = 2f;

    public PathNode currentNode;
    public PathNode targetNode;

    private Camera _camera;
    private ClickEffect _effect;

    private void Start()
    {
        _camera = Camera.main;
        _effect = GetComponent<ClickEffect>();

        RaycastHit hit;

        if(Physics.Raycast(transform.GetChild(0).position, -transform.up, out hit, 1, LayerMask.GetMask("PathNode"))) {
            currentNode = hit.transform.GetComponent<PathNode>();
        }
        if(currentNode == null) {
            Debug.LogError("agent is not on any path node");
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 1000, LayerMask.GetMask("PathNode"))) {
                targetNode = hit.transform.GetComponent<PathNode>();

                if(targetNode != null) {
                    PathManager.Instance.FindPath(this, targetNode);

                    _effect.GenerateEffect(targetNode.transform);
                }
            }
        }
    }
}
