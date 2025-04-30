using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogoScript : MonoBehaviour
{
    [Header("Diálogo")]
    [SerializeField] private string[] dialogoNpc;
    [SerializeField] private int dialogoIndex;
    [SerializeField] private GameObject dialogoPanel;
    [SerializeField] private TMP_Text dialogoTexto;
    [SerializeField] private TMP_Text nomeDialogoTexto;
    [SerializeField] private Image imageNpc;
    [SerializeField] private Sprite spriteNpc;
    [SerializeField] private bool readyToSpeak;
    [SerializeField] private bool startDialogo;

    private GameObject playerObj;

    void Start()
    {
        dialogoPanel.SetActive(false);

        // Desabilita o player e inicia o diálogo assim que a cena carrega
        playerObj = GameObject.FindGameObjectWithTag("Player");
        playerObj.GetComponent<Player>().enabled = false;
        playerObj.GetComponent<Animator>().enabled = false;


        StartDialogo();
    }

    void Update()
    {
        // Permite reativar o diálogo ao apertar Fire1 dentro do trigger
        if (Input.GetButtonDown("Fire1") && readyToSpeak && !startDialogo)
        {
            StartDialogo();
        }
        // Avança linhas automaticamente quando o texto terminar de ser exibido
        else if (startDialogo && dialogoTexto.text == dialogoNpc[dialogoIndex])
        {
            dialogoIndex++;
            if (dialogoIndex < dialogoNpc.Length)
            {
                StartCoroutine(DelayAndShowNext());
            }
            else
            {
                // Encerra o diálogo e devolve o controle ao player
                dialogoPanel.SetActive(false);
                startDialogo = false;
                dialogoIndex = 0;
                playerObj.GetComponent<Player>().enabled = true;
                playerObj.GetComponent<Animator>().enabled = true;
            }
        }
    }

    private void StartDialogo()
    {
        // Desabilita o controle do player
        if (playerObj == null)
            playerObj = GameObject.FindGameObjectWithTag("Player");
        playerObj.GetComponent<Player>().enabled = false;

        // Exibe o painel e configura o primeiro texto
        dialogoPanel.SetActive(true);
        startDialogo = true;
        dialogoIndex = 0;
        // nomeDialogoTexto.text = "Sogra"; // se quiser exibir o nome
        imageNpc.sprite = spriteNpc;
        StartCoroutine(ShowDialogo());
    }

    private IEnumerator DelayAndShowNext()
    {
        yield return new WaitForSeconds(1f);
        StartCoroutine(ShowDialogo());
    }

    private IEnumerator ShowDialogo()
    {
        dialogoTexto.text = "";
        foreach (char letra in dialogoNpc[dialogoIndex])
        {
            dialogoTexto.text += letra;
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            readyToSpeak = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            readyToSpeak = false;
    }
}
