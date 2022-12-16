using UnityEngine;

public class Movement : MonoBehaviour
{
    public float radius = 0.5f;
    private float latestDirectionChangeTime;
    private float directionChangeTime = 3f;
    public float characterVelocity = 1f;
    private Vector2 startPos;
    private Vector2 moveTo;


    void Start()
    {
        startPos = transform.position;
        latestDirectionChangeTime = 0f;
        calcuateNewMovementVector();
    }

    void calcuateNewMovementVector()
    {
        moveTo = startPos + Random.insideUnitCircle * radius;
        directionChangeTime = Random.Range(2.0f, 4.0f);
    }

    void Update()
    {
        if (Time.time - latestDirectionChangeTime > directionChangeTime)
        {
            latestDirectionChangeTime = Time.time;
            calcuateNewMovementVector();
        }

        transform.position = Vector2.MoveTowards(transform.position, moveTo, characterVelocity * Time.deltaTime);
    }
}
