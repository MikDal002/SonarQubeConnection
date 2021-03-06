﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace SonarQubeTest
{
    public class SonarQubeConnection
    {
        private string _baseAddress = @"http://localhost:9000/api/measures/search_history";
        private string _urlParams = $"?metrics={Vulnerabilities},{Lines},{Statements},{DuplicatedLinesDensity},{Complexity},{Functions},{Classes},{CodeSmells}&component=battletanks";

        public static string CodeSmells => "code_smells";
        public static string Classes => "classes";
        public static string Functions => "functions";
        public static string Complexity => "complexity";
        public static string DuplicatedLinesDensity => "duplicated_lines_density";
        public static string Statements => "statements";
        public static string Lines => "lines";
        public static string Vulnerabilities => "vulnerabilities";


        public Dictionary<DateTimeOffset, Dictionary<string, double>> Ask()
        {
            try
            {

                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(_baseAddress);

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/JSON"));

                HttpResponseMessage response = client.GetAsync(_urlParams).Result;
                if (response.IsSuccessStatusCode)
                {
                    var dataObjects = response.Content.ReadAsStringAsync().Result;
                    var component = JsonConvert.DeserializeObject<SonarQubeMeasurments>(dataObjects);
                    Dictionary<DateTimeOffset, Dictionary<string, double>> retDict = component.Measures.First().History.ToDictionary(d => DateTimeOffset.Parse(d.Date), _ => new Dictionary<string, double>());

                    foreach (var meas in component.Measures)
                    {
                        foreach (var hist in meas.History)
                        {
                            retDict[DateTimeOffset.Parse(hist.Date)].Add(meas.Metric, Convert.ToDouble(hist.Value));
                        }
                    }
                    return retDict;
                    //return component.Measures.First().Measures.ToDictionary(d => d.Metric, e => Convert.ToDouble((string) e.Value));
                }
                else
                {
                    throw new SonarQubeConnectionException("Serwer zwrócił odpowiedź o kodzie: " + response.StatusCode);
                }
            }
            catch (Exception exce)
            {
                if (exce.InnerException is HttpRequestException er)
                    throw new SonarQubeConnectionException("Nie udało sie nawiązać połączenia z SonarQube", er);
                else
                    throw exce;
            }
        }
    }

    public class SonarQubeConnectionException : Exception
    {
        public SonarQubeConnectionException()
        {
        }

        public SonarQubeConnectionException(string message) : base(message)
        {
        }

        public SonarQubeConnectionException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected SonarQubeConnectionException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}