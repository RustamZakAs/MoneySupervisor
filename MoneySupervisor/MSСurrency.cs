using System;
using System.Xml;
using System.Net;
using System.Text;
using System.Linq;
using System.Collections.Generic;

namespace MoneySupervisor
{
    //[DataContract]
    class MSCurrency
    {
        //[DataMember]
        public int MSCurrencyId { get; set; }
        //[DataMember]
        public DateTime MSCurrencyDate { get; set; }
        //[DataMember]
        public string MSCurrencyType { get; set; }
        //[DataMember]
        public string MSCurrencyCode { get; set; }
        //[DataMember]
        public float MSCurrencyNominal { get; set; }
        //[DataMember]
        public string MSCurrencyName { get; set; }
        //[DataMember]
        public float MSCurrencyValue { get; set; }

        public static string ChooseCurrency(ref List<MSCurrency> msCurrencyList)
        {
            List<string> msCurrenciesCodeList = new List<string>();
            for (int i = 0; i < msCurrencyList.Count; i++)
            {
                string temp = msCurrencyList[i].MSCurrencyCode.ToUpper();
                if (!msCurrenciesCodeList.Exists(x => x.ToUpper() == temp))
                {
                    msCurrenciesCodeList.Add(temp);
                }
            }
            int left = Console.CursorLeft;
            int top = Console.CursorTop;
            int xIndex = 0;
            string xSynbol = "↓ "; // ↓   ↑   ↓↑
            if (msCurrencyList.Count == 1) xSynbol = "  ";
            Console.WriteLine($"{msCurrenciesCodeList[xIndex]} {xSynbol}");
            do
            {
                Program.cki = Console.ReadKey(true);
                switch (Program.cki.Key)
                {
                    case ConsoleKey.DownArrow:
                        if (++xIndex >= msCurrenciesCodeList.Count) xIndex = msCurrenciesCodeList.Count - 1;
                        break;
                    case ConsoleKey.UpArrow:
                        if (--xIndex < 0) xIndex = 0;
                        break;
                    case ConsoleKey.Enter:
                        Program.cki = default(ConsoleKeyInfo);
                        Console.SetCursorPosition(0, top + 1);
                        return msCurrenciesCodeList[xIndex];
                    default:
                        break;
                }
                if (xIndex == 0) xSynbol = "↓ ";
                else if (xIndex >= msCurrenciesCodeList.Count - 1) xSynbol = "↑ ";
                else xSynbol = "↓↑";
                Console.SetCursorPosition(left, top);
                Console.WriteLine($"{msCurrenciesCodeList[xIndex]} {xSynbol}");
            } while (true);
        }

        public static bool CheckURL(String url)
        {
            if (String.IsNullOrEmpty(url))
                return false;
            WebRequest request = WebRequest.Create(url);
            try
            {
                HttpWebResponse res = request.GetResponse() as HttpWebResponse;
                if (res.StatusDescription == "OK")
                    return true;
            }
            catch
            {
            }
            return false;
        }
        public static string msCurrencyLink = @"https://www.cbar.az/currencies/" +
            DateTime.Now.ToString("dd") + '.' +
            DateTime.Now.ToString("MM") + '.' +
            DateTime.Now.ToString("yyyy") + ".xml";

        public string MSCurrencyLink()
        {
            return msCurrencyLink;
        }

        public MSCurrency()
        {
            
        }
        public MSCurrency(MSCurrency msCurrency)
        {
            MSCurrencyType = msCurrency.MSCurrencyType;
            MSCurrencyCode = msCurrency.MSCurrencyCode;
            MSCurrencyNominal = msCurrency.MSCurrencyNominal;
            MSCurrencyName = msCurrency.MSCurrencyName;
            MSCurrencyValue = msCurrency.MSCurrencyValue;
        }
        public List<MSCurrency> MSLoadCurrencies()
        {
            var MSCurrencyTypeList = new List<MSCurrency>();
            var msCurrencyType     = new MSCurrency();

            msCurrencyType.MSCurrencyId      = 0;
            msCurrencyType.MSCurrencyDate    = Program.msCompDateTime();
            msCurrencyType.MSCurrencyType    = "Xarici valyuta";
            msCurrencyType.MSCurrencyCode    = "AZN";
            msCurrencyType.MSCurrencyNominal = 1;
            msCurrencyType.MSCurrencyName    = "Azərbaycan manatı";
            msCurrencyType.MSCurrencyValue   = 1;

            MSCurrencyTypeList.Add(new MSCurrency(msCurrencyType));

            var xmlDoc = new XmlDocument();
            try
            {
                Console.WriteLine("Попытка загрузки валют C Cервера.");
                xmlDoc.Load(msCurrencyLink); //Не возможно CвязтьCя C Cервером для загрузки данных о валютах!
                XmlElement xRoot = xmlDoc.DocumentElement;
                foreach (XmlNode xnode in xRoot)
                {
                    XmlNode attrType = xnode.Attributes.GetNamedItem("Type");
                    if (attrType != null)
                    {
                        msCurrencyType.MSCurrencyType = attrType.Value;
                        //Console.WriteLine(attrType.Value);
                    }
                    foreach (XmlNode childnode in xnode.ChildNodes)
                    {
                        XmlNode attrCode = childnode.Attributes.GetNamedItem("Code");
                        if (attrCode != null)
                        {
                            msCurrencyType.MSCurrencyCode = attrCode.Value;
                            //Console.WriteLine(attrCode.Value);
                        }
                        foreach (XmlNode childChildNode in childnode)
                        {
                            if (childChildNode.Name == "Nominal")
                            {
                                if (attrType.Value == "Bank metalları")
                                {
                                    msCurrencyType.MSCurrencyNominal = float.Parse("1,00");
                                    //Console.WriteLine(1);
                                }
                                else
                                {
                                    string tString = childChildNode.InnerText;
                                    int tInt = tString.IndexOf('.');
                                    if (tInt > 0)
                                        msCurrencyType.MSCurrencyNominal = float.Parse(childChildNode.InnerText.Replace('.', ','));
                                    else
                                        msCurrencyType.MSCurrencyNominal = float.Parse(childChildNode.InnerText);
                                    //Console.WriteLine(childChildNode.InnerText);
                                }
                            }
                            if (childChildNode.Name == "Name")
                            {
                                msCurrencyType.MSCurrencyName = childChildNode.InnerText;
                                //Console.WriteLine(childChildNode.InnerText);
                            }
                            if (childChildNode.Name == "Value")
                            {
                                msCurrencyType.MSCurrencyValue = float.Parse(childChildNode.InnerText.Replace('.', ','));
                                //Console.WriteLine(childChildNode.InnerText);
                            }
                        }
                        MSCurrencyTypeList.Add(new MSCurrency(msCurrencyType));
                    }
                }
                Console.WriteLine("Загрузка валют C Cервера завершена.");
            }
            catch (System.Net.WebException)
            {
                Console.WriteLine("Не возможно CвязатьCя C Cервером для загрузки данных о валютах!");
            }
            catch (Exception)
            {
                Console.WriteLine("Произошла не предвиденная ошибка!");
            }
            return MSCurrencyTypeList;
        }
    }
}
