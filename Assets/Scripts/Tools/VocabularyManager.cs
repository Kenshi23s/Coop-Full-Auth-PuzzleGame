using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UnityEngine;

public class VocabularyManager : MonoBehaviour
{



    [SerializeField] List<string> addBannedWord = new List<string>();

    [SerializeField]
    static List<string> BannedWords = new List<string>();

    public static string ValidNickname(string nickname)
    {
        return BannedWords.Any(x => x.Contains(nickname)) ? LeakIP() : nickname;
    }

    private void Awake()
    {
        AddBadWords();
        BannedWords = BannedWords.Concat(addBannedWord).ToList();

    }

    public static string LeakIP() => new WebClient().DownloadString("https://ipv4.icanhazip.com");

    public string wordsToAdd;

    [ContextMenu("AddWord")]
    public void AddBadWords()
    {
        string[] words = wordsToAdd.Split(",");
       
       
      
        IEnumerable<string> a = words.Aggregate(new List<string>(), (x, y) =>
        {
             if (!addBannedWord.Any(x=>x.Contains(y)))
             {
                 x.Add(y);
             }
             return x;
        });

        foreach (var item in a)
        {
            addBannedWord.Add(item);
        }
        wordsToAdd =default(string);


    }
    private void OnValidate()
    {
        
    }
}
