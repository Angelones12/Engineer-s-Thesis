using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;

    [SerializeField] private CinemachineVirtualCamera[] _allVirtualCameras;

    [SerializeField] private float _fallAmount = 0.25f;
    [SerializeField] private float _fallYTime = 0.35f;
    public float _fallSpeddChangeThreshold = -15f;

    public bool IsLerpingDamping { get; private set; }
    public bool LerpedFromPlayerFalling { get; set; }

    private Coroutine _lerpCoroutine;

    private CinemachineVirtualCamera _currentCamera;
    private CinemachineFramingTransposer _framingTransposer;

    private float _normAmount;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        for (int i = 0; i < _allVirtualCameras.Length; i++)
        {
            if (_allVirtualCameras[i].enabled)
            {
                _currentCamera = _allVirtualCameras[i];
                _framingTransposer = _currentCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
            }
        }

        _normAmount = _framingTransposer.m_YDamping;
    }

    public void LerpYDamping(bool isPlayerFalling)
    {
        _lerpCoroutine = StartCoroutine(LerpAction(isPlayerFalling));
    }

    private IEnumerator LerpAction(bool isPlayerFalling)
    {
        IsLerpingDamping = true;

        float startDampAmount = _framingTransposer.m_YDamping;
        float endDampAmount = 0f;

        if(isPlayerFalling)
        {
            endDampAmount = _fallAmount;
            LerpedFromPlayerFalling = true;
        }
        else
        {
            endDampAmount = _normAmount;
        }

        float elapsedTime = 0f;
        while (elapsedTime < _fallYTime)
        {
            elapsedTime += Time.deltaTime;

            float lerpedAmount = Mathf.Lerp(startDampAmount, endDampAmount, (elapsedTime / _fallYTime));
            _framingTransposer.m_YDamping = lerpedAmount;

            yield return null;
        }

        IsLerpingDamping = false;
    }
}
