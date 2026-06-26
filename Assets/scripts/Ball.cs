using UnityEngine;
using System.Collections.Generic;


public class Ball : MonoBehaviour
{
    private List<Ball> touchingBalls = new List<Ball>();
    public int points;
    
    [System.Obsolete]


    private void Start()
    {
        
    }
    private void Update()
    {
       
    }

    [System.Obsolete]
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Stop the ball's movement
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.isKinematic = true;

        // Check if the colliding object is another ball
        Ball otherBall = collision.gameObject.GetComponent<Ball>();
        if (otherBall != null && !touchingBalls.Contains(otherBall))
        {
            touchingBalls.Add(otherBall);
            otherBall.AddTouchingBall(this);
            CheckForMatch();
        }
    }

    private void AddTouchingBall(Ball ball)
    {
        if (!touchingBalls.Contains(ball))
        {
            touchingBalls.Add(ball);
        }
    }

    private void CheckForMatch()
    {
        
        List<Ball> connectedSameColor = GetConnectedBallsOfSameColor();
        
       
        if (connectedSameColor.Count >= 3)
        {
            int matchPoints = connectedSameColor.Count * 10;
            if (shoot.Instance != null)
            {
                shoot.Instance.AddScore(matchPoints);
            }

            CameraShake.Shake();
            
            foreach (Ball ball in connectedSameColor)
            {
                Destroy(ball.gameObject);
            }
        }
    }

    private List<Ball> GetConnectedBallsOfSameColor()
    {
        List<Ball> result = new List<Ball> { this };
        Queue<Ball> toCheck = new Queue<Ball>();
        HashSet<Ball> visited = new HashSet<Ball> { this };

        // Start with touching balls
        foreach (Ball ball in touchingBalls)
        {
            if (ball != null && IsSameColor(ball))
            {
                toCheck.Enqueue(ball);
                visited.Add(ball);
            }
        }

        
        while (toCheck.Count > 0)
        {
            Ball current = toCheck.Dequeue();
            result.Add(current);

            foreach (Ball neighbor in current.touchingBalls)
            {
                if (neighbor != null && !visited.Contains(neighbor) && IsSameColor(neighbor))
                {
                    visited.Add(neighbor);
                    toCheck.Enqueue(neighbor);
                }
            }
        }

        return result;
    }

    private bool IsSameColor(Ball other)
    {
        return other.CompareTag(gameObject.tag);
    }

}
