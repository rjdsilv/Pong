using UnityEngine;

/**
 * Class      : BallController
 * Description: This class will have the ball control programed in order to make fly arround the screen with
 *              properly calculated velocity.
 * Author     : Rodrigo Januario da Silva
 * Version    : 1.0
 */
public class BallController : MonoBehaviour {
    public const string BALL_NAME = "Ball";

    // Attributes declararion.
    private Vector2 direction;
    private Rigidbody2D ballRigidBody;

    // Public variables in order to be set on the inspector
    public float speed = 0.0f;

    // Use this for initialization
    void Start ()
    {
        direction = Vector2.right;
        ballRigidBody = GetComponent<Rigidbody2D>();
        ballRigidBody.velocity = direction * speed;
    }

    // Used to get information after a collision happens.
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (CollidedWithPaddle(collision.gameObject.name))
        {
            ballRigidBody.velocity = CalculateVelocity(collision) + collision.rigidbody.velocity;
        }
    }

    /**
     * Method     : CalculateReflectionAngle
     * Param      : <b>ballPosition</b>  : The ball position on the screen.
     * Param      : <b>paddlePosition</b>: The paddle position on the screen.
     * Param      : <b>paddleHeight</b>  : The height of the paddle.
     * Return     : The ball's reflection angle.
     * Description: Calculates the angle the ball should reflect after colliding with a racket. More on the edges the ball hits,
     *              greater the reflection angle will be. We subtract the ball position from the paddle position in order to have
     *              a relative position beteween the ball and the paddle
     * 
     * Thanks to www.nobututs.com
     */
    float CalculateReflectionAngle(Vector2 ballPosition, Vector2 paddlePosition, float paddleHeight)
    {
        return (ballPosition.y - paddlePosition.y) / paddleHeight;
    }

    /**
     * Method     : IsLeftPaddle
     * Param      : <b>gameObjectName</b>: The name of the game object to be checked.
     * Return     : True if it's the left paddle. False otherwise.
     * Description: This method will check the game object name in order to know whether it's the left paddle or not.
     */
    bool IsLeftPaddle(string gameObjectName)
    {
        return PaddleController.LEFT_PADDLE_NAME == gameObjectName;
    }

    /**
     * Method     : IsRightPaddle
     * Param      : <b>gameObjectName</b>: The name of the game object to be checked.
     * Return     : True if it's the right paddle. False otherwise.
     * Description: This method will check the game object name in order to know whether it's the right paddle or not.
     */
    bool IsRightPaddle(string gameObjectName)
    {
        return PaddleController.RIGHT_PADDLE_NAME == gameObjectName;
    }

    /**
     * Method     : CollidedWithPaddle
     * Param      : <b>gameObjectName</b>: The name of the game object to be checked.
     * Return     : True if it's the collision was with any of the paddles. False otherwise.
     * Description: This method will check the game object name in order to know whether it's any of the paddles or not.
     */
    bool CollidedWithPaddle(string gameObjectName)
    {
        return IsLeftPaddle(gameObjectName) || IsRightPaddle(gameObjectName);
    }

    /**
     * Method     : CalculateVelocity
     * Param      : <b>collision</b>   : The ball's collision information.
     * Return     : The ball's vectorial speed.
     * Description: This method will check what paddle was hit, the paddle's hit position and calculate the ball's vectorial velocity.
     */
    Vector2 CalculateVelocity(Collision2D collision)
    {
        // X axis direction always for left or right only.
        float xDirection = IsLeftPaddle(collision.gameObject.name) ? 1.0f : -1.0f;
        float reflctionAngle = CalculateReflectionAngle(transform.position, collision.transform.position, collision.collider.bounds.size.y);
        Vector2 direction = new Vector2(xDirection, reflctionAngle);

        return direction * speed;
    }

    /**
     * Method     : ResetBall
     * Param      : <b>ballDirection</b> : The direction (left or right) in which the ball will be thrown.
     * Return     : The direction the ball will start.
     * Description: This method will reset the ball to (0, 0) and throw it in the correct direction after a score.
     */
    public void ResetBall()
    {
        ballRigidBody.position = new Vector2(0, 0);
        ballRigidBody.velocity = direction * speed;
    }

    /**
     * Method     : SetBallStartDirection
     * Param      : <b>ballDirection</b>: The direction the ball should start
     * Description: This method will set in which direction the ball should start its movement.
     */
    public void SetBallStartDirection(Vector2 ballDirection)
    {
        direction = ballDirection;
    }

    /**
     * Method     : GetBallPositionX
     * Return     : The ball X position on the screen.
     * Description: This method will check for the current ball X position on screen and return it.
     */
    public float GetBallPositionX()
    {
        return ballRigidBody.position.x;
    }
}
