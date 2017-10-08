using UnityEngine;

/**
 * Class      : BallMovement
 * Description: This class will have the ball movement programed in order to make fly arround the screen.
 * Author     : Rodrigo Januario da Silva
 * Version    : 1.0
 */
public class Ball : MonoBehaviour {
    // The paddle as a rigid body.
    private Rigidbody2D ball;

    /**
     * Public variables in order to be set on the inspector
     */
    public float speed = 0.0f;

    // Use this for initialization
    void Start ()
    {
        ball = GetComponent<Rigidbody2D>();
        ball.velocity = Vector2.right * speed;
	}
}
