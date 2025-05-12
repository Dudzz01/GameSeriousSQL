using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChooseFases : MonoBehaviour
{

    [SerializeField] private Image[] spriteLevel = new Image[6];
    [SerializeField] private Sprite[] spriteBotoes = new Sprite[2];

    private void Update()
    {
        ConfigArtMenu();
    }
    public void Fase1()
    {
        if (GameController.s.arrayFasesDesbloqueadas[0] == true)
        {
            SceneManager.LoadScene("Fase 1");
        }
    }

    public void Fase2()
    {
        if (GameController.s.arrayFasesDesbloqueadas[1] == true)
        {
            SceneManager.LoadScene("Fase 2");
        }
    }

    public void FaseFinal()
    {
        if (GameController.s.arrayFasesDesbloqueadas[2] == true)
        {
            SceneManager.LoadScene("Fase Final");
        }
    }

    public void BackMenu()
    {
        SceneManager.LoadScene("Menu");
    }


    public void Fase3()
    {

        SceneManager.LoadScene("Fase 3");
    }

    public void Fase4()
    {

        SceneManager.LoadScene("Fase 4");
    }

    public void Fase5()
    {

        SceneManager.LoadScene("Fase 5");
    }

    public void Fase6()
    {

        SceneManager.LoadScene("Fase 6");
    }

    public void Fase7()
    {

        SceneManager.LoadScene("Fase 7");
    }

    public void Fase8()
    {

        SceneManager.LoadScene("Fase 8");
    }

    public void Fase9()
    {

        SceneManager.LoadScene("Fase 9");
    }

    public void Fase10()
    {

        SceneManager.LoadScene("Fase 10");
    }

    public void ConfigArtMenu()
    {
        if (GameController.s.arrayFasesDesbloqueadas[0] == true)
        {
            spriteLevel[0].sprite = spriteBotoes[0];
        }
        else
        {
            spriteLevel[0].sprite = spriteBotoes[1];
        }

        if (GameController.s.arrayFasesDesbloqueadas[1] == true)
        {
            spriteLevel[1].sprite = spriteBotoes[0];
        }
        else
        {
            spriteLevel[1].sprite = spriteBotoes[1];
        }

        if (GameController.s.arrayFasesDesbloqueadas[2] == true)
        {
            spriteLevel[2].sprite = spriteBotoes[0];
        }
        else
        {
            spriteLevel[2].sprite = spriteBotoes[1];
        }

        //if (GameController.s.arrayFasesDesbloqueadas[3] == true)
        //{
        //    spriteLevel[3].sprite = spriteBotoes[0];
        //}
        //else
        //{
        //    spriteLevel[3].sprite = spriteBotoes[1];
        //}

        //if (GameController.s.arrayFasesDesbloqueadas[4] == true)
        //{
        //    spriteLevel[4].sprite = spriteBotoes[0];
        //}
        //else
        //{
        //    spriteLevel[4].sprite = spriteBotoes[1];
        //}

    }


}
