using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickEffect : MonoBehaviour
{
    public GameObject pointEffectPF;
    public GameObject annulusEffectPF;

    private GameObject lastPoint;

    public float annulusAnimLength = 0.67f;
    private WaitForSeconds ws;

    private Camera _uiCamera;

    private void Start()
    {
        foreach(Camera item in FindObjectsOfType<Camera>()) {
            if(item.name == "Annulus Camera") {
                _uiCamera = item;
            }
        }

        if(_uiCamera == null) {
            Debug.LogError("Annulus Camera not found");
        }

        ws = new WaitForSeconds(annulusAnimLength);
    }

    public void GenerateEffect(Transform nodeTF)
    {
        if(lastPoint != null) {
            GameObjectPool.Instance.ObjectCollect("PointEffect", lastPoint);
        }

        lastPoint = GameObjectPool.Instance.Obtain("PointEffect", pointEffectPF, nodeTF.position + nodeTF.up * 0.01f, nodeTF.rotation * Quaternion.Euler(90, 0, 0));
        lastPoint.transform.parent = nodeTF;
        GameObject go = GameObjectPool.Instance.Obtain("AnnulusEffect", annulusEffectPF, nodeTF.position, _uiCamera.transform.rotation);

        StartCoroutine(CollectAnnulusEffect(go));
    }

    public IEnumerator CollectAnnulusEffect(GameObject obj)
    {
        yield return ws;
        GameObjectPool.Instance.ObjectCollect("AnnulusEffect", obj);
    }
}
