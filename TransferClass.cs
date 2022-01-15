using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Concurrent;

namespace Transfer
{
    class TransferClass
    {
        private Dictionary<string, int> MakeDictionary(string text)
        {
            Dictionary<string, int> wordlist = new Dictionary<string, int>();
            text = text.ToLower();
            string[] words = text.Split(new char[] { ' ', ',', '-', '.', '!', '?', '(', ')', '[', ']', '\"', '\'', '\n', '\r', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string word in words)
            {
                if (wordlist.ContainsKey(word))
                    wordlist[word]++;
                else
                    wordlist.Add(word, 1);
            }
            return wordlist;
        }
        private Dictionary<string, int> MakeDictionaryParallel(string text)
        {
            ConcurrentDictionary<string, int> wordlist = new ConcurrentDictionary<string, int>();
            text = text.ToLower();
            string[] words = text.Split(new char[] { ' ', ',', '-', '.', '!', '?', '(', ')', '[', ']', '\"', '\'', '\n', '\r', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            
            
            Parallel.ForEach<string>(words, word => {
                if (wordlist.ContainsKey(word))
                    wordlist[word]++;
                else
                    wordlist.TryAdd(word, 1);
            });            
            return wordlist.ToDictionary(k => k.Key, v => v.Value);
        }
    }
}
