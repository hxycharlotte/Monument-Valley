using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ActiveEdge
{
    public PathNode node;
    public int index;
}

public class Rotator : MonoBehaviour
{
    public bool dragable = true;

    [Header("激活路径列表")]
    public List<ActiveEdge> activeEdgeList0;
    public List<ActiveEdge> activeEdgeList90;
    public List<ActiveEdge> activeEdgeList180;
    public List<ActiveEdge> activeEdgeList270;

    /// <summary>
    /// 轴心点的屏幕坐标
    /// </summary>
    private Vector2 originPos;

    private bool isDraging;
    private bool isSteady = true;

    private float targetY;

    private void Start()
    {
        originPos = Camera.main.WorldToScreenPoint(this.transform.position);
        cylinder1TF = transform.FindChildByName("Cylinder1");
        cylinder2TF = transform.FindChildByName("Cylinder2");
    }

    private bool lastSteadyState = true;

    private void Update()
    {
        if (!isDraging) {
            float curY = Mathf.Lerp(transform.localEulerAngles.y, targetY, 0.08f);

            this.transform.localEulerAngles = new Vector3(0, curY, 0);

            if(Mathf.Abs(curY - targetY) < 1) {
                this.transform.localEulerAngles = new Vector3(0, targetY, 0);
                isSteady = true;
            }

            //上一帧不稳定，这一帧稳定触发，激活当前对应的边
            if(isSteady && lastSteadyState == false) {
                //UnActiveAllEdge();

                float curLocalEuler = this.transform.localEulerAngles.y;

                if (curLocalEuler >= 315 || curLocalEuler < 45) {
                    SetEdgeListState(activeEdgeList0, true);
                }
                else if (curLocalEuler >= 45 && curLocalEuler < 135) {
                    SetEdgeListState(activeEdgeList90, true);
                }
                else if (curLocalEuler >= 135 && curLocalEuler < 225) {
                    SetEdgeListState(activeEdgeList180, true);
                }
                else {
                    SetEdgeListState(activeEdgeList270, true);
                }
            }
        }
        else if(dragable) {
            this.transform.localRotation = Quaternion.Lerp(transform.localRotation, aimRotation, 0.3f);
        }

        lastSteadyState = isSteady;
    }

    private Vector2 startDirection;
    private Quaternion startRotation;
    private Quaternion aimRotation;

    private void OnMouseDown()
    {
        if (!dragable) return;

        isDraging = true;
        isSteady = false;

        startDirection = (Vector2)Input.mousePosition - originPos;
        startRotation = transform.localRotation;

        UnActiveAllEdge();
    }

    private void OnMouseDrag()
    {
        if (!dragable) return;

        Vector2 currentDirection = (Vector2)Input.mousePosition - originPos;

        float angle = Vector2.Angle(startDirection, currentDirection);

        //this.transform.Rotate(Vector3.Cross(startDirection, currentDirection).normalized.z * Vector3.up * angle, Space.Self);

        aimRotation = startRotation * Quaternion.Euler(Vector3.Cross(startDirection, currentDirection).normalized.z * Vector3.up * angle);
    }

    private void OnMouseUp()
    {
        isDraging = false;

        float rotationY = this.transform.localEulerAngles.y;

        if(rotationY >= 315 || rotationY < 45) {
            targetY = 0;
            if (rotationY >= 315) targetY = 359.9f;
        }
        else if(rotationY >= 45 && rotationY < 135) {
            targetY = 90;
        }
        else if(rotationY >= 135 && rotationY < 225) {
            targetY = 180;
        }
        else {
            targetY = 270f;
        }
    }

    private void SetEdgeListState(List<ActiveEdge> list, bool state)
    {
        foreach(ActiveEdge ae in list) {
            ae.node.edgeList[ae.index].isActive = state;
        }
    }

    private void UnActiveAllEdge()
    {
        SetEdgeListState(activeEdgeList0, false);
        SetEdgeListState(activeEdgeList90, false);
        SetEdgeListState(activeEdgeList180, false);
        SetEdgeListState(activeEdgeList270, false);
    }

    private Transform cylinder1TF;
    private Transform cylinder2TF;

    public void SetDragable(bool state)
    {
        if(state == true) {
            cylinder1TF.DOScaleY(1, 0.5f);
            cylinder2TF.DOScaleY(1, 0.5f);
        }
        else {
            cylinder1TF.DOScaleY(0.6f, 0.5f);
            cylinder2TF.DOScaleY(0.6f, 0.5f);
        }
        dragable = state;
    }
}
