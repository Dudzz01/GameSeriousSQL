using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(FurnitureInteractable))]
public class LivroCasaNamoradaSetup : MonoBehaviour
{
    // Desafio 6: NOT no LivroCasaNamorada
    const int ExpectedMovelId = 28;
    static readonly HashSet<string> ExpectedItems = new HashSet<string> {
        "PortaRetratoQuartoCasaNamorada",
        "CaixaMemoriasCasaNamorada"
    };

    void Awake()
    {
        var fi = GetComponent<FurnitureInteractable>();
        fi.validator = new Func<object, bool>(itemsObj =>
        {
            // 1) Checa se é IEnumerable
            if (!(itemsObj is IEnumerable enumerable))
            {
                Debug.Log("Validator: não é IEnumerable → invalid");
                return false;
            }

            // 2) Materializa em lista
            var lista = enumerable.Cast<object>().ToList();

            // 3) Deve devolver exatamente os 2 itens sem "Chave"
            if (lista.Count != ExpectedItems.Count)
            {
                Debug.Log($"Validator: esperava {ExpectedItems.Count} itens, mas recebeu {lista.Count} → invalid");
                return false;
            }

            var found = new HashSet<string>();
            foreach (var item in lista)
            {
                var type = item.GetType();

                // 4) Confirma IdMovel
                var propMov = type.GetProperty("IdMovel");
                if (propMov == null) return Fail("sem propriedade IdMovel");
                if (Convert.ToInt32(propMov.GetValue(item)) != ExpectedMovelId)
                    return Fail($"IdMovel ≠ {ExpectedMovelId}");

                // 5) Confirma NomeItem e ausência de "Chave"
                var propNome = type.GetProperty("NomeItem");
                if (propNome == null) return Fail("sem propriedade NomeItem");
                var nome = propNome.GetValue(item) as string;
                if (nome.Contains("Chave"))
                    return Fail($"NomeItem contém 'Chave' → invalid");
                if (!ExpectedItems.Contains(nome))
                    return Fail($"NomeItem inesperado '{nome}'");

                found.Add(nome);
            }

            // 6) Garante que são os exatamente esperados
            if (!found.SetEquals(ExpectedItems))
                return Fail($"Itens retornados [{string.Join(",", found)}] ≠ esperados [{string.Join(",", ExpectedItems)}]");

            Debug.Log("Validator: resposta correta → valid");
            return true;
        });
    }

    private bool Fail(string msg)
    {
        Debug.Log($"Validator: {msg}");
        return false;
    }
}
