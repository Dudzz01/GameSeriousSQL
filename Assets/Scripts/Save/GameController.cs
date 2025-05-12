using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static Save s = new Save();
    private static GameObject gmController;
    private void Awake()
    {
        Application.targetFrameRate = 60;
        s.arrayFasesDesbloqueadas[0] = true; // fase 1
        Debug.Log("Desafios Concluidos: " + s.desafiosConcluidos);
        DontDestroyOnLoad(this.transform.root.gameObject);

        if (gmController == null)
        {
            gmController = this.transform.root.gameObject;
        }
        else
        {
            Destroy(this.transform.root.gameObject);
        }
    }

    private void Update()
    {


        switch (s.desafiosConcluidos)
        {
            case 12:
                if(s.arrayFasesDesbloqueadas[1] == false)
                {
                    s.arrayFasesDesbloqueadas[1] = true; // fase 2
                    GetComponent<SaveGame>().SaveGameOfScene(s);
                }
                
                break;
            case 28:
                if(s.arrayFasesDesbloqueadas[2] == false)
                {
                    s.arrayFasesDesbloqueadas[2] = true; // final
                    GetComponent<SaveGame>().SaveGameOfScene(s);
                }
                
                break;
            
        }
    }




}




