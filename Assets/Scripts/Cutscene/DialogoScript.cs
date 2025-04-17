using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogoScript : MonoBehaviour
{
    [Header("Dialogo")]
    [SerializeField]
    private string[] dialogoNpc;
    [SerializeField]
    private int dialogoIndex;
    [SerializeField]
    private GameObject dialogoPanel;
    [SerializeField]
    private TMP_Text dialogoTexto;
    [SerializeField]
    private TMP_Text nomeDialogoTexto;
    [SerializeField]
    private Image imageNpc;
    [SerializeField]
    private Sprite spriteNpc;
    [SerializeField]
    private bool readyToSpeak;
    [SerializeField]
    private bool startDialogo;

    private GameObject playerObj;
    void Start()
    {
        dialogoPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire1") && readyToSpeak && !startDialogo)
        {
            startDialogo = true;
             playerObj = GameObject.FindGameObjectWithTag("Player");
            playerObj.GetComponent<Player>().GetComponent<Player>().enabled = false;
            StartDialogo();
        }
        else if(dialogoTexto.text == dialogoNpc[dialogoIndex])
        {
            
                dialogoIndex++;
                if (dialogoIndex < dialogoNpc.Length)
                {

                StartCoroutine(DelayAndShowNext());
            }
                else
                {
                    dialogoPanel.SetActive(false);
                    startDialogo = false;
                    dialogoIndex = 0;
                    playerObj.GetComponent<Player>().GetComponent<Player>().enabled = true;
                }
            
        }
    }

    private void StartDialogo()
    {
        dialogoPanel.SetActive(true);
        startDialogo = true;
        dialogoIndex = 0;
        nomeDialogoTexto.text = "Sogra";
        imageNpc.sprite = spriteNpc;
        StartCoroutine(ShowDialogo());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            readyToSpeak = true;
        }
        
    }

    IEnumerator ShowDialogo()
    {
        dialogoTexto.text = "";
        foreach(char letra in dialogoNpc[dialogoIndex])
        {
            dialogoTexto.text += letra;
            yield return new WaitForSeconds(0.1f);
        }
       
        
    }

    private IEnumerator DelayAndShowNext()
    {
        yield return new WaitForSeconds(1f);
        StartCoroutine(ShowDialogo());
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            readyToSpeak = false;
            
        }
    }
}
