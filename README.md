# HotChocolate Bug Demo

## Bug description

Type extensions are not working correctly when using `.UsePaging()`, `.UseFirstOrDefault()` or `.UseSingleOrDefault()`.

- `.UsePaging()` on `Query.articles` should transform the output `[Article!]!` into `ArticleConnection`.
- `.UseFirstOrDefault()` on `Query.article` should transform the output `[Article!]!` into `Article`.

For `Query.customer` and `Query.customer` everything works as expetced. But if you swap the type extension registration (`.AddTypeExtension<ArticleQueryTypeExtension>()` and `.AddTypeExtension<CustomerQueryTypeExtension>()` in the `Program.cs` file), you will see, that only the latter one is working correctly.

## Some example queries

Note: The IDs will be different for each start.

```gql
query CM {
  customers(
    where: { isActive: { eq: true }}
  ) {
    nodes {
      id
      name
      isActive
    }
  }
}

query CO {
  customer(
    customerId: "c888bf19-59ea-4b22-9d6a-de49eba3befa"
  ) {
    id
    name
    isActive
  }
}

# This request will not work because of that bug. Swap the type extensions lines in the `Program.cs` file and it will work again but then the `Query.customer` is then broken instead.
query AM {
  articles(
    order: { price: DESC }
  ) {
    id
    name
    price
  }
}

query AO {
  article(
    articleId: "c888bf19-59ea-4b22-9d6a-de49eba3befa"
  ) {
    id
    name
    price
  }
}
```

## Schema

This project will generates the following GraphQL schema.

```gql
"""
The `@defer` directive may be provided for fragment spreads and inline fragments to inform the executor to delay the execution of the current fragment to indicate deprioritization of the current fragment. A query with `@defer` directive will cause the request to potentially return multiple responses, where non-deferred data is delivered in the initial response and data deferred is delivered in a subsequent response. `@include` and `@skip` take precedence over `@defer`.
"""
directive @defer(
  """
  If this argument label has a value other than null, it will be passed on to the result of this defer directive. This label is intended to give client applications a way to identify to which fragment a deferred result belongs to.
  """
  label: String

  """
  Deferred when true.
  """
  if: Boolean
) on FRAGMENT_SPREAD | INLINE_FRAGMENT

"""
The `@stream` directive may be provided for a field of `List` type so that the backend can leverage technology such as asynchronous iterators to provide a partial list in the initial response, and additional list items in subsequent responses. `@include` and `@skip` take precedence over `@stream`.
"""
directive @stream(
  """
  If this argument label has a value other than null, it will be passed on to the result of this stream directive. This label is intended to give client applications a way to identify to which fragment a streamed result belongs to.
  """
  label: String

  """
  The initial elements that shall be send down to the consumer.
  """
  initialCount: Int! = 0

  """
  Streamed when true.
  """
  if: Boolean
) on FIELD

"""
This is the root Query type
"""
type Query {
  """
  Get many articles
  """
  articles(
    """
    Returns the first _n_ elements from the list.
    """
    first: Int

    """
    Returns the elements in the list that come after the specified cursor.
    """
    after: String

    """
    Returns the last _n_ elements from the list.
    """
    last: Int

    """
    Returns the elements in the list that come before the specified cursor.
    """
    before: String
    order: [ArticleSortInput!]
    where: ArticleFilterInput
  ): [Article!]! # <- should be ArticlesConnection

  """
  Get one article
  """
  article(articleId: UUID!): [Article!]! # <- should be Article

  """
  Get many customers
  """
  customers(
    """
    Returns the first _n_ elements from the list.
    """
    first: Int

    """
    Returns the elements in the list that come after the specified cursor.
    """
    after: String

    """
    Returns the last _n_ elements from the list.
    """
    last: Int

    """
    Returns the elements in the list that come before the specified cursor.
    """
    before: String
    order: [CustomerSortInput!]
    where: CustomerFilterInput
  ): CustomersConnection

  """
  Get one customer
  """
  customer(customerId: UUID!): Customer
}

input ArticleSortInput {
  id: SortEnumType
  name: SortEnumType
  price: SortEnumType
}

input ArticleFilterInput {
  and: [ArticleFilterInput!]
  or: [ArticleFilterInput!]
  id: ComparableGuidOperationFilterInput
  name: StringOperationFilterInput
  price: ComparableSingleOperationFilterInput
}

"""
A connection to a list of items.
"""
type ArticlesConnection {
  """
  Information to aid in pagination.
  """
  pageInfo: PageInfo!

  """
  A list of edges.
  """
  edges: [ArticlesEdge!]

  """
  A flattened list of the nodes.
  """
  nodes: [Article!]
}

input CustomerSortInput {
  id: SortEnumType
  name: SortEnumType
  isActive: SortEnumType
}

input CustomerFilterInput {
  and: [CustomerFilterInput!]
  or: [CustomerFilterInput!]
  id: ComparableGuidOperationFilterInput
  name: StringOperationFilterInput
  isActive: BooleanOperationFilterInput
}

"""
A connection to a list of items.
"""
type CustomersConnection {
  """
  Information to aid in pagination.
  """
  pageInfo: PageInfo!

  """
  A list of edges.
  """
  edges: [CustomersEdge!]

  """
  A flattened list of the nodes.
  """
  nodes: [Customer!]
}

enum SortEnumType {
  ASC
  DESC
}

input ComparableGuidOperationFilterInput {
  eq: UUID
  neq: UUID
  in: [UUID!]
  nin: [UUID!]
  gt: UUID
  ngt: UUID
  gte: UUID
  ngte: UUID
  lt: UUID
  nlt: UUID
  lte: UUID
  nlte: UUID
}

input StringOperationFilterInput {
  and: [StringOperationFilterInput!]
  or: [StringOperationFilterInput!]
  eq: String
  neq: String
  contains: String
  ncontains: String
  in: [String]
  nin: [String]
  startsWith: String
  nstartsWith: String
  endsWith: String
  nendsWith: String
}

input ComparableSingleOperationFilterInput {
  eq: Float
  neq: Float
  in: [Float!]
  nin: [Float!]
  gt: Float
  ngt: Float
  gte: Float
  ngte: Float
  lt: Float
  nlt: Float
  lte: Float
  nlte: Float
}

"""
Information about pagination in a connection.
"""
type PageInfo {
  """
  Indicates whether more edges exist following the set defined by the clients arguments.
  """
  hasNextPage: Boolean!

  """
  Indicates whether more edges exist prior the set defined by the clients arguments.
  """
  hasPreviousPage: Boolean!

  """
  When paginating backwards, the cursor to continue.
  """
  startCursor: String

  """
  When paginating forwards, the cursor to continue.
  """
  endCursor: String
}

type Article {
  id: UUID!
  name: String!
  price: Float!
}

"""
An edge in a connection.
"""
type ArticlesEdge {
  """
  A cursor for use in pagination.
  """
  cursor: String!

  """
  The item at the end of the edge.
  """
  node: Article!
}

input BooleanOperationFilterInput {
  eq: Boolean
  neq: Boolean
}

type Customer {
  id: UUID!
  name: String!
  isActive: Boolean!
}

"""
An edge in a connection.
"""
type CustomersEdge {
  """
  A cursor for use in pagination.
  """
  cursor: String!

  """
  The item at the end of the edge.
  """
  node: Customer!
}

scalar UUID
```
