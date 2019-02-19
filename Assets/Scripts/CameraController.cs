using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraTypes
{
    Follow,
    FreeLook
}

public class CameraController : MonoBehaviour {
    public Camera FollowCamera;
    public Camera FreeLookCamera;
    public GameObject target;
    public CameraTypes currentCameraType;
    public float objectHeight;
    private Vector3 offset;
    
    private float oldPosX;
    private float mousePosX;
    public float rotateSpeed;
    public float maxRotateSpeed;


    void Start () {
        currentCameraType = CameraTypes.FreeLook;
        //offset = target.transform.position - FollowCamera.transform.position;
        SwitchCamera();
    }
	
	void LateUpdate () {
        if (currentCameraType == CameraTypes.Follow)
        {
            //float desiredAngle = target.transform.eulerAngles.y;
            //Quaternion rotation = Quaternion.Euler(0, desiredAngle, 0);
            FollowCamera.transform.position = target.transform.position - 5* target.transform.forward + new Vector3(0f, 2f, 0f);//(rotation * offset);
            FollowCamera.transform.LookAt(target.transform);
        }
        else
        {
            FreeLookCamera.gameObject.transform.position = target.transform.position + new Vector3(0f, objectHeight, 0f);
        }
    }

    void Update()
    {
        if (currentCameraType == CameraTypes.FreeLook)
            UpdateRotation();
    }

    void UpdateRotation()
    {
        #if UNITY_EDITOR
        if (!Input.GetMouseButton(0))
        {
            oldPosX = 0;
            return;
        }
        #else
            if (Input.touches.Length != 1) {
                oldPosX = 0;
                return;
            }
        #endif

        mousePosX = Input.mousePosition.x / Screen.width; // 0-1

        if (oldPosX != 0)
        {
            CameraTypes type = currentCameraType;
            float tempFloat = (mousePosX - oldPosX) * rotateSpeed * Time.deltaTime;
            tempFloat = Mathf.Clamp(tempFloat, -maxRotateSpeed * Time.deltaTime, maxRotateSpeed * Time.deltaTime);
            FreeLookCamera.transform.Rotate(Vector3.up, tempFloat);
        }
        oldPosX = mousePosX;
    }

    public void SwitchCamera()
    {
        if (currentCameraType == CameraTypes.Follow)
        {
            FollowCamera.gameObject.SetActive(false);
            FreeLookCamera.gameObject.SetActive(true);
            currentCameraType = CameraTypes.FreeLook;
        }
        else
        {
            FollowCamera.gameObject.SetActive(true);
            FreeLookCamera.gameObject.SetActive(false); ;
            currentCameraType = CameraTypes.Follow;
        }
    }
}
