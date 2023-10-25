// See https://aka.ms/new-console-template for more information
using HotChocolate.Language;
using HotChocolate.Utilities.Introspection;

Console.WriteLine("Hello, World!");


HttpClient httpClient = new HttpClient();
httpClient.BaseAddress = new Uri("https://.../graphql");
httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer",
    "eyJh...");
IntrospectionClient introspectionClient = new IntrospectionClient();
DocumentNode schema = await introspectionClient.DownloadSchemaAsync(httpClient);
string schemaString = schema.ToString();
Console.WriteLine(schemaString);
File.WriteAllText("gql-schema.schema", schemaString);



Console.ReadKey();