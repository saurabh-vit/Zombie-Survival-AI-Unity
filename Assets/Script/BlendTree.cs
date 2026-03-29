using UnityEngine;

public class BlendController : MonoBehaviour
{
    public float walkspeed;
    public float runspeed;

    public float acceleration;
    public float deceleration;

    private float playerCurrentSpeed;

    public Animator animator;

    void Start()
    {
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            Acceleration();
        }
        else
        {
            Deceleration();
        }

        animator.SetFloat("PlayerSpeed", playerCurrentSpeed);

        transform.Translate(Vector3.forward * playerCurrentSpeed * Time.deltaTime);
    }

    void Acceleration()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (playerCurrentSpeed < runspeed)
            {
                playerCurrentSpeed += acceleration * Time.deltaTime;
            }
        }
        else
        {
            if (playerCurrentSpeed < walkspeed)
            {
                playerCurrentSpeed += acceleration * Time.deltaTime;
            }
        }
    }

    void Deceleration()
    {
        if (playerCurrentSpeed > 0.0f)
        {
            playerCurrentSpeed -= deceleration * Time.deltaTime;
        }
        else
        {
            playerCurrentSpeed = 0.0f;
        }
    }
}