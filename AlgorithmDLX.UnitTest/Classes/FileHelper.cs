using AlgorithmDLX.UnitTest.Classes.Converters;
using Newtonsoft.Json;
using System.Reflection;

namespace AlgorithmDLX.UnitTest.Classes;

/// <summary>
/// Tools for interacting with unit test files
/// </summary>
public static class FileHelper
{
    public static JsonSerializerSettings JsonSerializerSettings = new()
    {
        Converters = new List<JsonConverter> { new SingleLineArrayConverter() },
        Formatting = Formatting.Indented
    };

    /// <summary>
    /// Read object from JSON file
    /// </summary>
    /// <typeparam name="T">Object type</typeparam>
    /// <param name="filePath">File path</param>
    /// <returns><see cref="T"/> object</returns>
    /// <exception cref="FileNotFoundException"></exception>
    public static T ReadFromJsonFile<T>(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException("File not found.", filePath);
        }

        string json = File.ReadAllText(filePath);
        T? result = JsonConvert.DeserializeObject<T>(json);
        return result == null ? throw new InvalidOperationException("Deserialization resulted in null.") : result;
    }


    /// <summary>
    /// Write object to JSON file
    /// </summary>
    /// <typeparam name="T">Object type</typeparam>
    /// <param name="filePath">File path</param>
    /// <param name="data"><see cref="T"/> object</param>
    public static void WriteToJsonFile<T>(string filePath, T data)
    {
        string json = JsonConvert.SerializeObject(data, JsonSerializerSettings);
        File.WriteAllText(filePath, json);
    }

    /// <summary>
    /// Read object from sepcificed JSON file path
    /// </summary>
    /// <typeparam name="T">Object type</typeparam>
    /// <param name="fileName">File name</param>
    /// <param name="baseDirectory">Determines if it is expected result or input file</param>
    /// <param name="additionalDirectoryPath">Additional path to nested file</param>
    /// <returns></returns>
    public static T ReadJsonTestFiles<T>(string fileName, string baseDirectory, params string[] additionalDirectoryPath)
    {
        string filePath = Path.Combine(GetTestFilesPath(baseDirectory, additionalDirectoryPath), fileName);
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException("File not found.", filePath);
        }
        return ReadFromJsonFile<T>(filePath);
    }

    /// <summary>
    /// Write object to specificed JSON file path
    /// </summary>
    /// <typeparam name="T">Object type</typeparam>
    /// <param name="fileName">File name</param>
    /// <param name="data"><see cref="T"/> object</param>
    /// <param name="baseDirectory">Base directory</param>
    /// <param name="additionalDirectoryPath">Additional path to nested file</param>
    public static void WriteJsonTestFiles<T>(string fileName, T data, string baseDirectory, params string[] additionalDirectoryPath)
    {
        string filePath = Path.Combine(GetTestFilesPath(baseDirectory, additionalDirectoryPath), fileName);
        WriteToJsonFile(filePath, data);
    }

    /// <summary>
    /// Get full path to the file
    /// </summary>
    /// <param name="baseDirectory">Root directory (Expected or Input)</param>
    /// <param name="additionalDirectoryPath">Additional path to nested file</param>
    /// <returns>Full path to the file</returns>
    public static string GetTestFilesPath(string baseDirectory, params string[] additionalDirectoryPath)
    {
        string assemblyPath = new Uri(Assembly.GetExecutingAssembly().Location).LocalPath;
        string assemblyDirectory = Path.GetDirectoryName(assemblyPath) ?? throw new ArgumentNullException(nameof(assemblyPath));

        string filesBasePath = Environment.GetEnvironmentVariable("TEST_FILES_PATH") ?? @"..\..\..\TestFiles";

        string filePath = Path.Combine(assemblyDirectory, filesBasePath, baseDirectory);
        foreach (var path in additionalDirectoryPath)
        {
            if (path != null)
            {
                filePath = Path.Combine(filePath, path);
            }
        }
        return Path.GetFullPath(filePath);
    }
}