using System;
using UnityEngine;
using Models;  // apenas se precisar de SQLConsoleUI

public class FurnitureInteractable : MonoBehaviour
{
    [Header("Identifica��o")]
    public string movableName;       

    [Header("Contexto SQL")]
    public string[] allowedTables;
    public Delegate validator;
    public SQLConsoleUI sqlUI;
}
