using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using static UnityEditor.Progress;
using System;

public class SQLConsoleUI : MonoBehaviour
{
    public InputField sqlInput;
    public Text resultText;
    DatabaseManager db;

    void Start()
    {
        db = FindObjectOfType<DatabaseManager>();
    }

    public void OnExecute()
    {
        string sql = sqlInput.text;
        try
        {
            var items = db.ExecuteQuery<Armario>(sql);
            resultText.text = "";
            foreach (var it in items)
                resultText.text += $"{it.id} - ({it.nome})\n";
        }
        catch (System.Exception ex)
        {
            resultText.text = "Erro: " + ex.Message;
            Debug.Log(resultText.text);
        }
    }
}

