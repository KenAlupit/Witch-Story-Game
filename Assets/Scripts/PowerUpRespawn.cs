using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpRespawn : MonoBehaviour
{
    public float appearAgainAfter = 5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HidePowerup()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
        StartCoroutine(DelayRespawn());
    }

    IEnumerator DelayRespawn()
    {
        yield return new WaitForSeconds(appearAgainAfter);
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<BoxCollider2D>().enabled = true;
    }
}
