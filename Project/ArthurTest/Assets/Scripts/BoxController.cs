using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BoxController : MonoBehaviour
{
    
    [Header("Includes")]
    [Space(1)]
    [SerializeField] private LayerMask boxLayer;
    [SerializeField] private GameObject rotationGizmo;
    [Space(5)]

    [Header("Config")]
    [Space(1)]
    [SerializeField] private float scalingSpeed = 1.001f;
    [SerializeField] private float rotationSpeed = 500f;
    [Space(5)]

    [Header("States")]
    [Space(1)]
    public ScalingAxis scalingAxis = ScalingAxis.None;
    public RotatingAxis rotatingAxis = RotatingAxis.None;
    public SpaceType spaceType = SpaceType.Object;
    public RotationMode rotationMode = RotationMode.Normal;


    private UIManager uiManager;
    private int scalingDir = 1;
    private bool boxClicked = false;
    private Quaternion initialRotation;
    public enum RotationMode
    {
        Normal,
        Gizmo, 
        None
    }

    public enum ScalingAxis
    {
        X,
        Y,
        Z,
        None
    }

    public enum RotatingAxis
    {
        X,
        Y,
        Z,
        None
    }

    public enum SpaceType
    {
        Object,
        World
    }


    private void Update()
    {
        CheckInput();
        Scale();
        RotateGizmo();
        RotateNormal();
    }

    private void RotateGizmo()
    {
        if (rotationMode != RotationMode.Gizmo) return;
        float directedRotationSpeed = (Input.GetAxis("Mouse X") + Input.GetAxis("Mouse Y")) / 2 * Time.deltaTime * rotationSpeed;
        switch (spaceType)
        {

            case SpaceType.Object:
                switch (rotatingAxis)
                {
                    case RotatingAxis.X:
                        transform.Rotate(-transform.forward, directedRotationSpeed, Space.World);
                        break;

                    case RotatingAxis.Y:
                        transform.Rotate(-transform.right, directedRotationSpeed, Space.World);
                        break;

                    case RotatingAxis.Z:
                        transform.Rotate(-transform.up, directedRotationSpeed, Space.World);
                        break;
                }
                break;


            case SpaceType.World:                   //Not yet implemented. Out of the task's scope. Just got overhyped to expect it's implementation.
                switch (rotatingAxis)
                {
                    case RotatingAxis.X:
                        break;

                    case RotatingAxis.Y:
                        break;

                    case RotatingAxis.Z:
                        break;
                }
                break;

        }
    }
    private void RotateNormal()
    {
        if (rotationMode != RotationMode.Normal || !boxClicked) return;

        switch (spaceType)
        {
            case SpaceType.Object:
                transform.Rotate(-Input.GetAxis("Mouse X") * Time.deltaTime * rotationSpeed, 0f, 0f, Space.Self);
                transform.Rotate(0f, 0f, -Input.GetAxis("Mouse Y") * Time.deltaTime * 500f, Space.Self);
                break;

            case SpaceType.World:
                transform.Rotate(0f, -Input.GetAxis("Mouse X") * Time.deltaTime * rotationSpeed, 0f, Space.World);
                transform.Rotate(Input.GetAxis("Mouse Y") * Time.deltaTime * rotationSpeed, 0f, 0f, Space.World);
                break;
        }
        

    }
    private void Scale()
    {
        switch (spaceType)
        {
            case SpaceType.Object:
                switch (scalingAxis)
                {
                    case ScalingAxis.X:
                        transform.localScale += new Vector3(0, 0f, scalingSpeed * Time.deltaTime * scalingDir);
                        break;

                    case ScalingAxis.Y:
                        transform.localScale += new Vector3(scalingSpeed * Time.deltaTime * scalingDir, 0f, 0f);
                        break;

                    case ScalingAxis.Z:
                        transform.localScale += new Vector3(0f, scalingSpeed * Time.deltaTime * scalingDir, 0f);
                        break;
                }
                break;

            case SpaceType.World:
                switch (scalingAxis)
                {
                    case ScalingAxis.X:
                        transform.parent.localScale += new Vector3(scalingSpeed * Time.deltaTime * scalingDir, 0f, 0f);
                        break;

                    case ScalingAxis.Y:
                        transform.parent.localScale += new Vector3(0f, transform.localScale.y * scalingSpeed * Time.deltaTime * scalingDir, 0f);
                        break;

                    case ScalingAxis.Z:
                        transform.parent.localScale += new Vector3(0f, 0f, scalingSpeed * Time.deltaTime * scalingDir);
                        break;
                }
                break;
        }

    }

    private void Start()
    {
        initialRotation = transform.transform.rotation;
        uiManager = SingletonManager.Instance.uiManager;
        uiManager.OnToggleChangedAction += ChangeWorldSpace;
        uiManager.OnResetClickedAction += Reset;
        uiManager.OnRotationGizmoChangedAction += ChangeRotationMode;
    }

    private void ChangeRotationMode(bool isGizmo)
    {
        rotationMode = isGizmo ? RotationMode.Gizmo : RotationMode.Normal;
        rotationGizmo.SetActive(isGizmo);
    }

    private void ChangeWorldSpace(bool spaceType)
    {
        this.spaceType = spaceType ? SpaceType.Object : SpaceType.World;
    }


    private void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            scalingDir = -1;
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            scalingDir = 1;
        }

        CheckBoxClicked();
    }

    private void CheckBoxClicked()
    {
        if (rotationMode != RotationMode.Normal) return;

        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), 100, boxLayer) && !EventSystem.current.IsPointerOverGameObject())
            {
                boxClicked = true;
            }
            else
            {
                boxClicked = false;
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            boxClicked = false;
        }

    }

    private void Reset()
    {
        transform.parent.localScale = Vector3.one;
        transform.localScale = Vector3.one;
        transform.rotation = initialRotation;
    }


}
