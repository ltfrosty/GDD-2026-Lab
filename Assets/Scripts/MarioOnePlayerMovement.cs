using UnityEngine;

public class MarioOnePlayerMovement : MonoBehaviour
{

    //rigidbody2D variables
    public float speed = 10;
    public float maxSpeed = 20;
    private Rigidbody2D marioBody;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Set to be 30 FPS
        Application.targetFrameRate = 30;
        marioBody = GetComponent<Rigidbody2D>();
        // Find the Rigidbody2D attached to the same Mario GameObject as this script 
        // and store it in marioBody.
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");

        if (Mathf.Abs(moveHorizontal) > 0)
        {
            Vector2 movement = new Vector2(moveHorizontal, 0);

            // Check if it doesn't go beyond maxSpeed
            if (marioBody.linearVelocity.magnitude < maxSpeed)
            {
                marioBody.AddForce(movement * speed);
            }
        }

        // Stop
        if (Input.GetKeyUp("a") || Input.GetKeyUp("d"))
        {
            marioBody.linearVelocity = Vector2.zero;
        }
    }
}
