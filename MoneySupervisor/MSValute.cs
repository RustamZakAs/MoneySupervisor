using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;

namespace MoneySupervisor
{
    //[DataContract]
    class MSValute
    {
        //[DataMember]
        public int MSValuteId { get; set; }
        //[DataMember]
        public DateTime MSValuteDate { get; set; }
        //[DataMember]
        public string MSValuteType { get; set; }
        //[DataMember]
        public string MSValuteCode { get; set; }
        //[DataMember]
        public float MSValuteNominal { get; set; }
        //[DataMember]
        public string MSValuteName { get; set; }
        //[DataMember]
        public float MSValuteValue { get; set; }

        public static string ChooseValute()
        {

            return "AZN";
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
        private string msValuteLink = @"https://www.cbar.az/currencies/" +
            DateTime.Now.ToString("dd") + '.' +
            DateTime.Now.ToString("MM") + '.' +
            DateTime.Now.ToString("yyyy") + ".xml";

        public string MSValuteLink()
        {
            return msValuteLink;
        }

        

        public readonly DateTime msValuteDate = new DateTime(int.Parse(DateTime.Now.ToString("yyyy")), int.Parse(DateTime.Now.ToString("MM")), int.Parse(DateTime.Now.ToString("dd")));

        public MSValute(DateTime msValuteDate = default(DateTime),
                         //DateTime(int.Parse(DateTime.Now.ToString("yyyy")), int.Parse(DateTime.Now.ToString("MM")), int.Parse(DateTime.Now.ToString("dd"))),
                         //DateTime.Parse(DateTime.Now.ToString("ddMMyyyy")),
                         string msValuteType = "",
                         string msValuteCode = "",
                         float msValuteNominal = 0,
                         string msValuteName = "",
                         float msValuteValue = 0)
        {
            MSValuteType = msValuteType;
            MSValuteCode = msValuteCode;
            MSValuteNominal = msValuteNominal;
            MSValuteName = msValuteName;
            MSValuteValue = msValuteValue;
        }
        public MSValute(MSValute msValutes)
        {
            MSValuteType = msValutes.MSValuteType;
            MSValuteCode = msValutes.MSValuteCode;
            MSValuteNominal = msValutes.MSValuteNominal;
            MSValuteName = msValutes.MSValuteName;
            MSValuteValue = msValutes.MSValuteValue;
        }
        public List<MSValute> MSLoadValutes()
        {
            var MSValuteTypeList = new List<MSValute>();
            var msValuteType = new MSValute();

            msValuteType.MSValuteType = "Xarici valyuta";
            msValuteType.MSValuteCode = "AZN";
            msValuteType.MSValuteNominal = 1;
            msValuteType.MSValuteName = "Azərbaycan manatı";
            msValuteType.MSValuteValue = 1;

            MSValuteTypeList.Add(new MSValute(msValuteType));

            var xmlDoc = new XmlDocument();
            xmlDoc.Load(msValuteLink);
            XmlElement xRoot = xmlDoc.DocumentElement;
            foreach (XmlNode xnode in xRoot)
            {
                XmlNode attrType = xnode.Attributes.GetNamedItem("Type");
                if (attrType != null)
                {
                    msValuteType.MSValuteType = attrType.Value;
                    //                    Console.WriteLine(attrType.Value);
                }
                foreach (XmlNode childnode in xnode.ChildNodes)
                {
                    XmlNode attrCode = childnode.Attributes.GetNamedItem("Code");
                    if (attrCode != null)
                    {
                        msValuteType.MSValuteCode = attrCode.Value;
                        //                        Console.WriteLine(attrCode.Value);
                    }
                    foreach (XmlNode childChildNode in childnode)
                    {
                        if (childChildNode.Name == "Nominal")
                        {
                            if (attrType.Value == "Bank metalları")
                            {
                                msValuteType.MSValuteNominal = float.Parse("1,00");
                                //                                Console.WriteLine(1);
                            }
                            else
                            {
                                msValuteType.MSValuteNominal = float.Parse(childChildNode.InnerText.Replace('.', ','));
                                //                               Console.WriteLine(childChildNode.InnerText);
                            }
                        }
                        if (childChildNode.Name == "Name")
                        {
                            msValuteType.MSValuteName = childChildNode.InnerText;
                            //                           Console.WriteLine(childChildNode.InnerText);
                        }
                        if (childChildNode.Name == "Value")
                        {
                            msValuteType.MSValuteValue = float.Parse(childChildNode.InnerText.Replace('.', ','));
                            //                            Console.WriteLine(childChildNode.InnerText);
                        }
                    }
                    MSValuteTypeList.Add(new MSValute(msValuteType));
                }
            }
            return MSValuteTypeList;
        }
        public void MSShowListConsol(List<MSValute> MSValuteTypeList)
        {
            foreach (MSValute u in MSValuteTypeList)
            {
                string msValuteType = u.MSValuteType;
                msValuteType = msValuteType.Replace('ı', 'i');
                msValuteType = msValuteType.Replace('ə', 'e');
                msValuteType = msValuteType.Replace('Ə', 'E');
                msValuteType = msValuteType.Replace('ş', 's');

                string msValuteName = u.MSValuteName;
                msValuteName = msValuteName.Replace('ı', 'i');
                msValuteName = msValuteName.Replace('ə', 'e');
                msValuteName = msValuteName.Replace('Ə', 'E');
                msValuteName = msValuteName.Replace('ş', 's');

                Console.WriteLine("{0} - {1} - {2} - {3} - {4}",
                    msValuteType, u.MSValuteCode, u.MSValuteNominal, msValuteName, u.MSValuteValue);
            }
        }
    }
}
