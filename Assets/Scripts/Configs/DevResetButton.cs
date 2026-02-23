using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class DevResetButton : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Button resetAllButton;

    [Header("Opções")]
    [SerializeField] private bool reloadScene = true;
    [SerializeField] private string sceneToReload = ""; // vazio = cena atual

    private void Awake()
    {
        if (resetAllButton) resetAllButton.onClick.AddListener(ResetSaveOnly);
    }

    [ContextMenu("Reset SAVE only")]
    public void ResetSaveOnly()
    {
        GameController.s = new Save();
        GameController.s.arrayFasesDesbloqueadas[0] = true;
        var savePath = Path.Combine(Application.persistentDataPath, "saveSceneGame.save");
        if (File.Exists(savePath)) File.Delete(savePath);
        Debug.Log("[DevReset] SAVE resetado.");
        if (reloadScene) SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    
}
