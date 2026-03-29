using UnityEngine;

public class ObjectImpactSound : MonoBehaviour
{
    public static Vector3 soundPosition;
    public static bool soundMade = false;

    AudioSource audioSource;
    Rigidbody rb;

    public float impactThreshold = 2f;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (rb.linearVelocity.magnitude > impactThreshold)
        {
            audioSource.Play();

            soundPosition = transform.position;
            soundMade = true;
        }
    }
}