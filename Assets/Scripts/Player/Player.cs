using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    
    void Start()
    {

    }

    
    void Update()
    {
        movimentacaoPlayer();
    }

    void movimentacaoPlayer()
    {
        

        float speed_h = Input.GetAxis("Horizontal") * 1.4f;
        float speed_v = Input.GetAxis("Vertical")* 1.4f;   

        Vector2 movPlayer = new Vector2(speed_h, speed_v);
        this.gameObject.GetComponent<Rigidbody2D>().velocity = movPlayer;
    }
}
