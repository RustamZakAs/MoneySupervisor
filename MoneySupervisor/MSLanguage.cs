using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MoneySupervisor
{
    public enum MSLang { RU = 0, AZ, EN, KO }
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
                RUS = "Новая категория",
                AZE = "Yeni kateqoriya",
                ENG = "New category",
                KOR = "새 카테고리"
            };
            dictionary.Add("newcategory", phrase1);

            MSLanguage phrase2 = new MSLanguage
            {
                RUS = "Новый счёт",
                AZE = "Yeni hesab",
                ENG = "New account",
                KOR = "새 계정"
            };
            dictionary.Add("newaccount", phrase2);

            MSLanguage phrase3 = new MSLanguage
            {
                RUS = "",
                AZE = "",
                ENG = "",
                KOR = ""
            };
            dictionary.Add("", phrase3);

            MSLanguage phrase4 = new MSLanguage
            {
                RUS = "",
                AZE = "",
                ENG = "",
                KOR = ""
            };
            dictionary.Add(" ", phrase4);
        }
    }
}
