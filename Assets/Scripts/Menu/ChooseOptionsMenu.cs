using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChooseOptionsMenu : MonoBehaviour
{
    // Start is called before the first frame update

    private void Start()
    {
        //audioSourceMenu = GetComponent<AudioSource>();
    }

    public void SelectPlay()
    {

        SceneManager.LoadScene("SelecionarFase");
    }

    public void Creditos()
    {
        SceneManager.LoadScene("Creditos");
    }

    public void Exit()
    {
        Application.Quit();
    }

}
