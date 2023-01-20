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
    private GameObject go_rock;
    [SerializeField]
    private GameObject go_debrisRock;

    [SerializeField]
    private string strikeSound;
    [SerializeField]
    private string destroySound;

    private void Awake()
    {
        sphereCollider= GetComponent<SphereCollider>();
    }
    public void Minning()
    {
        SoundManager.instance.PlaySE(strikeSound);
        hp--;
        if(hp <= 0)
        {
            Crush();
        }
    }

    private void Crush()
    {
        SoundManager.instance.PlaySE(destroySound);
        sphereCollider.enabled= false;
        Destroy(go_rock);
        go_debrisRock.SetActive(true);

        Destroy(go_debrisRock, destroyDelay);
    }
}
