
                using System;
                using System.Collections.Generic;
                using System.Linq;
                using System.Text;
                using System.Threading.Tasks;

                namespace CapaUtilitarios
                {
                    public class ConnectionHelper
                    {
                        public string GetConnectionString(string database)
                        {
                            return ObtenerCadena(database);
                        }
                        string ObtenerCadena(string databaseName)
                        {
                            Dictionary<string, string> cadenas = new Dictionary<string, string>()
                            {
                                { "BD_ONLINE", "kucYrF495EQlvfafWdx/PUGJSZlgajj552uePa+WwpugSMa/sy6D7ksBL2urV+84+fjlSXw3oyJwiRdZ6AJcI0NdicN50W1PvCdx8VvcbSb4IGMDSrUjSytZ0eLTUWz67wF4rSbdVPd3ArqyAtJs1PWkqvboG7KEGrC8s4fDHVM=" },{ "BD_TECNOLOGIAS", "SYLI63bZWA/STL0NRa9hjbY4TeTs9zRCP5CsE9MINxhnjRTK9EVLSX79eoQMajGXM34FMjZ7/J1W6hN4JkHarRaA0zvwchM2X3Mymz4HTc0OKB3nf8hcZBAFsh+TGrab4IIQozWZUADfjIawz2YhMiCqHG74dsgZg2kUS+27hgo=" },{ "IntranetExtranetBD_S3K", "USEEdpleuA/cqg+CCE0r81dg39JbcSU2Wqi5u0rMMUFG64diWGlnzv2XkG8Y5vRBHbYBxw5t/TavJD19s796p4Xxqaq2A78jCTxi3iD2FZ8wUKZB/XR212VrZeGigmzzxY+yFIQNnSsbwDOvCMjDvGSF2wIDTDzk9X81YX4yieU=" },{ "msdb", "bMz9e52j8slOO8N6bqiKt52o9BaNkjZnV+BHB2fzzhaRcYyYC+xFxyQw4cPQDRCY9EIiiPmfAdaYLS0K+vBNM1CPM3Xysfz5IvjDc9s8bDDEkuEXWADL4YJw/RWatiBVgJBroSt4KuS5UauhQ9hnXiQ+3uK3/xUuP4EC0MlkVV8=" },{ "bd_s3k_playertracking", "YAMcVGcO8MEs5RCWfR0IUQRkZ2ILC2+9TxGwC6BrW4rTmy0aUyHmki902J1YniTgHFUYABrGXflhOLyblg9B/2bcS2ZIsGBXrJRABcXklAdu69rfY6Ar7Ue6mf+ru1RjgBY7GkcyLs07Zmgq1jHWVJzrab1jd2AbHuH1yJkx0tM=" },{ "BD_SEGURIDAD_PJ", "Y0mqQhEGwZp6hFdHBIBuGJ5czqG0HX0Vx90yKQ4cZpoVTBvzOJPiQzB1iEhJbKzoxMLMpmn1wevFVNd/yuZh73dsdk7uFZXHd0pldsmi8IJpWffCFa9OVhgUdboWv/vtvI8gPiFDDgTHCtb4I0pkxU+UN8j2fi4NDeBnAGXuOxs=" },{ "DEFAULT", "JvnVtF9TqejszLnxPiXOaRHcz6VJPUylrTA3rRAPNcWEidgzFYsgHClqCvtcWOuxIlE8+yCGN4UPyZkV0xH8rSDHYEKVD30cKweEKWKmlFQI8Q1TTVN/LhhhADPEEIPzK7WpmzajVlvmLwD0SOwAMm4OUu0kyfp1fvYnBtrFSLM=" }
                            };

                            if (cadenas.ContainsKey(databaseName.ToUpper()))
                            {
                                return cadenas[databaseName.ToUpper()];
                            }
                            else
                            {
                                return cadenas["DEFAULT"];
                            }
                        }
                        string ObtenerCadena2()
                        {
                           return string.Empty;
                        }
                    }
                }
            