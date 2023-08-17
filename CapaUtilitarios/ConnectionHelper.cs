
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
                                { "BD_ONLINE", "DQPzja+7gsncOQNjrQXnvFruvwO898vriZolQ/B3+0QS7KXG2fR508vtvQx5ekip95djZhi1MXA9Kww0d+U4Ru6P0jvL647+8WP3tAQXS3ZVU0sEQGCHaSksfIQ/nNx6PYxtilJz1k9KxmWZPKKWoCDoSQSvDtznM2aFJQImBow=" },{ "BD_TECNOLOGIAS", "fi6fJPb8g8eKfAwmXdE0ycYasnlEc6NBwxjRl6i0rAw6n+IzBrajiRHLnWo944AVmH6q8hu9osKYlUFj/5WTsHERo8bKMvLLEbOloHgkTT+ZpIyp/q7/CDPw5sG6NLF7ynkuJSG79UvJREmxFtOKaPqg0y17Pq04LiuARvBfI1Q=" },{ "BD_SEGURIDAD_PJ", "hRXxG41N8nD5WGZg6Ur/iR4X/ReZcHjjhxNU9iEK2k0t2PcMZP1hZn6kiD6szcdXrWzNYh0OFIrwX+OGaj07Qt1oJDUk+jgdkd5rjdBGLn1tovMLIAUN0pNPhCNV70K5QvniyAXmJSWLLQOJKFr4IKX/lQE1MNxB9ED2/7tAaKM=" },{ "BD_S3K_ADMINISTRATIVO_DATA", "crwVhCIbMNzYsCq2kQbYq+SbiAg2n5bZ1UtxfPovWWXnXSwZ2MbO9HSRHKJ3UMbxvsw8stgIZgWSe4IcI/Zb6Gu/+nbWTPDCpRt92uCqZ49Ug2yxm9zTyk5VeAutgm2CTorbYchL5QeFVpaC8LxHKJyVw4cdbE5vJOhdeXQFw3o=" },{ "DEFAULT", "p2WiPktFg8pW0IeEZCExRDxdTPi/DG49vL2rvsfWlzAYF/zUxaEQHjgIAEapU92k7fT4KCXihX8nWV6ynnaIxf5O3xxTiW3y5dgU92KnAkqM/3FKr9UM+xZQU7J3wuoCe+5yIpXl+UbNiy5IJCVymeMumj52ndai2fPwM+c18cU=" }
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
            