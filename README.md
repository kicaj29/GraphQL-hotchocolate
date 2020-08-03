# GettingStarted (asp.net core)
Example based on https://hotchocolate.io/docs/tutorial-01-gettingstarted   

Run the service and open site http://localhost:14671/playground/ (port might be different).
.

To call a query that has parameter execute

```
{
    helloWithParam(name: "Jacek")
}

```

# Introduction
https://hotchocolate.io/docs/introduction

# Code-first approach

[code-first](./Introduction/Introduction/CodeFirst/CodeFirstRun.cs)

# Schema-first

[schema-first](./Introduction/Introduction/SchemaFirst/SchemaFirstRun.cs)

# From template

[how-to-use-template](https://github.com/ChilliCream/hotchocolate#templates)

To install the GraphQL server template, run the following command:
```
dotnet new -i HotChocolate.Templates.Server
```

Now that you have implemented this you can generate a new server project by running the following commands.   

```
mkdir myserver
cd myserver
dotnet new graphql
```

# asp.net core example

example based on this [article](https://www.blexin.com/en-US/Article/Blog/Creating-our-API-with-GraphQL-and-Hot-Chocolate-79)


To get authors
```
{
	  authors {
        nodes {
            id
            name
            surname
        }
    }
}
```

# links
https://hotchocolate.io/docs/tutorial-mongo   
https://hotchocolate.io/docs/aspnet