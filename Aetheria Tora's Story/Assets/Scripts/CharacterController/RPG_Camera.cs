using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPG_Camera : MonoBehaviour
{
    #region Enums
    public enum CameraUsage
    {
        MainCamera,
        SpawnOwnCamera,
        AssignedCamera
    }
    public enum CursorHiding
    {
        Never,
        WhenOrbiting,
        Always
    }

    public enum CursorBehavior
    {
        Move,
        MoveConfined,
        Stay,
        LockInCenter
    }
    #endregion

    [Header("References")]
    private Camera CameraToUse;
    private InputManager inputManager;

    [Header("Booleans")]
    public bool AlwaysRotateCamera = false;
    public bool LockRotationX = false;
    public bool LockRotationY = false;
    public bool InvertRotationX = true;
    public bool InvertRotationY = true;
    /// Constrain the camera's orbit horizontal axis from "RotationXMin" degrees to "RotationXMax" degrees
    public bool ConstrainRotationX = false;

    [Header("Enums")]
    public CursorHiding HideCursor = CursorHiding.WhenOrbiting;

    [Header("Floats")]
    protected float _inputZoomAmount = 0;
   
    [Header("Vectors")]
    protected Vector2 _inputRotationAmount;
        


}
