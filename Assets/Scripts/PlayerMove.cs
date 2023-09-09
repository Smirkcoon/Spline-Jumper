using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class PlayerMove : MonoBehaviour
{
    
    [SerializeField] private float xMaxOffsetFromPath;
    [SerializeField] private float speed = 0.1f;
    [SerializeField] private float fixInputSpeedSide = 0.01f;

    private float offsetFromPath;
    private SplinePath path;
    private float t = 0f;
    private bool inGame;
    private void Start()
    {
        Observer.EndGame += (x) => inGame = false;
        Observer.ResetPositionAllYZ += (y,z) => transform.position -= new Vector3(0, y, z);
        Observer.SetNewPath += SetPath;
    }
    public void SetPath(SplinePath _path)
    {
        t = 0;
        path = _path;
        inGame = true;
    }
    private void FixedUpdate()
    {
        if (!inGame)
            return;
        Vector3 pos = path.EvaluatePosition(t);
        transform.position = pos + new Vector3(offsetFromPath, 0, 0);
        t += speed * Time.fixedDeltaTime;
        if (t > 1f) t = 0f;
    }
    private void Update()
    {
        if (!inGame)
            return;
#if UNITY_EDITOR
        float horizontalInput = Input.GetAxis("Horizontal");

        if (horizontalInput != 0)
        {
            // ���� �������� ����� � offsetFromPath ������ xMaxOffsetFromPath, ���������� ��� ������ xMaxOffsetFromPath.
            if (horizontalInput < 0 && offsetFromPath > -xMaxOffsetFromPath)
            {
                offsetFromPath = Mathf.Max(offsetFromPath + horizontalInput, -xMaxOffsetFromPath);
            }
            // ���� �������� ������ � offsetFromPath ������ xMaxOffsetFromPath, ���������� ��� ������ xMaxOffsetFromPath.
            else if (horizontalInput > 0 && offsetFromPath < xMaxOffsetFromPath)
            {
                offsetFromPath = Mathf.Min(offsetFromPath + horizontalInput, xMaxOffsetFromPath);
            }
        }
#endif
#if UNITY_ANDROID || UNITY_IOS
        if (Input.touchCount > 0)
        {
            // �������� ������ �������
            Touch touch = Input.GetTouch(0);

            // ���������, ���� ��������� ������� �� ������
            if (touch.phase == TouchPhase.Moved)
            {
                float horizontalInputMobile = touch.deltaPosition.x * fixInputSpeedSide;

                // ���� �������� ����� � offsetFromPath ������ xMaxOffsetFromPath, ���������� ��� ������ xMaxOffsetFromPath.
                if (horizontalInputMobile < 0 && offsetFromPath > -xMaxOffsetFromPath)
                {
                    offsetFromPath = Mathf.Max(offsetFromPath + horizontalInputMobile, -xMaxOffsetFromPath);
                }
                // ���� �������� ������ � offsetFromPath ������ xMaxOffsetFromPath, ���������� ��� ������ xMaxOffsetFromPath.
                else if (horizontalInputMobile > 0 && offsetFromPath < xMaxOffsetFromPath)
                {
                    offsetFromPath = Mathf.Min(offsetFromPath + horizontalInputMobile, xMaxOffsetFromPath);
                }
            }
        }
#endif
    }
    private void OnTriggerEnter(Collider col)
    {
        if (!inGame)
            return;

        if (col.gameObject.CompareTag("Obstacle"))
        {
            Observer.EndGame(false);
        }
        if (col.gameObject.CompareTag("Jump"))
        {
            Observer.PlayerAnimJump();
        }
        if (col.gameObject.CompareTag("Down"))
        {
            Observer.PlayerAnimDown();
        }
    }
    private void OnDestroy()
    {
        Observer.EndGame -= (x) => inGame = false;
        Observer.ResetPositionAllYZ -= (y, z) => transform.position -= new Vector3(0, y, z);
    }
}
