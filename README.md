- [GettingStarted (asp.net core)](#gettingstarted-aspnet-core)
- [Introduction](#introduction)
- [Code-first approach](#code-first-approach)
- [Schema-first](#schema-first)
- [From template](#from-template)
- [asp.net core example](#aspnet-core-example)
  - [get authors](#get-authors)
  - [get authors with pagination](#get-authors-with-pagination)
  - [get authors with filtering](#get-authors-with-filtering)
  - [children nodes (relations)](#children-nodes-relations)
  - [mutations](#mutations)
  - [data loaders](#data-loaders)
    - [class data loader](#class-data-loader)
    - [Delegate data loaders](#delegate-data-loaders)
      - [Batch data loader](#batch-data-loader)
      - [Group data loader](#group-data-loader)
      - [Cache data loader](#cache-data-loader)
      - [Fetch Once](#fetch-once)
  - [tests](#tests)
    - [book query test](#book-query-test)
    - [book mutation test](#book-mutation-test)
    - [schema tests](#schema-tests)
  - [subscriptions](#subscriptions)
  - [authentication and authorization](#authentication-and-authorization)
    - [authentication](#authentication)
      - [basic authentication](#basic-authentication)
      - [JWT based authentication](#jwt-based-authentication)
    - [authorization](#authorization)
      - [Policies](#policies)
    - [GlobalStateAttribute and HotChocolate interceptor](#globalstateattribute-and-hotchocolate-interceptor)
- [links](#links)

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

[unit tests](https://www.blexin.com/en-US/Article/Blog/Testing-our-API-with-GraphQL-and-Hot-Chocolate-89)   

To run open: http://localhost:37926/playground/   

NOTE: depends on compiled version authentication is required. More can be found in [authentication and authorization](#authentication-and-authorization) chapter.

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

## mutations

First create [mutation](./aspnetcore/aspnetcore/GraphQL/Mutations/Mutation.cs) and next [register](./aspnetcore/aspnetcore/Startup.cs) it.

<details>
<summary>create book - request</summary>
<p>

```js
mutation {
    createBook(inputBook: {
        title: "my super book",
        authorId: 1,
        price: 12.5
        })
    {
        id
        title
    }
}
```

</p>
</details>

<details>
<summary>create book - response</summary>
<p>

```json
{
  "data": {
    "createBook": {
      "id": "5",
      "title": "my super book"
    }
  }
}
```

</p>
</details>

<details>
<summary>delete book - request</summary>
<p>


```js
mutation {
    deleteBook(inputBook: {
        id: 5
    })
    {
        id
        title
    }
}
```

</p>
</details>

<details>
<summary>delete book - response</summary>
<p>


```json
{
  "data": {
    "deleteBook": {
      "id": "5",
      "title": "my super book"
    }
  }
}
```

</p>
</details>

## data loaders

Although we send one request to graphql server usually it will create multiple round-trips to data sources (usually DB). To solve this problem use [data loaders](https://hotchocolate.io/docs/dataloaders). They also solve [N+1 query problem](https://secure.phabricator.com/book/phabcontrib/article/n_plus_one/).

>NOTE: DataLoader contains a **cache** and holds the resolved instances by default for the duration of your request. For example if in the same HTTP request we ask about the same data twice:
``` a1: authorById(id:1)``` and in another place also ```a1: authorById(id:1)``` the second request will be handled from the cache. **Cache is not supported by resolvers** like [BookResolver](./aspnetcore/aspnetcore/GraphQL/BookResolver.cs).

### class data loader

[Class data loader](./aspnetcore/aspnetcore/GraphQL/DataLoaders/ClassDataLoaders/AuthorDataLoader.cs).

request
```
{
  a1: authorById(id:1)
  {
    id
    name
    surname
    authorBooks {
      title
      price
    }
  }
  a2: authorById(id: 2)
  {
    id
    name
    surname
    authorBooks {
      title
      price
    }
  }
}
```

response
```
{
  "data": {
    "a1": {
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
    "a2": {
      "id": "2",
      "name": "Paolo",
      "surname": "Verdi",
      "authorBooks": [
        {
          "title": "Second Book",
          "price": 11
        }
      ]
    }
  }
}
```

### Delegate data loaders

#### Batch data loader

Batch data loader can be defined as a [class](./aspnetcore/aspnetcore/GraphQL/DataLoaders/DelegateDataLoaders/AuthorBatchDataLoader.cs) or [inline](./aspnetcore/aspnetcore/GraphQL/BookType.cs).

Request that uses batch data loader as class:

```
{
  a1: authorByIdBatch(id:1)
  {
    id
    name
    surname
    authorBooks {
      title
      price
    }
  }
  a2: authorByIdBatch(id: 2)
  {
    id
    name
    surname
    authorBooks {
      title
      price
    }
  }
}
```

Request that uses batch data loader as inline implementation:
```
{
  books {
    totalCount
    nodes {
      id
      authorId
      title
      price
      authorFromBatch {
        name
        surname
      }
    }
  }
}
```

#### Group data loader

[Inline implementation](./aspnetcore/aspnetcore/GraphQL/QueryType.cs#L23) and [class implementation](./aspnetcore/aspnetcore/GraphQL/DataLoaders/DelegateDataLoaders/AuthorGroupedDataLoader.cs).
> NOTE: inline implementation requires registration in [Startup.cs](./aspnetcore/aspnetcore/Startup.cs#L31).

```
{
  PL: authorByCountry(country: "PL") {
    id
    name
    surname
    country
  }
  USA: authorByCountry(country: "USA") {
    id
    name
    surname
    country
  }
}
```
#### Cache data loader

The cache DataLoader is basically the easiest to implement since there is no batching involved.

[Inline implementation](./aspnetcore/aspnetcore/GraphQL/QueryType.cs#L44) and [class implementation](./aspnetcore/aspnetcore/GraphQL/DataLoaders/DelegateDataLoaders/AuthorCacheDataLoader.cs).
> NOTE: inline implementation requires registration in [Startup.cs](./aspnetcore/aspnetcore/Startup.cs#L31).

```
{
  a1: authorFromCache(authorId: 1) {
    id
    name
    surname
    country
  }
  a2: authorFromCache(authorId: 2) {
    id
    name
    surname
    country
  }
}
```

#### Fetch Once

FetchOnceAsync is not really a DataLoader like described by facebook. It rather uses the infrastructure of our DataLoader to provide an easy way to provide cache heavy resource calls that **shall only be done once per request.**

[Inline implementation](./aspnetcore/aspnetcore/GraphQL/QueryType.cs#L58).

Request:
```
{
  a1: authorFetchOnce(authorId: 1) {
    id
    name
    surname
    country
  }
  a2: authorFetchOnce(authorId: 2) {
    id
    name
    surname
    country
  }
}
```

Response contains the same data twice because it is **Fetch Once** and next it is taken from the cache.
```js
{
  "data": {
    "a1": {
      "id": "1",
      "name": "Fabio",
      "surname": "Rossi",
      "country": "PL"
    },
    "a2": {
      "id": "1",
      "name": "Fabio",
      "surname": "Rossi",
      "country": "PL"
    }
  }
}
```

## tests

### book query test

In tests is used nuget [ApprovalTests](https://github.com/approvals/ApprovalTests.Net). First we have to run the test: it will fail but will generate a file ```BookQueryTest.Test.received.txt```. If file content is correct change the file name to ```BookQueryTest.Test.approved.txt``` and run the test again: this time ```Approvals.Verify(resultJson);``` will compare received data with the file content. If it will be the same the test will pass.

![BookQueryTest.Test.received.txt](./images/ApprovalTestFileGeneration.png)

### book mutation test

If needed in mutation we can use params to pass the values from C# to the actual payload.

```
mutation($title: String, $price: Decimal!, $authorId: Int!)
```

```c#
.AddVariableValue("title", "Test book")
.AddVariableValue("authorId", 4)
.AddVariableValue("price", 50.0)
```

### schema tests

[schema tests](./aspnetcore/aspnetcore.tests/SchemaTest.cs)

## subscriptions
Subscriptions are fired by API ```IEventSender.SendAsync```.
Subscriptions can be split into 2 categories:
* without parameters
* with parameters

```cs
public class Subscription
{
    /// <summary>
    /// Subscription without parameter.
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    public Review OnReview(IEventMessage message)
    {
        return (Review)message.Payload;
    }

    /// <summary>
    /// Subscription with parameter.
    /// Parameter can be used by the client (subscriber) to subscribe e.g. only for reviews for bookId = 5.
    /// </summary>
    /// <param name="bookId"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    public Review OnReviewWithBookId(int bookId, IEventMessage message)
    {
        return (Review)message.Payload;
    }
}
```

Binding between ```EventMessage``` and particular subscription is defined in ```EventDescription``` constructor, for example:

```cs
public class OnReviewMessage: EventMessage
{
    public OnReviewMessage(Review review)
        : base(CreateEventDescription(), review)
    {

    }

    private static EventDescription CreateEventDescription()
    {
        return new EventDescription("onReview");
    }
}
```

To trigger subscription use the following sequence of operations

<details>
<summary>subscribe for onReview (click the play icon in the playground view to start listening)</summary>
<p>

```js
subscription onReviewSubscription {
  onReview {
    commentary
    stars
    bookId
  }
}
```
</p>
</details>

<details>
<summary>subscribe for onReviewWithBookId only for bookId = 5, (click the play icon in the playground view to start listening)</summary>
<p>

```js
subscription onReviewWithBookIdSubscription {
  onReviewWithBookId(bookId: 5) {
    commentary
    stars
    bookId
  }
}
```
</p>
</details>

<details>
<summary>run mutation to create a book and trigger subscriptions</summary>
<p>

```js
mutation {
    createBook(inputBook: {
        title: "my super book!!!",
        authorId: 1,
        price: 12.5
        })
    {
        id
        title
    }
}
```
</p>
</details>

If the created book will have id = 5 both subscriptions will be triggered if not only subscription ```onReview``` will be triggered.

## authentication and authorization

### authentication

GraphQL does not support authentication process. It is recommended to use [ASP.NET Core authentication](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/?view=aspnetcore-3.1) because it relies on ```AuthenticationTicket``` created during authentication process.

>NOTE: there are implemented 2 authentication mechanism **basic** and **JWT**. To enable one of them call one of these functions in [startup.cs](./aspnetcore/aspnetcore/Startup.cs):
```
this.ConfigureBasicAuthentication(services);
this.ConfigureJwtAuthentication(services);
```

#### basic authentication 

Example of [basic authentication](./aspnetcore/aspnetcore/Authentication/BasicAuthenticationHandler.cs).

To send credentials from GraphQL playground use HTTP HEADERS box:
![http-header-box](./images/http-headers-box.png).

Formatted and encoded credentials can be generated for example on this [page](https://www.blitter.se/utils/basic-authentication-header-generator/).

#### JWT based authentication

Article about [JWT based authentication](https://medium.com/@marcinjaniak/graphql-simple-authorization-and-authentication-with-hotchocolate-11-and-asp-net-core-3-162e0a35743d) in ASP.NET Core 3.

To authenticate call dedicated for this mutation:
```
mutation {
  authenticate(
    email: "abc@dev.com", password: "pass123"
  )
}

```
Sample result:
```json
{
  "data": {
    "authenticate": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjgzMWJhMDI0LWUxMWUtNDMxMi04OGE0LWE0ZDIyNWIxNjk3ZCIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWUiOiJhYmNAZGV2LmNvbSIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6WyJNYW5hZ2VyIiwiU2VuaW9yIE1hbmFnZXIiXSwiZXhwIjoxNjA2Mzg5MDMwLCJpc3MiOiJpc3N1ZXIiLCJhdWQiOiJhdWRpZW5jZSJ9.avrZcSHxE0mbDG4ZeBD2l5_ecfOL09gLZpAxsDf6nH0"
  }
}
```
Use [jwt.io](https://jwt.io/) to decode generated token:
![token](./images/jwt-decode.png)

Next GraphQL endpoints which require authorization can be accessed using HTTP HEADERS BOX in playground UI:
![http-headers-box-jwt.png](./images/http-headers-box-jwt.png)

### authorization

Authorization is supported by ```HotChocolate.AspNetCore.Authorization.Authorize``` attribute. It can be also called as a method.

Samples:
```c#
[Authorize]
public class Person
{
    public string Name { get; }

    [Authorize(Roles = new[] { "foo", "bar" })]
    public Address Address { get; }
}
```
```c#
public class PersonType : ObjectType<Person>
{
    protected override Configure(IObjectTypeDescriptor<Person> descriptor)
    {
        descriptor.Authorize(new [] {"foo"});
        descriptor.Field(t => t.Address).Authorize(new [] {"foo", "bar"});
    }
}
```

#### Policies
Policies are also supported by hot chocolate authorization.   
Create policy:
```c#
        private void AddPolicies(IServiceCollection services)
        {
            services.AddAuthorization(a =>
            {
                a.AddPolicy("super-boss-policy", builder =>
                    builder
                        .RequireAuthenticatedUser()
                        .RequireRole("Managers")
                        .RequireRole("Senior Managers")
                        );

            });
        }
```
Check policy:
```c#
  [Authorize(Policy = "super-boss-policy")]
  public Task<Author> GetAuthorByIdAsync(
          int id,
          AuthorDataLoader dataLoader,
          CancellationToken cancellationToken) => dataLoader.LoadAsync(id, cancellationToken);
```

### GlobalStateAttribute and HotChocolate interceptor

Sometimes there is a need to execute some logic depends on user permissions. For example **if else** block. To make it possible we can use ```HotChocolate.GlobalStateAttribute``` that can create an instance of a type that will represent data that we need access to. Next we have to register in DI ```HotChocolate.AspNetCore.Interceptors.OnCreateRequestAsync```.

[CurrentUser](./aspnetcore/aspnetcore/Authentication/CurrentUser.cs)

Interceptor:
```cs
private static OnCreateRequestAsync AuthenticationInterceptor()
{
    return (context, builder, cancelationToken) =>
    {
        if (context.GetUser().Identity.IsAuthenticated)
        {
            builder.SetProperty("currentUser",
                new CurrentUser(context.User.FindFirstValue(ClaimTypes.NameIdentifier),
                    context.User.Claims.Select(x => new Tuple<string, string>(x.Type, x.Value)).ToList()));
            
        }

        return Task.CompletedTask;
    };
}
```
Finally we can use it in selected GraphQL endpoints:
```cs
[Authorize(Roles = new[] { "Managers" })]
public Task<Author[]> GetAuthorByCountry(
    string country,
    AuthorGroupedDataLoader dataLoader,
    CancellationToken cancellationToken,
    [CurrentUserGlobalState] CurrentUser user)
{
    // We could pass user instance to other functions to execute logic based on the claims
    foreach(var claim in user.Claims)
    {
        Debug.WriteLine($"Type: {claim.Item1}, Value: {claim.Item2}");
    }

    return dataLoader.LoadAsync(country, cancellationToken);
}
```


# links
https://hotchocolate.io/docs/tutorial-mongo   
https://hotchocolate.io/docs/aspnet
https://medium.com/@__xuorig__/the-graphql-dataloader-pattern-visualized-3064a00f319f   
https://secure.phabricator.com/book/phabcontrib/article/n_plus_one/   
https://dev.to/michaelstaib/get-started-with-hot-chocolate-and-entity-framework-e9i   
https://corstianboerman.com/posts/2019-02-12/generic-data-loaders-for-entity-framework-in-graphql.html  
