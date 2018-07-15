using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MoneySupervisor
{
    public enum Lang { RU = 0, AZ, EN, KO }
    class MSLanguage
    {
        public string RUS { get; set; }
        public string AZE { get; set; }
        public string ENG { get; set; }
        public string KOR { get; set; }

        public MSLanguage()
        {
        }

        public MSLanguage(string rUS, string aZE, string eNG, string kOR)
        {
            RUS = rUS;
            AZE = aZE;
            ENG = eNG;
            KOR = kOR;
        }

        public string RetLang(int lang)
        {
            if (lang == 0)
            {
                return RUS;
            }
            else if (lang == 1)
            {
                return AZE;
            }
            else if (lang == 2)
            {
                return ENG;
            }
            else return KOR;
        }

        public void CreateDictionary(ref Dictionary<string, MSLanguage> dictionary)
        {
            MSLanguage phrase1 = new MSLanguage
            {
                RUS = "Новая категория ",
                AZE = "Yeni şəxs     ",
                ENG = "New person    ",
                KOR = "새로운 사람"
            };
            dictionary.Add("newper", phrase1);
        }
    }
}
