using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleWallManager : MonoBehaviour
{
    [SerializeField]
    bool killSwitch = true;

    public PlayerBehaviour Player;
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
        if (killSwitch)
        {
            if (c.gameObject.tag.Equals("Player"))
            {
                Player.currentHealth = 0;
            }
        }
    }
}
