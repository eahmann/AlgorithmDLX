using AlgorithmDLX.Core.Models;
using AlgorithmDLX.UnitTest.Classes.Exceptions;
using Newtonsoft.Json;
using System.Text;

namespace AlgorithmDLX.UnitTest.Classes;

public class TestHelper
{
    public class Visualize
    {
        /// <summary>
        /// Diagnostic tool to help identify matrix linking
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        public static string DLXMatrix(Header header)
        {
            StringBuilder sb = new();
            Header currentHeader = (Header)header.Right; // Start from the first column.

            while (currentHeader != null && currentHeader != header) // Ensure we don't loop indefinitely.
            {
                sb.AppendLine(new string('-', 40)); // Visual separator
                sb.AppendLine($"Header (Column {currentHeader.ColumnNumber}): NodeCount = {currentHeader.NodeCount}");

                Node currentRowNode = currentHeader.Down;
                if (currentRowNode == null)
                {
                    sb.AppendLine("Error: No nodes in column.");
                    continue;
                }

                // Iterate down the column.
                while (currentRowNode != currentHeader)
                {
                    sb.AppendFormat("  Row {0}: ", currentRowNode.RowIndex);

                    // Iterate right across the row.
                    Node currentRowElement = currentRowNode;
                    do
                    {
                        if (currentRowElement.ColumnHeader == null)
                        {
                            sb.Append("Error in row element. ");
                            break;
                        }

                        sb.AppendFormat("{0} ", currentRowElement.ColumnHeader.ColumnNumber);
                        currentRowElement = currentRowElement.Right;

                    } while (currentRowElement != currentRowNode);

                    sb.AppendLine(); // End the line for the current row.
                    currentRowNode = currentRowNode.Down;
                }

                currentHeader = (Header)currentHeader.Right; // Move to the next column header.
            }

            sb.AppendLine(new string('-', 40)); // Visual separator at the end
            return sb.ToString();
        }

        public static string DLXMatrixAsGrid(Header header)
        {
            // Calculate max rows and columns
            int maxRows = CalculateMaxRows(header);
            int maxColumns = CalculateMaxColumns(header);

            // Initialize the grid
            string[,] grid = new string[maxRows, maxColumns];
            for (int i = 0; i < maxRows; i++)
                for (int j = 0; j < maxColumns; j++)
                    grid[i, j] = "-"; // Default value for empty cells

            // Populate the grid
            Header currentHeader = (Header)header.Right;
            int columnIndex = 0;
            while (currentHeader != header)
            {
                Node currentRowNode = currentHeader.Down;
                while (currentRowNode != currentHeader)
                {
                    grid[currentRowNode.RowIndex, columnIndex] = $"C{currentHeader.ColumnNumber}";
                    currentRowNode = currentRowNode.Down;
                }
                currentHeader = (Header)currentHeader.Right;
                columnIndex++;
            }

            // Convert the grid to a string for output
            StringBuilder sb = new();
            for (int i = 0; i < maxRows; i++)
            {
                for (int j = 0; j < maxColumns; j++)
                {
                    sb.AppendFormat("{0,3}", grid[i, j]); // Adjust the format for alignment
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }

        private static int CalculateMaxRows(Header header)
        {
            int maxRows = 0;
            Header currentHeader = (Header)header.Right;
            while (currentHeader != header)
            {
                Node currentRowNode = currentHeader.Down;
                while (currentRowNode != currentHeader)
                {
                    maxRows = Math.Max(maxRows, currentRowNode.RowIndex + 1);
                    currentRowNode = currentRowNode.Down;
                }
                currentHeader = (Header)currentHeader.Right;
            }
            return maxRows;
        }

        private static int CalculateMaxColumns(Header header)
        {
            int maxColumns = 0;
            Header currentHeader = (Header)header.Right;
            while (currentHeader != header)
            {
                maxColumns++;
                currentHeader = (Header)currentHeader.Right;
            }
            return maxColumns;
        }
    }

    /// <summary>
    /// Class for validating various data types against JSON files.
    /// </summary>
    public class Validate
    {
        /// <summary>
        /// Validates a JSON file against an object of the same type.
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="fileName"></param>
        /// <param name="objectToValidate"></param>
        /// <param name="writeFile">Flag to indicate if the file should be created and/or updated</param>
        /// <param name="additionalDirectoryPath"></param>
        /// <exception cref="ValidationException"></exception>
        public static void Object<T>(string fileName, T objectToValidate, bool writeFile = false, params string[] additionalDirectoryPath)
        {
            T existingObject;
            string filePath = Path.Combine(FileHelper.GetTestFilesPath(TestFileDirectory.Expected, additionalDirectoryPath), fileName);
            try
            {
                existingObject = FileHelper.ReadJsonTestFiles<T>(fileName, TestFileDirectory.Expected, additionalDirectoryPath);
            }
            catch (FileNotFoundException ex)
            {
                if (writeFile)
                {
                    FileHelper.WriteToJsonFile(filePath, objectToValidate);
                }
                throw new ValidationException($"Validation failed: JSON file not found. {ex.Message}", ex);
            }

            string existingObjectJson = JsonConvert.SerializeObject(existingObject, FileHelper.JsonSerializerSettings);
            string objectToValidateJson = JsonConvert.SerializeObject(objectToValidate, FileHelper.JsonSerializerSettings);

            if (!existingObjectJson.Equals(objectToValidateJson))
            {
                if (writeFile)
                {
                    FileHelper.WriteToJsonFile(filePath, objectToValidate);
                }
                throw new ValidationException("Validation failed: Object does not match JSON file contents.");
            }
        }
    }



    


}
