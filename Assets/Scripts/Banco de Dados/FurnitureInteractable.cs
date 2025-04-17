using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Models;

public class FurnitureInteractable : MonoBehaviour
{
    [Header("Contexto SQL")]
    public string[] allowedTables;
    public Delegate validator;
    public SQLConsoleUI sqlUI;

    [Header("Proximidade")]
    public float interactRadius = 2f;
    public KeyCode interactKey = KeyCode.LeftControl;
    public Transform player;
    public GameObject prompt;

    void Start()
    {
        prompt.SetActive(false);
        if (player == null)
        {
            var playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;
        }
    }

    void Update()
    {
        if (player == null)
            return;

        
        Vector2 objectPos = new Vector2(transform.position.x, transform.position.y);
        Vector2 playerPos = new Vector2(player.position.x, player.position.y);
        float d = Vector2.Distance(objectPos, playerPos);

        
        prompt.SetActive(d <= interactRadius);

       
        if (d <= interactRadius && Input.GetKeyDown(interactKey))
        {
            Debug.Log("Allowed tables: " + string.Join(", ", allowedTables));
            sqlUI.Open(allowedTables, validator);
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            sqlUI.Close();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, interactRadius);
    }
}
