using UnityEngine;
using Pong;

/**
 * Class      : PaddleMovement
 * Description: This class will have the  paddle movement programed in order to make both paddles moving on the screen.
 * Author     : Rodrigo Januario da Silva
 * Version    : 1.0
 */
public class Paddle : MonoBehaviour {
    // Attributes declararion.
    private Rigidbody2D paddleRigidBody;
    private Renderer paddleRenderer;

    /**
     * Public variables in order to be set on the inspector
     */
    public float speed = 0.0f;
    public Boundary boundary;
    public string inputAxisName;

    // Use this for initialization
    void Start()
    {
        paddleRigidBody = GetComponent<Rigidbody2D>();
        paddleRenderer = GetComponent<Renderer>();
    }

    // FixedUpdate is called once per frame and is to be used with physics.
    void FixedUpdate()
    {
        float verticalMovement = Input.GetAxis(inputAxisName);
        Vector2 movement = new Vector2(0, verticalMovement);
        paddleRigidBody.velocity = movement * speed;
        paddleRigidBody.position = new Vector2(paddleRigidBody.position.x, Mathf.Clamp(paddleRigidBody.position.y, boundary.yMin, boundary.yMax));
    }

    /**
     * Method     : GetPaddleLeftBoundary
     * Return     : The calculated paddle's left boundary X position.
     * Description: This method will calculate and return the paddle's left boundary X position.
     */
    public float GetPaddleLeftBoundary()
    {
        return paddleRigidBody.position.x - paddleRenderer.bounds.size.x / 2;
    }

    /**
     * Method     : GetPaddleRightBoundary
     * Return     : The calculated paddle's right boundary X position.
     * Description: This method will calculate and return the paddle's right boundary X position.
     */
    public float GetPaddleRightBoundary()
    {
        return paddleRigidBody.position.x + paddleRenderer.bounds.size.x / 2;
    }
}
