using UnityEngine;

[CreateAssetMenu(fileName = "HandCoachConfig", menuName = "HandCoach/Config")]
public class HandCoachConfig : ScriptableObject
{
    public enum AnimationType
    {
      Tap,
      Swipe,
      Drag,
      Circle
    }
    
    [Header("PrefKeys")]
    public string screenKey;
    
    [Header("Animation Types")]
    public AnimationType animationType = AnimationType.Tap;
    
    
    // Animation Settings
    
    [Header("Tap Animation Settings")]
    public float tapScale = 1.2f;
    public float tapDuration = 1f;
    public float tapSpeed = 1f;
    
    [Header("Swipe Animation Settings")]
    public Vector3 swipeDirection = Vector3.right;
    public float swipeDuration = 1f;
    public float swipeDistance = 150f;
    public float swipeSpeed = 1f;
    
    [Header("Drag Animation Settings")]
    public Vector3 dragStartPosition = Vector3.zero;
    public Vector3 dragEndPosition = new Vector3(0f, 200f, 0f);
    public float dragDuration = 1f;
    public float dragSpeed = 1f;
    
    [Header("Circle Animation Settings")]
    public Vector3 circleCenter = Vector3.zero;
    public float circleRadius = 100f;
    public bool clockwise = true;
    public float circleSpeed = 1f;
    
    




}
