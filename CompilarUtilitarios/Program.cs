using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CompilarUtilitarios
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string projectDllDirectory = "E:\\Tutos\\ASP\\CompilarUtilitarios\\CompilarUtilitarios\\CapaUtilitarios";//directorio donde se encuentra en .csproj
            //string projectUtilityDirectory = "E:\\Tutos\\ASP\\CompilarUtilitarios\\CompilarUtilitarios\\CompilarUtilitarios";
            string jsonDirectory = "E:\\Tutos\\ASP\\CompilarUtilitarios\\CompilarUtilitarios\\CompilarUtilitarios";//directorio donde se encuentra cadenas.json
            ClearDirectories(projectDllDirectory);
            CreateClass(projectDllDirectory,jsonDirectory);
            int exitCode=Compile(projectDllDirectory);
            Console.WriteLine($"Proceso finalizado con código de salida: {exitCode}");
            Console.ReadKey();
        }
        static void ClearDirectories(string projectDllDirectory)
        {
            if (System.IO.File.Exists(Path.Combine(projectDllDirectory, "ConnectionHelper.cs")))
            {
                System.IO.File.Delete(Path.Combine(projectDllDirectory, "ConnectionHelper.cs"));
            }
            if (System.IO.Directory.Exists(Path.Combine(projectDllDirectory, "bin")))
            {
                System.IO.Directory.Delete(Path.Combine(projectDllDirectory, "bin"), true);
            }
            if (System.IO.Directory.Exists(Path.Combine(projectDllDirectory, "obj")))
            {
                System.IO.Directory.Delete(Path.Combine(projectDllDirectory, "obj"), true);
            }
        }
        static void CreateClass(string projectDllDirectory,string jsonDirectory)
        {
            string uriClass = Path.Combine(projectDllDirectory, "ConnectionHelper.cs");
            using (FileStream fs = File.Create(uriClass))
            {
                byte[] info = new UTF8Encoding(true).GetBytes(GetClassString(jsonDirectory));
                fs.Write(info,0,info.Length);
            }
        }
        static string GetClassString(string jsonDirectory)
        {
            string connectionsString = GetConnectionStrings(jsonDirectory);
            return $@"
                using System;
                using System.Collections.Generic;
                using System.Linq;
                using System.Text;
                using System.Threading.Tasks;

                namespace CapaUtilitarios
                {{
                    public class ConnectionHelper
                    {{
                        public string GetConnectionString(string database)
                        {{
                            return ObtenerCadena(database);
                        }}
                        string ObtenerCadena(string databaseName)
                        {{
                            Dictionary<string, string> cadenas = new Dictionary<string, string>()
                            {{
                                {connectionsString}
                            }};

                            if (cadenas.ContainsKey(databaseName.ToUpper()))
                            {{
                                return cadenas[databaseName.ToUpper()];
                            }}
                            else
                            {{
                                return cadenas[""DEFAULT""];
                            }}
                        }}
                        string ObtenerCadena2()
                        {{
                           return string.Empty;
                        }}
                    }}
                }}
            ";
        }
        static string GetConnectionStrings(string jsonDirectory)
        {
            string publicKey = "<RSAKeyValue><Modulus>swYmlphRWfs5jXaTlrxlcdPKeg2wfOqpHbLIbt/8c+b/QRL/OW6T+iTtQP1GlhSJeEkjKjrhxrZxiyHt0K8DD7e6q6FL/sXhQGZvan4ND3GfoEQ6xndE5XWvrwUY067IrhklRVmfUbWBx8SMJ0I3KlKbO1l1EHAeLuTe8A9Asr0=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";

            List<string> listConnections = new List<string>();
            // Buscar la ruta raíz del proyecto ascendiendo en el árbol de directorios
            string json = string.Empty;
            using (StreamReader jsonStream = File.OpenText(Path.Combine(jsonDirectory, "cadenas.json")))
            {
                json = jsonStream.ReadToEnd();
            }
            List<Dictionary<string, string>> connectionStrings = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(json);

            foreach (var connectionString in connectionStrings)
            {
                foreach (var kvp in connectionString)
                {
                    byte[] originalBytes = Encoding.UTF8.GetBytes(kvp.Value);
                    // Encriptar usando la llave pública
                    byte[] encryptedBytes = EncryptWithPublicKey(originalBytes, publicKey);
                    string encriptedMessage = Convert.ToBase64String(encryptedBytes);

                    listConnections.Add($"{{ \"{kvp.Key}\", \"{encriptedMessage}\" }}");
                }
            }
            return String.Join(",", listConnections);
        }
        static byte[] EncryptWithPublicKey(byte[] dataToEncrypt, string publicKey)
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(publicKey);
                return rsa.Encrypt(dataToEncrypt, false);
            }
        }
        static int Compile(string projectDllDirectory)
        {
            Process process = new Process();
            process.StartInfo.FileName = "dotnet";
            process.StartInfo.Arguments = $"build {projectDllDirectory}";
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;

            // Manejar la salida y los errores
            process.OutputDataReceived += (sender, e) => Console.WriteLine(e.Data);
            process.ErrorDataReceived += (sender, e) => Console.WriteLine(e.Data);

            // Iniciar el proceso
            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            // Esperar a que termine el proceso
            process.WaitForExit();

            // Imprimir el código de salida
            int exitCode = process.ExitCode;
            return exitCode;
          
        }
    }

}
