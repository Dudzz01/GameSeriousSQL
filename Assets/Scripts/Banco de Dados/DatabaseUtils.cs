using UnityEngine;
using System.IO;
#if UNITY_ANDROID && !UNITY_EDITOR
using UnityEngine.Networking;
#endif

public static class DatabaseUtils
{
    public static string GetDatabasePath(string filename)
    {
        // Caminho onde o jogo vai usar/gravar o DB
        string dst = Path.Combine(Application.persistentDataPath, filename);

        if (!File.Exists(dst))
        {
            // Origem empacotada com o build
            string src = Path.Combine(Application.streamingAssetsPath, filename);
            byte[] data;

#if UNITY_ANDROID && !UNITY_EDITOR
            // Android usa UnityWebRequest
            var www = UnityWebRequest.Get(src);
            www.SendWebRequest();
            while (!www.isDone) {}
            data = www.downloadHandler.data;
#else
            // No Editor e desktop, usamos FileStream com FileShare para evitar o lock
            using (var fs = new FileStream(
                src,
                FileMode.Open,
                FileAccess.Read,
                FileShare.ReadWrite   // <-- permite abrir mesmo que Unity já tenha aberto
            ))
            {
                data = new byte[fs.Length];
                fs.Read(data, 0, data.Length);
            }
#endif

            // Grava na pasta gravável
            File.WriteAllBytes(dst, data);
        }

        return dst;
    }
}
