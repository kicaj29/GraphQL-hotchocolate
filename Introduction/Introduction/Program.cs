using System;
using Introduction.CodeFirst;
using Introduction.SchemaFirst;

namespace Introduction
{
    class Program
    {
        static void Main(string[] args)
        {
            CodeFirstRun.RunPureCodeFirst();
            CodeFirstRun.RunCodeFirstBySchemaBuilder();
            CodeFirstRun.RunCodeFirstByObjectType();

            SchemaFirstRun.RunWithResolver();
            SchemaFirstRun.RunWithBindComplexType();
            SchemaFirstRun.RunWithBindComplexTypeSpecifyField();

            Console.ReadKey();
        }


    }
}
