using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Crate : MonoBehaviour
{
    Vector3 rotation;

    [SerializeField]
    private ParticleSystem breakingEffect;

    private CinemachineImpulseSource cinemachineImpulseSource;
    private Player player;

    private void Start()
    {
        cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();
        player = FindObjectOfType<Player>();

        var xRotation = Random.Range(90f, 180f);
        rotation = new Vector3(-xRotation, 0);
    }

    private void Update()
    {
        transform.Rotate(rotation * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Crate"))
        {
            Destroy(gameObject);
            Instantiate(breakingEffect, transform.position, Quaternion.identity);

            if (player != null)
            {
                // Check how distant a crate is from the player and measure the force to use on the ShakeCamera
                var distance = Vector3.Distance(transform.position, player.transform.position);
                var force = 1f / distance; // Distance and force are inversely proportional

                cinemachineImpulseSource.GenerateImpulse(force);
            }
          
        }
    }

}
