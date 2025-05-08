using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(FurnitureInteractable))]
public class ComputadorCasaNamoradaSetup : MonoBehaviour
{
    // Desafio 5: BETWEEN no ComputadorCasaNamorada
    const int ExpectedMovelId = 26;
    // Queremos exatamente estes dois IDs para forçar o uso de BETWEEN 51 AND 52
    static readonly HashSet<int> ExpectedItemIds = new HashSet<int> { 51, 52 };

    void Awake()
    {
        var fi = GetComponent<FurnitureInteractable>();
        fi.validator = new Func<object, bool>(itemsObj =>
        {
            // 1) Verifica se é IEnumerable
            if (!(itemsObj is IEnumerable enumerable))
            {
                Debug.Log("Validator: não é IEnumerable → invalid");
                return false;
            }

            // 2) Converte para lista
            var lista = enumerable.Cast<object>().ToList();

            // 3) Deve retornar exatamente 2 registros
            if (lista.Count != ExpectedItemIds.Count)
            {
                Debug.Log($"Validator: esperava {ExpectedItemIds.Count} itens, mas recebeu {lista.Count} → invalid");
                return false;
            }

            var foundIds = new HashSet<int>();
            foreach (var item in lista)
            {
                var type = item.GetType();

                // 4) Confirma IdMovel
                var propMovel = type.GetProperty("IdMovel");
                if (propMovel == null)
                {
                    Debug.Log("Validator: objeto sem propriedade IdMovel → invalid");
                    return false;
                }
                int idMovel = Convert.ToInt32(propMovel.GetValue(item));
                if (idMovel != ExpectedMovelId)
                {
                    Debug.Log($"Validator: encontrou IdMovel={idMovel} (esperado {ExpectedMovelId}) → invalid");
                    return false;
                }

                // 5) Coleta IdItem
                var propItem = type.GetProperty("IdItem");
                if (propItem == null)
                {
                    Debug.Log("Validator: objeto sem propriedade IdItem → invalid");
                    return false;
                }
                foundIds.Add(Convert.ToInt32(propItem.GetValue(item)));
            }

            // 6) Garante que são exatamente 51 e 52
            if (!foundIds.SetEquals(ExpectedItemIds))
            {
                Debug.Log($"Validator: IDs retornados [{string.Join(",", foundIds)}] ≠ esperados [{string.Join(",", ExpectedItemIds)}] → invalid");
                return false;
            }

            Debug.Log("Validator: resposta correta → valid");
            return true;
        });
    }
}
