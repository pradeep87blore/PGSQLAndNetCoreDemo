using System;
using System.ComponentModel;
using PGSqlAccess;

namespace NETCoreAndPGSQLDemo
{
    class Program
    {
        
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            DBTester dbTester = new DBTester();
            dbTester.AddNewPatients(10);
        }

        
    }
}
