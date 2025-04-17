using UnityEngine;
using TMPro;
using System;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Models;

public class SQLConsoleUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject panel;        
    [SerializeField] private TMP_InputField inputField; 
    [SerializeField] private TMP_Text feedbackText;     

    private DatabaseManager db;
    private string[] allowedTables;
    private Delegate validator;
    private GameObject playerObj;

    void Start()
    {
        db = FindObjectOfType<DatabaseManager>();
        panel.SetActive(false);
    }

   
    public void Open(string[] tables, Delegate validator)
    {
        this.allowedTables = tables;
        this.validator = validator;
        inputField.text = "";
        feedbackText.text = "";
        panel.SetActive(true);
        inputField.ActivateInputField();
        playerObj = GameObject.FindGameObjectWithTag("Player");
        playerObj.GetComponent<Player>().GetComponent<Player>().enabled = false;
    }

   
    public void Close()
    {
        panel.SetActive(false);
        playerObj.GetComponent<Player>().GetComponent<Player>().enabled = true;
    }

   
    public void OnExecute()
    {
        string sql = inputField.text.Trim();

        
        if (!ValidateTables(sql))
        {
            feedbackText.text = "Consulta contém tabela não permitida.";
            return;
        }

        
        List<string> tables = ExtractTables(sql);
        if (tables.Count == 0)
        {
            feedbackText.text = "Nenhuma tabela em FROM/JOINS detectada.";
            return;
        }

        
        Type resultType;
        try
        {
            resultType = MapTableNameToType(tables[0]);
        }
        catch (Exception e)
        {
            feedbackText.text = e.Message;
            return;
        }

        try
        {
            
            MethodInfo exec = typeof(DatabaseManager)
                .GetMethod("ExecuteQuery", BindingFlags.Public | BindingFlags.Instance);
            MethodInfo gen = exec.MakeGenericMethod(resultType);
            object itemsObj = gen.Invoke(db, new object[] { sql });

           
            bool valid = (bool)validator.DynamicInvoke(itemsObj);

            
            string output = GetFormattedQueryResults(itemsObj);
            feedbackText.text = (valid ? "Resposta correta!\n" : "Resposta incorreta!\n")
                                + output;
        }
        catch (TargetInvocationException tie)
        {
            
            feedbackText.text = "Erro ao executar a consulta: " + tie.InnerException.Message;
            Debug.LogError(tie.InnerException);
        }
        catch (Exception ex)
        {
            feedbackText.text = "Erro na query: " + ex.Message;
            Debug.LogError(ex);
        }
    }

    // ----- Helpers Internos -----

    
    private List<string> ExtractTables(string sql)
    {
        var tables = new List<string>();
        var regex = new Regex(@"\bFROM\s+([^\s,]+)|\bJOIN\s+([^\s,]+)",
                               RegexOptions.IgnoreCase);
        foreach (Match m in regex.Matches(sql))
        {
            string tbl = !string.IsNullOrEmpty(m.Groups[1].Value)
                       ? m.Groups[1].Value
                       : m.Groups[2].Value;
            tbl = tbl.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries)[0];
            if (!tables.Contains(tbl, StringComparer.OrdinalIgnoreCase))
                tables.Add(tbl);
        }
        return tables;
    }

    
    private bool ValidateTables(string sql)
    {
        var regex = new Regex(@"\b(FROM|JOIN)\s+([A-Za-z0-9_]+)",
                              RegexOptions.IgnoreCase);
        foreach (Match m in regex.Matches(sql))
        {
            string tbl = m.Groups[2].Value;
            if (!allowedTables.Any(a =>
                a.Equals(tbl, StringComparison.OrdinalIgnoreCase)))
                return false;
        }
        return true;
    }

    
    private Type MapTableNameToType(string table)
    {
        switch (table.ToLower())
        {
            case "itens": return typeof(Item);
            case "moveis": return typeof(Movel);
            case "casas": return typeof(Casa);
            case "comodos": return typeof(Comodo);
            default:
                throw new ArgumentException($"Nenhum tipo mapeado para '{table}'");
        }
    }

   
    private string GetFormattedQueryResults(object itemsObj)
    {
        if (!(itemsObj is IEnumerable enumerable))
            return "Nenhum registro encontrado.";

        var sb = new StringBuilder();
        var enumIter = enumerable.GetEnumerator();

        if (!enumIter.MoveNext())
            return "Nenhum registro encontrado.";

        
        object first = enumIter.Current;
        var allProps = first.GetType()
                            .GetProperties(BindingFlags.Public | BindingFlags.Instance);

        
        var props = allProps.Where(p =>
            !(p.DeclaringType.Namespace?.StartsWith("UnityEngine") ?? false)
            && !typeof(UnityEngine.Object).IsAssignableFrom(p.PropertyType)
        ).ToArray();

        if (props.Length == 0)
            return "Nenhuma propriedade válida.";

        
        sb.AppendLine(string.Join("\t", props.Select(p => p.Name)));

        
        sb.AppendLine(GetRowValues(first, props));

        
        while (enumIter.MoveNext())
            sb.AppendLine(GetRowValues(enumIter.Current, props));

        return sb.ToString();
    }

   
    private string GetRowValues(object item, PropertyInfo[] props)
    {
        var row = new StringBuilder();
        foreach (var p in props)
        {
            try
            {
                var v = p.GetValue(item, null);
                row.Append(v != null ? v.ToString() : "null");
            }
            catch
            {
                row.Append("n/a");
            }
            row.Append("\t");
        }
        return row.ToString().TrimEnd('\t');
    }
}
