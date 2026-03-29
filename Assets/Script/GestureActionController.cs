using UnityEngine;

public class GestureActionController : MonoBehaviour
{
    public Animator animator;

    public GestureReceiver gestureReceiver;   // 🔥 Assigned in Inspector
    public Transform handHoldPoint;
    public GameObject objectToThrow;

    public float pickupDistance = 1.5f;

    private Rigidbody rb;
    private bool isHolding = false;

    private string lastGesture = "";

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = objectToThrow.GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (gestureReceiver == null) return;

        string gesture = gestureReceiver.gesture;

        // 🔥 Prevent animation spam
        if (gesture != lastGesture)
        {
            if (gesture == "FIST")
            {
                TryPickup();
            }
            else if (gesture == "OPEN")
            {
                TryThrow();
            }

            lastGesture = gesture;
        }
    }

    // ---------------- PICKUP ----------------
    void TryPickup()
    {
        if (isHolding) return;

        float distance = Vector3.Distance(transform.position, objectToThrow.transform.position);

        if (distance <= pickupDistance)
        {
            animator.SetTrigger("PickUp");

            Invoke(nameof(AttachObject), 0.5f); // sync with animation
        }
    }

    void AttachObject()
    {
        rb.isKinematic = true;

        objectToThrow.transform.position = handHoldPoint.position;
        objectToThrow.transform.parent = handHoldPoint;

        isHolding = true;
    }

    // ---------------- THROW ----------------
    void TryThrow()
    {
        if (!isHolding) return;

        animator.SetTrigger("Throw");

        Invoke(nameof(ReleaseObject), 0.4f);
    }

    void ReleaseObject()
    {
        objectToThrow.transform.parent = null;

        rb.isKinematic = false;

        rb.AddForce(transform.forward * 600 + transform.up * 200);

        isHolding = false;
    }
}