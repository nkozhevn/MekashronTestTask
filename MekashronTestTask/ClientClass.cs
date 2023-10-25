using System;
using System.Net.Http.Headers;
using System.Text;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;

namespace MekashronTestTask
{
    public class ClientClass
    {
        private readonly string serviceUrl;

        public ClientClass(string serviceUrl)
        {
            this.serviceUrl = serviceUrl;
        }

        public async Task<string> CallWebServiceMethodAsync(string methodName, string login, string password)
        {
            string iPs = "0";
            string soapRequest = $@"<?xml version=""1.0"" encoding=""utf-8""?>
            <soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
                <soap:Body>
                    <{methodName} xmlns=""urn:ICUTech.Intf-IICUTech"">
                        <UserName>{login}</UserName>
                        <Password>{password}</Password>
                        <IPs>{iPs}</IPs>
                    </{methodName}>
                </soap:Body>
            </soap:Envelope>";

            using (HttpClient client = new HttpClient())
            {
                var content = new StringContent(soapRequest, Encoding.UTF8, "text/xml");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
                var response = await client.PostAsync(serviceUrl, content);
                var soapResponse = await response.Content.ReadAsStringAsync();

                Console.WriteLine(soapResponse);

                XDocument xdoc = XDocument.Parse(soapResponse);

                XElement returnElement = xdoc.Descendants("return").FirstOrDefault();
                if (returnElement != null)
                {
                    string jsonValue = returnElement.Value;
                    Console.WriteLine(jsonValue);

                    var json = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(jsonValue);

                    if(json.ContainsKey("ResultCode"))
                    {
                        int resultCode = json.ResultCode;
                        if(resultCode == -1)
                        {
                            return resultCode.ToString();
                        }
                    }
                    if(json.ContainsKey("EntityId"))
                    {
                        string entityId = json.EntityId;
                        return entityId;
                    }
                    else
                    {
                        return "-1";
                    }
                }
                else
                {
                    Console.WriteLine("Элемент <return> не найден в ответе.");
                    return "-1";
                }
            }
        }
    }
}

