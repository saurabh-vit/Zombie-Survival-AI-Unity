using UnityEngine;
using UnityEngine.InputSystem;  // 🔥 ADD THIS

public class PlayerRunSound : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip runSound;

    public float stepInterval = 0.45f;
    private float stepTimer;

    public static Vector3 lastSoundPosition;
    public static bool soundMade = false;

    void Update()
    {
        // ✅ NEW INPUT SYSTEM
        bool isRunning =
            Keyboard.current.leftShiftKey.isPressed &&
            Keyboard.current.wKey.isPressed;

        if (isRunning)
        {
            stepTimer -= Time.deltaTime;

            if (stepTimer <= 0f)
            {
                audioSource.PlayOneShot(runSound);

                lastSoundPosition = transform.position;
                soundMade = true;

                stepTimer = stepInterval;
            }
        }
        else
        {
            // optional: reset timer when not running
            stepTimer = 0f;
        }
    }
}