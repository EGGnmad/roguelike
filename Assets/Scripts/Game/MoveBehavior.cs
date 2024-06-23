using UnityEngine;

public class MoveBehavior : MonoBehaviour
{
    public Vector2 velocity;
    public float duration;

    private float _startTime;
    
    private void Start()
    {
        _startTime = Time.time;
    }

    private void Update()
    {
        transform.Translate(velocity*Time.deltaTime);
        if (Time.time - _startTime >= duration)
        {
            Destroy(gameObject);
        }
    }
}