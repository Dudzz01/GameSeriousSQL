using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Configuração de Interação")]
    [SerializeField] float interactRadius = 2f;
    [SerializeField] KeyCode interactKey = KeyCode.LeftControl;

    [Header("Referências de UI")]
    [SerializeField] GameObject promptUI;           // painel que contém o texto
    [SerializeField] TextMeshProUGUI promptText;    // componente de texto
    [SerializeField] Button closeConsoleButton;     // botão “Fechar” do painel de consulta

    private FurnitureInteractable nearestFI;
    private bool consoleOpen = false;                // controla se o console está aberto

    void Start()
    {
        if (closeConsoleButton != null)
            closeConsoleButton.onClick.AddListener(CloseConsole);
    }

    void Update()
    {
        
        var all = FindObjectsOfType<FurnitureInteractable>();

       
        var inRange = all
            .Select(fi => new {
                fi,
                dist = Vector3.Distance(transform.position, fi.transform.position)
            })
            .Where(x => x.dist <= interactRadius)
            .OrderBy(x => x.dist)
            .FirstOrDefault();

        if (inRange != null)
        {
            nearestFI = inRange.fi;

            
            if (!consoleOpen)
            {
                promptUI.SetActive(true);
                promptText.text = $"Pressione CTRL para acessar {nearestFI.movableName}";
            }

            
            if (Input.GetKeyDown(interactKey))
            {
                nearestFI.sqlUI.Open(nearestFI.allowedTables, nearestFI.validator);
                consoleOpen = true;
                promptUI.SetActive(false);
            }
        }
        else
        {
            nearestFI = null;
            promptUI.SetActive(false);
        }

        
        if (Input.GetKeyDown(KeyCode.Escape) && consoleOpen)
            CloseConsole();
    }

    private void CloseConsole()
    {
        if (nearestFI != null)
            nearestFI.sqlUI.Close();

        consoleOpen = false;

        if (nearestFI != null &&
            Vector3.Distance(transform.position, nearestFI.transform.position) <= interactRadius)
        {
            promptUI.SetActive(true);
            promptText.text = $"Pressione CTRL para acessar {nearestFI.movableName}";
        }
        else
        {
            promptUI.SetActive(false);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, interactRadius);
    }
}
