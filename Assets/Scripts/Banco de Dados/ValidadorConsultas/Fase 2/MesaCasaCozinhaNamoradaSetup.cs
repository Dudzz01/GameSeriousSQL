using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(FurnitureInteractable))]
public class MesaCasaCozinhaNamoradaSetup : MonoBehaviour
{
 
    const int ExpectedMovelId = 15;
    const string ExpectedNomeItem = "ChaveCozinhaCasaNamorada";

    void Awake()
    {
        var fi = GetComponent<FurnitureInteractable>();

        fi.validator = new Func<object, bool>(itemsObj =>
        {
   
            if (!(itemsObj is IEnumerable enumerable))
            {
                Debug.Log("Validator: não é IEnumerable → invalid");
                return false;
            }

            var lista = enumerable.Cast<object>().ToList();
       
            if (lista.Count != 1)
            {
                Debug.Log($"Validator: esperava 1 item, mas recebeu {lista.Count} → invalid");
                return false;
            }

   
            var item = lista[0];
            var type = item.GetType();


            var propId = type.GetProperty("IdMovel");
            if (propId == null)
            {
                Debug.Log("Validator: objeto sem propriedade IdMovel → invalid");
                return false;
            }
            int idMovel = Convert.ToInt32(propId.GetValue(item));
            if (idMovel != ExpectedMovelId)
            {
                Debug.Log($"Validator: IdMovel={idMovel} (esperado {ExpectedMovelId}) → invalid");
                return false;
            }

            var propNome = type.GetProperty("NomeItem");
            if (propNome == null)
            {
                Debug.Log("Validator: objeto sem propriedade NomeItem → invalid");
                return false;
            }
            var nomeItem = propNome.GetValue(item) as string;
            if (nomeItem != ExpectedNomeItem)
            {
                Debug.Log($"Validator: NomeItem='{nomeItem}' (esperado '{ExpectedNomeItem}') → invalid");
                return false;
            }

            Debug.Log("Validator: resposta correta → valid");
            return true;
        });
    }
}
