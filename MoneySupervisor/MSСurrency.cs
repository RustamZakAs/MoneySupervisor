using System;
using System.Xml;
using System.Net;
using System.Text;
using System.Linq;
using System.Collections.Generic;

namespace MoneySupervisor
{
    //[DataContract]
    class MSСurrency
    {
        //[DataMember]
        public int MSСurrencyId { get; set; }
        //[DataMember]
        public DateTime MSСurrencyDate { get; set; }
        //[DataMember]
        public string MSСurrencyType { get; set; }
        //[DataMember]
        public string MSСurrencyCode { get; set; }
        //[DataMember]
        public float MSСurrencyNominal { get; set; }
        //[DataMember]
        public string MSСurrencyName { get; set; }
        //[DataMember]
        public float MSСurrencyValue { get; set; }

        public static string ChooseСurrency(ref List<MSСurrency> msСurrenciesList)
        {
            List<string> msCurrenciesCodeList = new List<string>();
            for (int i = 0; i < msСurrenciesList.Count; i++)
            {
                string temp = msСurrenciesList[i].MSСurrencyCode.ToUpper();
                if (!msCurrenciesCodeList.Exists(x => x.ToUpper() == temp))
                {
                    msCurrenciesCodeList.Add(temp);
                }
            }
            int left = Console.CursorLeft;
            int top = Console.CursorTop;
            int xIndex = 0;
            do
            {
                Console.SetCursorPosition(left, top);
                if (Console.KeyAvailable)
                {
                    Program.cki = Console.ReadKey();
                }
                switch (Program.cki.Key)
                {
                    case ConsoleKey.DownArrow:
                        if (++xIndex >= msCurrenciesCodeList.Count) xIndex = msCurrenciesCodeList.Count - 1;
                        break;
                    case ConsoleKey.UpArrow:
                        if (--xIndex < 0) xIndex = 0;
                        break;
                    case ConsoleKey.Enter:
                        return msCurrenciesCodeList[xIndex];
                    default:
                        break;
                }
                string xSynbol = ""; // ↓   ↑   ↓↑
                if (xIndex == 0) xSynbol = "↓";
                else if (xIndex >= msCurrenciesCodeList.Count - 1) xSynbol = "↑";
                else xSynbol = "↓↑";
                Console.WriteLine($"{msCurrenciesCodeList[xIndex]} {xSynbol}");
            } while (true);
        }

        public bool CheckURL(String url)
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
        private string msСurrencyLink = @"https://www.cbar.az/currencies/" +
            DateTime.Now.ToString("dd") + '.' +
            DateTime.Now.ToString("MM") + '.' +
            DateTime.Now.ToString("yyyy") + ".xml";

        public string MSСurrencyLink()
        {
            return msСurrencyLink;
        }

        public static readonly DateTime msСurrencyDate = new DateTime(int.Parse(DateTime.Now.ToString("yyyy")), 
            int.Parse(DateTime.Now.ToString("MM")), 
            int.Parse(DateTime.Now.ToString("dd")));

        public MSСurrency()
        {
            
        }
        public MSСurrency(MSСurrency msСurrency)
        {
            MSСurrencyType = msСurrency.MSСurrencyType;
            MSСurrencyCode = msСurrency.MSСurrencyCode;
            MSСurrencyNominal = msСurrency.MSСurrencyNominal;
            MSСurrencyName = msСurrency.MSСurrencyName;
            MSСurrencyValue = msСurrency.MSСurrencyValue;
        }
        public List<MSСurrency> MSLoadСurrencies()
        {
            var MSСurrencyTypeList = new List<MSСurrency>();
            var msСurrencyType     = new MSСurrency();


            msСurrencyType.MSСurrencyId      = 1;
            msСurrencyType.MSСurrencyType    = "Xarici valyuta";
            msСurrencyType.MSСurrencyCode    = "AZN";
            msСurrencyType.MSСurrencyNominal = 1;
            msСurrencyType.MSСurrencyName    = "Azərbaycan manatı";
            msСurrencyType.MSСurrencyValue   = 1;

            MSСurrencyTypeList.Add(new MSСurrency(msСurrencyType));

            var xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.Load(msСurrencyLink); //Не возможно связться с сервером для загрузки данных о валютах!
                XmlElement xRoot = xmlDoc.DocumentElement;
                foreach (XmlNode xnode in xRoot)
                {
                    XmlNode attrType = xnode.Attributes.GetNamedItem("Type");
                    if (attrType != null)
                    {
                        msСurrencyType.MSСurrencyType = attrType.Value;
                        //Console.WriteLine(attrType.Value);
                    }
                    foreach (XmlNode childnode in xnode.ChildNodes)
                    {
                        XmlNode attrCode = childnode.Attributes.GetNamedItem("Code");
                        if (attrCode != null)
                        {
                            msСurrencyType.MSСurrencyCode = attrCode.Value;
                            //Console.WriteLine(attrCode.Value);
                        }
                        foreach (XmlNode childChildNode in childnode)
                        {
                            if (childChildNode.Name == "Nominal")
                            {
                                if (attrType.Value == "Bank metalları")
                                {
                                    msСurrencyType.MSСurrencyNominal = float.Parse("1,00");
                                    //Console.WriteLine(1);
                                }
                                else
                                {
                                    msСurrencyType.MSСurrencyNominal = float.Parse(childChildNode.InnerText.Replace('.', ','));
                                    //Console.WriteLine(childChildNode.InnerText);
                                }
                            }
                            if (childChildNode.Name == "Name")
                            {
                                msСurrencyType.MSСurrencyName = childChildNode.InnerText;
                                //Console.WriteLine(childChildNode.InnerText);
                            }
                            if (childChildNode.Name == "Value")
                            {
                                msСurrencyType.MSСurrencyValue = float.Parse(childChildNode.InnerText.Replace('.', ','));
                                //Console.WriteLine(childChildNode.InnerText);
                            }
                        }
                        MSСurrencyTypeList.Add(new MSСurrency(msСurrencyType));
                    }
                }
            }
            catch (System.Net.WebException)
            {
                Console.WriteLine("Не возможно связться с сервером для загрузки данных о валютах!");
            }
            catch (Exception)
            {
                Console.WriteLine("Произошла ошибка!");
            }
            
            return MSСurrencyTypeList;
        }
    }
}
