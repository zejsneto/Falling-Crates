using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float forceMultiplier = 3f;
    [SerializeField]
    private float maximumVelocity = 3f;
    [SerializeField]
    private ParticleSystem deathParticles;

    private Rigidbody rb;
    private CinemachineImpulseSource cinemachineImpulseSource;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();
    }

    void Update()
    {
        // Avoid player to move while GameManager is disabled (Main Menu Screen)
        if (GameManager.Instance == null)
        {
            return;
        }

        var horizontalInput = Input.GetAxis("Horizontal");

        if (rb.velocity.magnitude <= maximumVelocity)
        {
            rb.AddForce(new Vector3(horizontalInput  * forceMultiplier * Time.deltaTime, 0, 0));
        }
    }

    private void OnEnable()
    {
        transform.position = new Vector3(0, 0.75f, 0);
        transform.rotation = Quaternion.identity;
        rb.velocity = Vector3.zero;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Crate"))
        {
            GameOver();

            Instantiate(deathParticles, transform.position, Quaternion.identity);
            cinemachineImpulseSource.GenerateImpulse();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("FallDown"))
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        GameManager.Instance.GameOver();

        gameObject.SetActive(false);
    }
}
