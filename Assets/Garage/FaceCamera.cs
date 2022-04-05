using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class FaceCamera : MonoBehaviour
{
    RectTransform myRectTransform;
    private float coficienResizeDistance = 1f;

    Vector3 origScale;
    void Start()
    {
        myRectTransform = gameObject.GetComponent<RectTransform>();
        origScale = myRectTransform.localScale;
    }

    private void LateUpdate()
    {
        float distToCamera = Vector3.Distance(Camera.main.transform.position, transform.position);
        float cofResult = distToCamera * coficienResizeDistance; myRectTransform.localScale = origScale * cofResult;
        transform.forward = new Vector3(Camera.main.transform.forward.x, transform.forward.y, Camera.main.transform.forward.z);
    }
}
