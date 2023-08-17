using Microsoft.CSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CompilarUtilitarios
{
    internal class Program
    {
        static void Main(string[] args)
        {
            GetConnections();
            // Ruta al directorio del proyecto (donde se encuentra el archivo .csproj)
            //string projectDirectory = @"E:\Tutos\ASP\CapaUtilitarios";
            // Obtener la ruta del directorio actual (donde se ejecuta la aplicación)
            string currentDirectory = Directory.GetCurrentDirectory();
            string solutionDirectory = GetSolutionDirectory();
            Console.WriteLine(solutionDirectory);

            // Buscar la ruta raíz del proyecto ascendiendo en el árbol de directorios
            string projectRoot = FindProjectRoot(currentDirectory);

            Console.WriteLine("Ruta raíz del proyecto: " + projectRoot);
            string projectDirectory = Path.Combine(solutionDirectory, "CapaUtilitarios");
            //Borar archivo cs
            if (System.IO.File.Exists(Path.Combine(projectDirectory, "ConnectionHelper.cs")))
            {
                System.IO.File.Delete(Path.Combine(projectDirectory, "ConnectionHelper.cs"));
            }
            if (System.IO.Directory.Exists(Path.Combine(projectDirectory, "bin")))
            {
                System.IO.Directory.Delete(Path.Combine(projectDirectory, "bin"),true);
            }
            if (System.IO.Directory.Exists(Path.Combine(projectDirectory, "obj")))
            {
                System.IO.Directory.Delete(Path.Combine(projectDirectory, "obj"),true);
            }

            using (FileStream fs = File.Create(Path.Combine(projectDirectory, "ConnectionHelper.cs")))
            {
                byte[] info = new UTF8Encoding(true).GetBytes(GetStringConnectionHelper());
                fs.Write(info, 0, info.Length);
            }
            //string projectDirectory = Path.Combine(projectRoot, "CapaUtilitarios");
            // Crear el proceso para ejecutar el comando "dotnet build"
            Process process = new Process();
            process.StartInfo.FileName = "dotnet";
            process.StartInfo.Arguments = $"build {projectDirectory}";
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
            Console.WriteLine($"Proceso finalizado con código de salida: {exitCode}");
            Console.ReadKey();
        }
        static string FindProjectRoot(string directory)
        {
            while (!File.Exists(Path.Combine(directory, "CompilarUtilitarios.csproj"))) // Reemplaza "yourproject.csproj" con el nombre de tu archivo de proyecto
            {
                string parentDirectory = Directory.GetParent(directory)?.FullName;

                // Si llegamos al directorio raíz sin encontrar el archivo de proyecto, detenemos la búsqueda
                if (parentDirectory == null || parentDirectory == directory)
                    throw new InvalidOperationException("No se pudo encontrar la ruta raíz del proyecto.");

                directory = parentDirectory;
            }

            return directory;
        }
        static string GetStringConnectionHelper()
        {
            string connections = GetConnections();
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
                                {connections}
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
        static string GetConnections()
        {
            string publicKey = "<RSAKeyValue><Modulus>swYmlphRWfs5jXaTlrxlcdPKeg2wfOqpHbLIbt/8c+b/QRL/OW6T+iTtQP1GlhSJeEkjKjrhxrZxiyHt0K8DD7e6q6FL/sXhQGZvan4ND3GfoEQ6xndE5XWvrwUY067IrhklRVmfUbWBx8SMJ0I3KlKbO1l1EHAeLuTe8A9Asr0=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";

            string currentDirectory = Directory.GetCurrentDirectory();

            string dictionaryBody = string.Empty;
            List<string> listConnections= new List<string>();
            // Buscar la ruta raíz del proyecto ascendiendo en el árbol de directorios
            string projectRoot = FindProjectRoot(currentDirectory);
            string json = string.Empty;
            using (StreamReader jsonStream = File.OpenText(Path.Combine(projectRoot, "cadenas.json")))
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
                    string encriptedMessage=Convert.ToBase64String(encryptedBytes);

                    listConnections.Add($"{{ \"{kvp.Key}\", \"{encriptedMessage}\" }}");
                }
            }
            return String.Join(",",listConnections);
        }
        static string GetSolutionDirectory()
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            string solutionDirectory = currentDirectory;

            while (!File.Exists(Path.Combine(solutionDirectory, "CompilarUtilitarios.sln")))
            {
                solutionDirectory = Directory.GetParent(solutionDirectory).FullName;

                // Comprobación para evitar llegar al directorio raíz del sistema de archivos
                if (solutionDirectory == Directory.GetDirectoryRoot(solutionDirectory))
                {
                    throw new Exception("No se pudo encontrar el directorio raíz de la solución.");
                }
            }

            return solutionDirectory;
        }
        static byte[] EncryptWithPublicKey(byte[] dataToEncrypt, string publicKey)
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(publicKey);
                return rsa.Encrypt(dataToEncrypt, false);
            }
        }
    }
}
