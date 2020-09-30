using Microsoft.Spark.Sql;
using System;
using System.Collections.Generic;

namespace GettingData
{
    public class Program
    {

        static void Main(string[] args)
        {
            var sparkSession = SparkSession.Builder().GetOrCreate();

            var options = new Dictionary<string, string>
            {
                {"delimiter", "|"},
                {"samplingRation", "1.0" }//,
                //{"inferSchema", "true" }
            };

            var schemaString = "firstName string, lastName string, sexe string";

            var csvFileSource = sparkSession.Read()
                .Format("csv")
                .Options(options)
                .Schema(schemaString)
                .Load("C:/Users/maxde/source/repos/DockerSparkPOC/GettingData/FileSpark/inventory-source.dat");
            //.Load("/src/GettingData/GettingData/FileSpark/inventory-source.dat");

            var csvFileTarget = sparkSession.Read()
                .Format("csv")
                .Options(options)
                .Schema(schemaString)
                .Load("C:/Users/maxde/source/repos/DockerSparkPOC/GettingData/FileSpark/inventory-target.dat");

            //csvFileSource.PrintSchema();
            //csvFileTarget.PrintSchema();

            //csvFileSource.Show(5);
            //csvFileTarget.Show(5);

            //csvFileSource.Select("invItemId").Where("invItemId > 2124").Show();

            //csvFileSource.GroupBy("invItemId").Count()
            //    .WithColumnRenamed("count", "total")
            //    .Filter("invItemId >= 2124")
            //    .Show();

            //Spark with SQL Queries
            csvFileSource.CreateOrReplaceTempView("csvFileSourceTemp");
            csvFileTarget.CreateOrReplaceTempView("csvFileTargetTemp");
            //sparkSession.Sql("select invItemId from csvFileSourceTemp").Show();
            sparkSession.Sql("SELECT csvFileSourceTemp.firstName, csvFileSourceTemp.lastName, csvFileSourceTemp.sexe FROM csvFileSourceTemp LEFT JOIN csvFileTargetTemp ON csvFileSourceTemp.firstName = csvFileTargetTemp.firstName WHERE csvFileTargetTemp.firstName IS NULL").Show();

            sparkSession.Sql("SELECT csvFileTargetTemp.firstName, csvFileTargetTemp.lastName, csvFileTargetTemp.sexe FROM csvFileSourceTemp RIGHT JOIN csvFileTargetTemp ON csvFileSourceTemp.firstName = csvFileTargetTemp.firstName WHERE csvFileSourceTemp.firstName IS NULL").Show();


            //RIGHT JOIN
            //SELECT*
            //FROM A
            //RIGHT JOIN B ON A.key = B.key
            //WHERE B.key IS NULL

            //Outliner Join
            //SELECT*
            //FROM csvFileSourceTemp
            //FULL JOIN csvFileTargetTemp ON csvFileSourceTemp.invItemId = csvFileTargetTemp.invItemId
            //WHERE csvFileSourceTemp.invItemId IS NULL
            //OR csvFileTargetTemp.invItemId IS NULL



        }
    }
}
