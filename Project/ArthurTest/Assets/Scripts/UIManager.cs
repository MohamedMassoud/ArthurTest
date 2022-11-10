using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Includes")]
    [Space(1)]
    [SerializeField] private Toggle isObjectSpaceToggle;
    [SerializeField] private Toggle isRotationGizmoEnabledToggle;
    [SerializeField] private Image spaceTypeImage;
    [SerializeField] private Image disabledRotationGizmoImage;
    [SerializeField] private Sprite objectSpaceSprite;
    [SerializeField] private Sprite worldSpaceSprite;

    [HideInInspector] public Action<bool> OnToggleChangedAction;
    [HideInInspector] public Action<bool> OnRotationGizmoChangedAction;
    [HideInInspector] public Action OnResetClickedAction;

    private BoxController boxController;

    private void Start()
    {
        boxController = SingletonManager.Instance.boxController;
    }
    public void OnToggleChanged()
    {
        OnToggleChangedAction?.Invoke(isObjectSpaceToggle.isOn);
        spaceTypeImage.sprite = isObjectSpaceToggle.isOn ? objectSpaceSprite : worldSpaceSprite;
    }

    public void OnRotationGizmoChanged()
    {
        OnRotationGizmoChangedAction?.Invoke(isRotationGizmoEnabledToggle.isOn);
        disabledRotationGizmoImage.enabled = !isRotationGizmoEnabledToggle.isOn;
    }

    public void OnResetClicked()
    {
        OnResetClickedAction?.Invoke();
    }

    public void OnAxisPress(int axis)
    {

        boxController.scalingAxis = (BoxController.ScalingAxis) axis;

    }

    public void OnAxisPressObjectSpace(int axis)
    {
        boxController.spaceType = BoxController.SpaceType.Object;
        boxController.scalingAxis = (BoxController.ScalingAxis)axis;
    }

    public void OnAxisPressWorldSpace(int axis)
    {
        boxController.spaceType = BoxController.SpaceType.World;
        boxController.scalingAxis = (BoxController.ScalingAxis)axis;
    }

    public void OnAxisRelease()
    {
        boxController.scalingAxis = BoxController.ScalingAxis.None;
    }

    public void OnRotationAxisPressObjectSpace(int axis)
    {
        boxController.spaceType = BoxController.SpaceType.Object;
        boxController.rotatingAxis = (BoxController.RotatingAxis) axis;
    }

    public void OnRotationAxisRelease()
    {
        boxController.rotatingAxis = BoxController.RotatingAxis.None;
    }

}
