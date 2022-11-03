using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using edu.stanford.nlp.ling;
using edu.stanford.nlp.tagger.maxent;
using java.io;
using java.util;
using System.Text;
using System.Text.RegularExpressions;

namespace GES.Inside.Data
{
    public static class PosTagger
    {
        //public const string Model =  @"..\..\Data\stanford-postagger-2013-06-20\models\wsj-0-18-bidirectional-nodistsim.tagger";       
        public const string Model = @"App_Data\NLP\stanford-postagger-2015-12-09\models\english-bidirectional-distsim.tagger";
        private static readonly MaxentTagger Tagger = new MaxentTagger(string.Format(@"{0}\{1}", AppDomain.CurrentDomain.BaseDirectory, Model));

        static readonly IList<string> InterestPatterns = new List<string>
            {
                "CD:N",
                "JJ:N",
                "NNP:NN",
                "RB:NN"
            };

        static readonly IDictionary<string, int> RemoveList = new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase)
        {
            {"-RRB-", 0},
            {"-LRB-", 0}
        };

        static readonly IDictionary<string, int> SkipThrough = new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase)
        {
            {"TO", 0}
        };

        private static IEnumerable<string> TagReader(Reader reader)
        {
            var results = new List<string>();
            MaxentTagger.tokenizeText(reader)
                .toArray()
                .Cast<List>()
                .Select(sentence => Tagger.tagSentence(sentence))
                .ToList()
                .ForEach(kwl => results.AddRange(GetInterestingKeywords(kwl)));
            return RemoveSubsets(results);
        }

        private static IEnumerable<string> GetInterestingKeywords(List tSentence)
        {
            System.Console.WriteLine("Sentence: {0}", tSentence);
            var keywords = new List<string>();
            var phrase = new StringBuilder();
            foreach (var pattern in InterestPatterns)
            {
                phrase.Clear();
                var p = pattern.Split(':');
                for (var i = 0; i < tSentence.size(); i++)
                {
                    var tw = (TaggedWord)tSentence.get(i);
                    // start condition is an empty phrase and tag matches start pattern.
                    if (phrase.Length == 0)
                    {
                        // start condition
                        if (tw.tag().StartsWith(p[0]))
                        {
                            phrase.Append(CleanValue(tw.value()));
                        }
                    }
                    // phrase started, so fill in the blanks.
                    else
                    {
                        phrase.Append(CleanValue(tw.value()));
                        // end condition
                        if (!tw.tag().StartsWith(p[1])) continue;

                        // match greedy patterns
                        var nextTaggedWord = i + 1 < tSentence.size()
                            ? (TaggedWord)tSentence.get(i + 1)
                            : null;

                        if (nextTaggedWord == null ||
                            !SkipThrough.ContainsKey(nextTaggedWord.tag()) && !nextTaggedWord.tag().StartsWith(p[1]))
                        {
                            keywords.Add(phrase.ToString().ToLower().Trim());
                            phrase.Clear();
                        }
                    }
                }
            }
            return keywords;
        }

        private static string CleanValue(string value)
        {
            if (RemoveList.ContainsKey(value))
            {
                return string.Empty;
            }
            return " " + value.Replace("\\\\", "");
        }

        private static IEnumerable<string> RemoveSubsets(IList<string> lst)
        {
            // get rid of dups.
            var result = new HashSet<string>(lst);

            // get rid of subsets.
            foreach (
                var k in
                    lst.Select(l => l.Trim())
                        .SelectMany(
                            l =>
                                lst.Select(k => k.Trim())
                                    .Where(k => l != k)
                                    .Where(k => l.IndexOf(k, StringComparison.Ordinal) != -1 && result.Contains(k))))
            {
                result.Remove(k);
            }
            return result.ToList();
        }

        public static IEnumerable<string> TagText(string text)
        {
            var res = TagReader(new StringReader(text));
            return res;
        }

        public static IEnumerable<string> ExtractKeywords(string text)
        {
            var kws = TagText(text);

            // CONST
            const int tooLongKwLimit = 4;
            const int maxNumOfKeywords = 15;

            // Remove too long keywords
            kws = kws.Where(i => i.Split(' ').Count() < tooLongKwLimit);

            // Remove bad keywords
            kws = kws.Where(i => Regex.IsMatch(i, "^[^0-9,.`]+$"));

            return kws.Take(maxNumOfKeywords);
        }
    }
}