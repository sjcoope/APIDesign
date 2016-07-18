# Web API Design Principles
The following is a quick discussion around the design principles/processes I've found important when I'm creating a Web API.

I've also created a sample .NET Core Web API Project to illustrate some of the implementation details discussed below.  This can be accessed on [Github](https://github.com/sjcoope/APIDesign). 

## RESTFul-ness
REST is a set of constraints that ensure a scalable, fault-tolerant and extendible system.  In the context of Web APIs, REST can be thought of as an architectural style for designing networked resources that communicate via a transfer protocol (usually HTTP).

There are 6 REST constraints: client-server; stateless; cacheable; layered system and uniform interface.  While all are significant, the uniform interface is fundamental to the design of the API.  The uniform interface defines the contract between the client and server.  Therefore, we should work to ensure this is simple and that the client client is in no way coupled to the archtitecture of the server.  This is achieved by ensuring the following:

* Use URIs as resource identifiers (e.g. api/products).
* Manipulate resources through transfer methods (e.g. HTTP Get, HTTP Post, HTTP Put, etc.)
* The outcome of operations is exposed via predefined statuses (e.g. OK - 200, Internal Server Error - 500, etc.)

To achieve the above, I've always found it useful to consider the following areas.

### URIs
Although it may occasionally _feel_ wrong I would always suggest pluralising the name of a URI.  This is because it's more intuitive in the long run to work with an API like _"api/products/10"_ than _"api/product/10"_. 

### Operations
The operation to perform on an API is constructed of the URI and the HTTP method.  The main HTTP methods use are:

| HTTP Method | Description                                            |
|-------------|--------------------------------------------------------|
| GET         | Used to get a list of resources or a specific resource |
| POST        | Create a new resource                                  |
| PUT         | Update an existing resource                            |
| PATCH       | Partially update an existing resource                  |
| DELETE      | Delete a resource                                      |

### Responses
The reponse of an operation should include a HTTP Status code and (depending on the HTTP method - POST, PUT or PATCH) a representation of the resource (e.g. in JSON or XML).  Some examples of scenarios and expected responses is shown below.

| HTTP Method | Example URL                       | HTTP Status      | Description                                                 |
|-------------|-----------------------------------|------------------|-------------------------------------------------------------|
| GET         | /api/products                     | 200 OK           | Success - Records returned                                  |
|             |                                   | 500 Server Error | Failed - An error occurred on the server                    |
| GET         | /api/products/1                   | 200 OK           | Success - The selected resource will be returned            |
|             |                                   | 400 Bad Request  | Failed - The request was incorrect                          |
|             |                                   | 404 Not Found    | Failed - The selected resource was not found                |
|             |                                   | 500 Server Error | Failed - An error occurred on the server                    |
| POST        | /api/products                     | 200 OK           | Success - The resource was created and will be returned     |
|             |                                   | 400 Bad Request  | Failed - The resource could not be created.                 |
|             |                                   | 500 Server Error | Failed - An error occurred on the server                    |
| PUT         | /api/products/1                   | 200 OK           | Success - The resource was updated and will be returned     |
|             |                                   | 404 Not Found    | Failed - The resource could not found.                      |
|             |                                   | 400 Bad Request  | Failed - The resource could not be updated.                 |
|             |                                   | 500 Server Error | Failed - An error occurred on the server                    |
| PATCH       | /api/products/1                   | 200 OK           | Success - The resource was updated and will be returned     |
|             |                                   | 404 Not Found    | Failed - The resource could not found.                      |
|             |                                   | 400 Bad Request  | Failed - The resource could not be updated.                 |
|             |                                   | 500 Server Error | Failed - An error occurred on the server                    |
| DELETE      | /api/products/1                   | 204 No Content   | Success - The resource was deleted and will not be returned |
|             |                                   | 404 Not Found    | Failed - The resource could not found.                      |
|             |                                   | 400 Bad Request  | Failed - The resource could not be deleted.                 |
|             |                                   | 500 Server Error | Failed - An error occurred on the server                    |

**Please Note:** Some return codes are debatable, for example, using _400 - Bad Request_ when we can't create/update/delete a resource isn't strictly correct.  We could instead us _409 - Conflict_, which may be more accurrate as there may be a conflict because the resource already exists (in the case of create or update).  However, the W3C specification for _400 - Bad Request_ indicates that the client should not retry the operation, while the _409 - Conflict_ specification suggests the client should be able to resolve the conflict and resubmit the request.  So in this case we don't really want the client to retry the operation as they'll have the same resulse, hence I've tended towards using _400 - Bad Request_.

## Documentation
It's really important to provide documentation of your API.  It's even better if this is done automatically via some tool that can interpret you're API and document the request and response contracts for you.

There are many tools available, but one of the best is [Swashbuckle/Swagger](https://github.com/domaindrivendev/Swashbuckle).

This lets us create APIs, that are then easy to test locally via the Swagger UI.  We can also add additional metadata to an API action method to add further information accessible via Swagger.

```csharp

/// <summary>
/// Add new student
/// </summary>
/// <param name="student">Student Model</param>
/// <remarks>Insert new student</remarks>
/// <response code="400">Bad request</response>
/// <response code="500">Internal Server Error</response>

```

## Data Sorting and Paging
TODO

## Validation
TODO

## Versioning
In the case of versioning your API, there are 4 main approaches.

### URI
This approach uses the URL to provide different routes to different versions.  For example, _/api/v1/products_ or _/api/v2/products_.  This approach is simple to implement and favours the developer because it's easier to jump between different versions.  However, this approach breaks REST principles because the URL should represent the resource.  Meaning we should always be able to request a resource from _/api/products_ and not a different version of a resource from _/api/v1/products_.

### Custom Request Header
In this approach we add a custom header to the request, for example _"api-version: 2".  Overall, this approach is more complicated than versioning via the URI.  It means we can't just send someone a URL to test our new version, instead they have to construct the request in the correct way to access the newly versioned resource.  

It does, however, mean that we can have very granular control of which resources are versioned and how they are consumed.

Though, overall this approach is more work and more difficult to test as we're adding a new header to the request.

### Content Negotiation
The content negotiation approach consists of changing the accept-header used to specify the format of the response data.  For example:

**Before:** 
HTTP GET:
https://domain.com/api/products
Accept: application/json

**After:**
HTTP GET:
https://domain.com/api/products
Accept: application/vnd.domain.v2+json

Using this approach the accept header follows [media type specifications](https://tools.ietf.org/html/rfc6838) providing a _"vendor tree"_, which is indicated by the _vnd_ prefix.  Then we supply a domain name and the version number, before finally specifying the content type we want returned (in this case _json_).

I personally prefer this approach because it means we don't have to version an entire API (i.e. all services) when a single resource/service has changed.  Meaning it gives us more granular control over updates to APIs, where a new version could be released and as long as there are no breaking changes and we maintain previous versions the consumer isn't affected until they opt to upgrade to the new version.  It also means we don't have to maintain different routing rules to direct consumers to the correct version of the API (as with the URI versioning approach). 

### Mixture
We could also, of course, implement a strategy that is a mixture of the above approaches.  For example, having the major version specified in the URI and the minor version in the _accept-header_.  But in my opinion this approach offers uneccessary complexity and is very easy for consumers to get wrong.

## Conclusions
There are far more areas of Web API design that could also be discussed (e.g. HATEOS, caching, response compression, etc.) but for now the above is sufficient to indicate some of the best practices I've found while working on these types of APIs in the past.