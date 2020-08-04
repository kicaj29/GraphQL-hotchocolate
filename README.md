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

To run open: http://localhost:37926/playground/   


## get authors
```
{
    authors {
        id
        name
        surname
    }
}
```

```
{
  "data": {
    "authors": [
      {
        "id": "1",
        "name": "Fabio",
        "surname": "Rossi"
      },
      {
        "id": "2",
        "name": "Paolo",
        "surname": "Verdi"
      },
      {
        "id": "3",
        "name": "Carlo",
        "surname": "Bianchi"
      }
    ]
  }
}
```

## get authors with pagination

Use attribute in C# ```[UsePaging(SchemaType = typeof(AuthorType))]```

<details>
<summary>get all authors - request</summary>
<p>

```js
{
	authors {
    	pageInfo {
            endCursor
            hasNextPage
            hasPreviousPage
            startCursor
        }
        edges {
            cursor
            node {
                id
                name
                surname
            }
        }
        totalCount
      	nodes {
            id
            name
        	surname 
        }
    }
}
```

</p>
</details>


<details>
<summary>get all authors - response</summary>
<p>

```json
{
  "data": {
    "authors": {
      "pageInfo": {
        "endCursor": "Mw==",
        "hasNextPage": false,
        "hasPreviousPage": false,
        "startCursor": "MA=="
      },
      "edges": [
        {
          "cursor": "MA==",
          "node": {
            "id": "1",
            "name": "Fabio",
            "surname": "Rossi"
          }
        },
        {
          "cursor": "MQ==",
          "node": {
            "id": "2",
            "name": "Paolo",
            "surname": "Verdi"
          }
        },
        {
          "cursor": "Mg==",
          "node": {
            "id": "3",
            "name": "Carlo",
            "surname": "Bianchi"
          }
        },
        {
          "cursor": "Mw==",
          "node": {
            "id": "4",
            "name": "Adam",
            "surname": "Bonec"
          }
        }
      ],
      "totalCount": 4,
      "nodes": [
        {
          "id": "1",
          "name": "Fabio",
          "surname": "Rossi"
        },
        {
          "id": "2",
          "name": "Paolo",
          "surname": "Verdi"
        },
        {
          "id": "3",
          "name": "Carlo",
          "surname": "Bianchi"
        },
        {
          "id": "4",
          "name": "Adam",
          "surname": "Bonec"
        }
      ]
    }
  }
}
```

</p>
</details>


<details>
<summary>get last 2 authors before Carlo Bianchi - request</summary>
<p>

```js
{
	authors (before: "Mg==", last: 2) {
        totalCount
        nodes {
            id
            name
            surname 
        }
    }
}
```

</p>
</details>

<details>
<summary>get last 2 authors before Carlo Bianchi - response</summary>
<p>

```json
{
  "data": {
    "authors": {
      "totalCount": 4,
      "nodes": [
        {
          "id": "1",
          "name": "Fabio",
          "surname": "Rossi"
        },
        {
          "id": "2",
          "name": "Paolo",
          "surname": "Verdi"
        }
      ]
    }
  }
}
```

</p>
</details>

<details>
<summary>get first 2 authors after Paolo Verdi - request</summary>
<p>

```js
{
    authors (after: "MQ==", first: 2) {
        totalCount
        nodes {
            id
            name
            surname 
        }
    }
}
```

</p>
</details>

<details>
<summary>get first 2 authors after Paolo Verdi - response</summary>
<p>

```json
{
  "data": {
    "authors": {
      "totalCount": 4,
      "nodes": [
        {
          "id": "3",
          "name": "Carlo",
          "surname": "Bianchi"
        },
        {
          "id": "4",
          "name": "Adam",
          "surname": "Bonec"
        }
      ]
    }
  }
}
```

</p>
</details>

## get authors with filtering

Use attribute in C# ```[UseFiltering]```

<details>
<summary>filters authors - request</summary>
<p>

```js
{
    authors (
    where: { OR: [{name_contains: "arl"}, {surname_contains: "one"}]}
    first: 1
    )
    {
        pageInfo {
            endCursor
            hasNextPage
            hasPreviousPage
            startCursor
        }
        totalCount
        nodes {
            id
            name
            surname 
        }
    }
}
```

</p>
</details>

<details>
<summary>filters authors - response</summary>
<p>

First is executed filtering and next paging

```json
{
  "data": {
    "authors": {
      "pageInfo": {
        "endCursor": "MA==",
        "hasNextPage": true,
        "hasPreviousPage": false,
        "startCursor": "MA=="
      },
      "totalCount": 2,
      "nodes": [
        {
          "id": "3",
          "name": "Carlo",
          "surname": "Bianchi"
        }
      ]
    }
  }
}
```

</p>
</details>

## children nodes (relations)

To create relations first create [resolver](./aspnetcore/aspnetcore/GraphQL/BookResolver.cs) and next use it in [object type](./aspnetcore/aspnetcore/GraphQL/AuthorType%20.cs) class.


<details>
<summary>authors with their books - request</summary>
<p>

>NOTE: name of children collection ```authorBooks``` is the same as second part of the method name from the resolver ```GetAuthorBooks```.

```js
{
    authors {
        totalCount
        nodes {
            id
            name
            surname
            authorBooks {
                title
                price
            }
        }
    }
}
```

</p>
</details>

<details>
<summary>authors with their books - response</summary>
<p>

```json
{
  "data": {
    "authors": {
      "totalCount": 4,
      "nodes": [
        {
          "id": "1",
          "name": "Fabio",
          "surname": "Rossi",
          "authorBooks": [
            {
              "title": "First Book",
              "price": 10
            },
            {
              "title": "Fourth Book",
              "price": 15
            }
          ]
        },
        {
          "id": "2",
          "name": "Paolo",
          "surname": "Verdi",
          "authorBooks": [
            {
              "title": "Second Book",
              "price": 11
            }
          ]
        },
        {
          "id": "3",
          "name": "Carlo",
          "surname": "Bianchi",
          "authorBooks": [
            {
              "title": "Third Book",
              "price": 12
            }
          ]
        },
        {
          "id": "4",
          "name": "Adam",
          "surname": "Bonec",
          "authorBooks": []
        }
      ]
    }
  }
}
```

</p>
</details>

# links
https://hotchocolate.io/docs/tutorial-mongo   
https://hotchocolate.io/docs/aspnet