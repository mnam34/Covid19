using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Import
{
    public class DefaultFileParser : IFileParser
    {
        //private static int _i = 1;
        private readonly int _chunkRowLimit;
        private readonly string _connectionString;

        public DefaultFileParser()
        {
            //_chunkRowLimit = 3000;//TODO:configurable
            _chunkRowLimit = 1000;//TODO:configurable
            //TODO:read from config file
            _connectionString = "";//ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        }

        IEnumerable<DataTable> IFileParser.GetFileData(string sourceFileFullName)
        {
            bool firstLineOfChunk = true;
            int chunkRowCount = 0;
            DataTable chunkDataTable = null;
            string columnData = null;
            bool firstLineOfFile = true;
            using (var sr = new StreamReader(sourceFileFullName))
            {
                string line = null;
                //Read and display lines from the file until the end of the file is reached.                
                while ((line = sr.ReadLine()) != null)
                {
                    //when reach first line it is column list need to create datatable based on that.
                    if (firstLineOfFile)
                    {
                        columnData = line;
                        firstLineOfFile = false;
                        continue;
                    }
                    if (firstLineOfChunk)
                    {
                        firstLineOfChunk = false;
                        chunkDataTable = CreateEmptyDataTable(columnData);
                    }
                    AddRow(chunkDataTable, line);
                    chunkRowCount++;

                    if (chunkRowCount == _chunkRowLimit)
                    {
                        firstLineOfChunk = true;
                        chunkRowCount = 0;
                        yield return chunkDataTable;
                        chunkDataTable = null;
                    }
                }
            }
            //return last set of data which less then chunk size
            if (null != chunkDataTable)
                yield return chunkDataTable;
        }

        private DataTable CreateEmptyDataTable(string firstLine)
        {
            IList<string> columnList = Split(firstLine);
            var dataTable = new DataTable("Data");
            dataTable.Columns.AddRange(columnList.Select(v => new DataColumn(v)).ToArray());
            return dataTable;
        }

        private void AddRow(DataTable dataTable, string line)
        {
            DataRow newRow = dataTable.NewRow();

            IList<string> fieldData = Split(line);
            var c1 = dataTable.Columns.Count;
            var c2 = fieldData.Count();
            if (c2 == c1)
            {
                for (int columnIndex = 0; columnIndex < dataTable.Columns.Count; columnIndex++)
                {
                    newRow[columnIndex] = fieldData[columnIndex];
                }
                dataTable.Rows.Add(newRow);
            }

            //var values = line.Split('\t');
            //dataTable.Rows.Add(values);
        }

        private IList<string> Split(string input)
        {
            //our csv file will be tab delimited
            var dataList = new List<string>();
            foreach (string column in input.Split('\t'))
            {
                dataList.Add(column);
            }
            return dataList;
        }

        void IFileParser.WriteChunkData(DataTable table, string distinationTable, IList<KeyValuePair<string, string>> mapList)
        {
            using (var bulkCopy = new SqlBulkCopy(_connectionString, SqlBulkCopyOptions.Default))
            {
                bulkCopy.BulkCopyTimeout = 0;//unlimited
                bulkCopy.DestinationTableName = distinationTable;
                foreach (KeyValuePair<string, string> map in mapList)
                {
                    bulkCopy.ColumnMappings.Add(map.Key, map.Value);
                }
                bulkCopy.WriteToServer(table, DataRowState.Added);
            }
        }
    }
}
