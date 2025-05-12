using System;
using UnityEngine;
using Models;
using TMPro;  

public class FurnitureInteractable : MonoBehaviour
{
    [Header("Identifica��o")]
    public string movableName;       

    [Header("Contexto SQL")]
    public string[] allowedTables;
    public Delegate validator;
    public SQLConsoleUI sqlUI;
    [Header("Blocos SQL deste m�vel")]
    public string[] tokens;

    [Header("TextosEnunciados")]
    
    public string textoEnunciado;

   

}
