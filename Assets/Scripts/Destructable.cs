using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructable : MonoBehaviour
{
    public int health = 4;
    public float appearAgainAfter = 5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D c)
    {
        if (c.gameObject.tag.Equals("Projectile"))
        {
            health -= 1;
            if (health == 0)
            {
                Destructable Crate = gameObject.GetComponent<Destructable>();
                Crate.HidePowerup();
                health = 4;
            }
        }
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
