using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private GameObject shootActionCameraGameObject;
    [SerializeField] private GameObject otherActionCameraGameObject;

    private void Start()
    {
        BaseAction.OnAnyActionStarted += BaseAction_OnAnyActionStarted;
        BaseAction.OnAnyActionEnded += BaseAction_OnAnyActionEnded;
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;

        HideAllActionCamera();
    }

    private void BaseAction_OnAnyActionStarted(object sender, EventArgs e)
    {
        switch (sender)
        {
            case ShootAction shootAction:
                Unit shooterUnit = shootAction.GetUnit();
                Unit targetUnit = shootAction.GetTargetUnit();

                Vector3 cameraCharacterHeight = Vector3.up * 1.7f;
                Vector3 shootDir = (targetUnit.GetWorldPosition() - shooterUnit.GetWorldPosition()).normalized;

                float shoulderOffsetAmount = 0.5f;
                Vector3 shoulderOffset = Quaternion.Euler(0, 90, 0) * shootDir * shoulderOffsetAmount;

                Vector3 actionCameraPosition = shooterUnit.GetWorldPosition() + cameraCharacterHeight + shoulderOffset + (shootDir * -1);
                shootActionCameraGameObject.transform.position = actionCameraPosition;

                shootActionCameraGameObject.transform.LookAt(targetUnit.GetWorldPosition() + cameraCharacterHeight);

                ShowActionCamera(shootActionCameraGameObject);
                break;

            case MoveAction moveAction:
                if (moveAction.GetUnit().IsEnemy())
                {
                    otherActionCameraGameObject.GetComponent<CinemachineVirtualCamera>().m_Follow = moveAction.GetUnit().transform;
                    otherActionCameraGameObject.GetComponent<CinemachineVirtualCamera>().m_LookAt = moveAction.GetUnit().transform;

                    ShowActionCamera(otherActionCameraGameObject);
                    HideActionCamera(shootActionCameraGameObject);
                }
                break;
        }
    }

    private void BaseAction_OnAnyActionEnded(object sender, EventArgs e)
    {
        switch (sender)
        {
            case ShootAction shootAction:
                if (!shootAction.GetUnit().IsEnemy())
                {
                    HideActionCamera(shootActionCameraGameObject);
                }
                else
                {
                    HideActionCamera(shootActionCameraGameObject);
                    ShowActionCamera(otherActionCameraGameObject);
                }
                break;
        }
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        if (TurnSystem.Instance.IsPlayerTurn())
        {
            HideAllActionCamera();
        }
    }

    private void ShowActionCamera(GameObject actionCamera)
    {
        actionCamera.SetActive(true);
    }

    private void HideActionCamera(GameObject actionCamera)
    {
        actionCamera.SetActive(false);
        // ResetActionCamera();
    }

    private void HideAllActionCamera()
    {
        shootActionCameraGameObject.SetActive(false);
        otherActionCameraGameObject.SetActive(false);
    }

    private void ResetActionCamera()
    {
        shootActionCameraGameObject.GetComponent<CinemachineVirtualCamera>().m_Follow = null;
        shootActionCameraGameObject.GetComponent<CinemachineVirtualCamera>().m_LookAt = null;
    }
}
