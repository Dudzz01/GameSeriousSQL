using System;
using System.Collections;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

[RequireComponent(typeof(FurnitureInteractable))]
public class FogaoDaCozinhaAvo : MonoBehaviour
{

    const int ExpectedMovelId = 74;  // o IdMovel do desafio
    const int ExpectedQtdItens = 5;   // quantos itens existem nesse móvel
    const int ExpectedQtdChaves = 0;   // quantas chaves existem (SUM NomeItem LIKE 'Chave%')

    void Awake()
    {
        var fi = GetComponent<FurnitureInteractable>();
        fi.validator = new Func<string, object, bool>((sql, itemsObj) =>
        {
            // 1) Normaliza espaços e pontos
            var norm = Regex.Replace(sql, @"\s+", " ").Trim();
            norm = Regex.Replace(norm, @"\s*\.\s*", ".");

            // 2) SELECT … FROM Itens
            var m = Regex.Match(norm,
                @"^SELECT\s+(?<cols>.*?)\s+FROM\s+(?<tbl>\w+)",
                RegexOptions.IgnoreCase);
            if (!m.Success)
                return DebugFail("não encontrou SELECT … FROM");
            if (!Regex.IsMatch(m.Groups["tbl"].Value, @"^itens$", RegexOptions.IgnoreCase))
                return DebugFail($"FROM deve ser 'Itens', achou '{m.Groups["tbl"].Value}'");

            // 3) Proíbe JOIN
            if (Regex.IsMatch(norm, @"\b(?:INNER|LEFT|RIGHT|FULL)\s+JOIN\b", RegexOptions.IgnoreCase))
                return DebugFail("nenhum JOIN é permitido neste exercício");

            // 4) Valida colunas exatas (em qualquer ordem)
            var cols = m.Groups["cols"].Value
                        .Split(',')
                        .Select(c => c.Trim())
                        .ToList();
            if (cols.Count != 3)
                return DebugFail($"esperava 3 colunas, achou {cols.Count}");

            var patterns = new[]
            {
                new Regex(@"^(?:Itens\.)?IdMovel$", RegexOptions.IgnoreCase),
                new Regex(@"^COUNT\(\s*\*\s*\)\s+AS\s+QtdItens$", RegexOptions.IgnoreCase),
                new Regex(@"^SUM\(\s*NomeItem\s+LIKE\s*'Chave%'\s*\)\s+AS\s+QtdChaves$", RegexOptions.IgnoreCase),
            };
            foreach (var pat in patterns)
                if (!cols.Any(col => pat.IsMatch(col)))
                    return DebugFail($"falta coluna correspondente a '{pat}'");

            // 5) WHERE IdMovel = X e somente uma cláusula WHERE
            var whereMatches = Regex.Matches(norm, @"\bWHERE\b", RegexOptions.IgnoreCase);
            if (whereMatches.Count != 1)
                return DebugFail("deve haver exatamente uma cláusula WHERE");
            var wherePat = new Regex(
                $@"\bWHERE\s+(?:\w+\.)?IdMovel\s*=\s*{ExpectedMovelId}\b",
                RegexOptions.IgnoreCase);
            if (!wherePat.IsMatch(norm))
                return DebugFail($"falta WHERE IdMovel = {ExpectedMovelId}");
            if (Regex.IsMatch(norm, @"\bAND\b|\bOR\b", RegexOptions.IgnoreCase))
                return DebugFail("AND/OR não permitido na cláusula WHERE");

            // 6) GROUP BY IdMovel e somente um GROUP BY
            var gbMatches = Regex.Matches(norm, @"\bGROUP\s+BY\b", RegexOptions.IgnoreCase);
            if (gbMatches.Count != 1)
                return DebugFail("deve haver exatamente um GROUP BY");
            var gbPat = new Regex(@"\bGROUP\s+BY\s+(?:\w+\.)?IdMovel\b", RegexOptions.IgnoreCase);
            if (!gbPat.IsMatch(norm))
                return DebugFail("falta GROUP BY IdMovel");

            // 7) HAVING SUM(NomeItem LIKE 'Chave%') > 0 e somente um HAVING
            var hMatches = Regex.Matches(norm, @"\bHAVING\b", RegexOptions.IgnoreCase);
            if (hMatches.Count != 1)
                return DebugFail("deve haver exatamente uma cláusula HAVING");
            var hPat = new Regex(@"\bHAVING\s+SUM\(\s*NomeItem\s+LIKE\s*'Chave%'\s*\)\s*>\s*0\b",
                                 RegexOptions.IgnoreCase);
            if (!hPat.IsMatch(norm))
                return DebugFail("HAVING incorreto — use HAVING SUM(NomeItem LIKE 'Chave%') > 0");

            // 8) LIMIT 1 e somente um LIMIT
            var lMatches = Regex.Matches(norm, @"\bLIMIT\b", RegexOptions.IgnoreCase);
            if (lMatches.Count != 1)
                return DebugFail("deve haver exatamente uma cláusula LIMIT");
            var lPat = new Regex(@"\bLIMIT\s+1\b", RegexOptions.IgnoreCase);
            if (!lPat.IsMatch(norm))
                return DebugFail("LIMIT incorreto — use LIMIT 1");

            // 9) Valida resultado
            if (!(itemsObj is IEnumerable en))
                return DebugFail("resultado não é IEnumerable");
            var list = en.Cast<object>().ToList();

            // se não esperamos nenhuma chave e não veio registro, aceitamos
            if (ExpectedQtdChaves == 0 && list.Count == 0)
                return true;

            // caso contrário, deve vir exatamente 1 registro
            if (list.Count != 1)
                return DebugFail($"esperava 1 registro, mas recebeu {list.Count}");

            // 10) Verifica valores
            var obj = list[0];
            var t = obj.GetType();

            var idVal = (int)t.GetProperty("IdMovel").GetValue(obj);
            if (idVal != ExpectedMovelId)
                return DebugFail($"IdMovel={idVal} (esperado {ExpectedMovelId})");

            var qtd = Convert.ToInt32(t.GetProperty("QtdItens").GetValue(obj));
            if (qtd != ExpectedQtdItens)
                return DebugFail($"QtdItens={qtd} (esperado {ExpectedQtdItens})");

            var chv = Convert.ToInt32(t.GetProperty("QtdChaves").GetValue(obj));
            if (chv != ExpectedQtdChaves)
                return DebugFail($"QtdChaves={chv} (esperado {ExpectedQtdChaves})");

            Debug.Log("Validator: resposta correta → valid");
            return true;
        });
    }

    bool DebugFail(string reason)
    {
        Debug.Log($"Validator: {reason} → invalid");
        return false;
    }
}
