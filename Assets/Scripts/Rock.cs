using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    [SerializeField]
    private int hp;

    [SerializeField]
    private float destroyDelay;

    [SerializeField]
    private SphereCollider sphereCollider;

    [SerializeField]
    private GameObject rockGeometry;
    [SerializeField]
    private GameObject debrisRockGeometry;

    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip hitSoundEffect;
    [SerializeField]
    private AudioClip destroySoundEffect;

    private void Awake()
    {
        sphereCollider= GetComponent<SphereCollider>();
        audioSource = GetComponent<AudioSource>();
    }
    public void Minning()
    {
        audioSource.clip = hitSoundEffect;
        audioSource.Play();
        hp--;
        if(hp <= 0)
        {
            Crush();
        }
    }

    private void Crush()
    {
        audioSource.clip = destroySoundEffect;
        audioSource.Play();
        sphereCollider.enabled= false;
        Destroy(rockGeometry);
        debrisRockGeometry.SetActive(true);

        Destroy(debrisRockGeometry, destroyDelay);
    }
}
