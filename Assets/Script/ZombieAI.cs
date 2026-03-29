using UnityEngine;
using UnityEngine.AI;

public class ZombieAI : MonoBehaviour
{
    public float attackCooldown = 2f;
    public AudioSource zombieRunAudio;

    private NavMeshAgent agent;
    private Animator animator;

    private float lastAttackTime;

    private Vector3 soundTarget;
    private bool investigatingSound = false;
    private bool reachedSoundPoint = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        DetectObjectSound();

        // Move to sound
        if (investigatingSound)
        {
            agent.isStopped = false;
            agent.SetDestination(soundTarget);

            float dist = Vector3.Distance(transform.position, soundTarget);

            if (dist < 1.5f && !reachedSoundPoint)
            {
                reachedSoundPoint = true;
                AttackAtSound();
            }
        }

        HandleAnimation();
        HandleZombieSound();
    }

    void DetectObjectSound()
    {
        if (ObjectImpactSound.soundMade)
        {
            soundTarget = ObjectImpactSound.soundPosition;
            investigatingSound = true;
            reachedSoundPoint = false;

            ObjectImpactSound.soundMade = false;
        }
    }

    void AttackAtSound()
    {
        agent.isStopped = true;

        if (Time.time - lastAttackTime > attackCooldown)
        {
            animator.SetTrigger("Attack");
            lastAttackTime = Time.time;
        }
    }

    void HandleAnimation()
    {
        if (agent.velocity.magnitude > 0.1f)
            animator.SetBool("isWalking", true);
        else
            animator.SetBool("isWalking", false);
    }

    void HandleZombieSound()
    {
        if (agent.velocity.magnitude > 0.1f)
        {
            if (!zombieRunAudio.isPlaying)
                zombieRunAudio.Play();
        }
        else
        {
            if (zombieRunAudio.isPlaying)
                zombieRunAudio.Stop();
        }
    }
}