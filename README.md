.NET Core and PGSQL Demo
-------------------------

This project is to demonstrate the use of PG SQL with .NET Core and C#.

NPGSQL (https://www.npgsql.org) is used to interact with the PGSQL server, running on the local host.

The scripts used to create the DB, the tables and the stored procedures is available as part of the SQLCommands.txt


The PatientDBTester XUnit test project was added thus:
1. Add a new XUnit project to the solution from the Add option
2. Add a reference to the project to be tested. Here, it is PGSqlAccess
3. In the XUnit test case file, there will be a default test case added. Rename it to the actual test name (like AddPatientsTest, etc.)
4. Add some test code in here. I have added a code to add some patients and check if this works. Use the Assert.True, Assert.False, Assert.Equal, etc. to conclude if the test passed or failed.
5. Add multiple such tests.
6. You can see these tests in the Test Explorer. If not, build the solution once. It should be visible now.
7. Click on the Run All option in the Test Explorer. This shall run the tests and report the results.




