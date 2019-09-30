using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Vector2 LimitY;
    private float Speed = 60f;
    float RotationY, RotationX;
    private int _zoomState;
    public int ZoomUnits;
    private Vector3 _forwardPosition;
    private bool _followToad;
    public Transform Toad;
    public Vector3 CenterOfLevel;
    public float CameraSpeed;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        Rotate();
        Zoom();
        transform.position = Vector3.Lerp(transform.position, _forwardPosition, Time.deltaTime*10);
        if (_followToad)
        {
            transform.position = Toad.position+_forwardPosition;
            CenterOfLevel = Toad.position;

        }
        else
        {
            CenterOfLevel = Vector3.zero;
            transform.position = Vector3.zero;
            //camera.main.transform.position.Lookat(CenterofLevel)
        }

        Debug.Log(Vector3.Normalize(Camera.main.transform.position - Toad.position));
    }

    private void Zoom()
    {
        if (Input.GetButtonDown("Zoom"))
        {

            if (_zoomState < 1)
            {
                _zoomState++;
            }
            else _zoomState = 0;
            if (_zoomState == 1)
            {
                _followToad = true;
                _forwardPosition =   (Toad.position-Camera.main.transform.position)-Vector3.Normalize(Toad.position - Camera.main.transform.position)*3;

            }
            else
            {
                _followToad = false;
                _forwardPosition = Vector3.zero;

            }


        }

    }

    private void Rotate()
    {
        //RotationX += Input.GetAxis("VerticalRight") * Speed * Time.deltaTime;
        // RotationY += Input.GetAxis("HorizontalRight") * Speed * Time.deltaTime;
        Camera.main.transform.RotateAround(CenterOfLevel, Vector3.up, Input.GetAxis("VerticalRight") * -1 * Time.deltaTime * CameraSpeed);
        Camera.main.transform.RotateAround(CenterOfLevel, Camera.main.transform.right, Input.GetAxis("HorizontalRight") * -1 * Time.deltaTime * CameraSpeed);

        RotationY = Mathf.Clamp(RotationY, LimitY.x, LimitY.y);
        GetComponentInChildren<Transform>().eulerAngles = new Vector3(RotationY, RotationX, 0);
    }
}
