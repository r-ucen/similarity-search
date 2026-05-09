When youre buying a used car on the internet, the seller often reposts the same ad multiple times to seek an illusion of it being posted as a fresh ad, this project is a web API app that tries to counter this problem by flagging such posted ads as reposts.

How it works?
-----------------
The pipeline is as folows:
- The user creates or updates and ad via the API
- The API controller calls the application's CreateAdAsync/EditAdAsync method found in the AdService class, this method creates or updates the add and enqueues a background job through a method named EnqueueAdAnalysisAsync which is implemented in the infrastructure layer in the HangfireBackgroundJobService class using Hangfire
- When the job is executed, it uses the AnalyseAdAsync method implemented in the AdAnalysisService (implemented in application layer)
- The AnalyseAdAsync takes the ad's info and creates an embedding through the TextAnalysisService (implemented in infrastructure layer), which uses the IEmbeddingGenerator and a IChatClient both using OllamaApiClient to generate an embedding from a given text (the ad's description) and also to generate a reason why the other ad is or is not a reupload. This reason is however generated only if the ad is determined to be a repost. If the ad is probably a repost based on the cosine distance of the generated embedding, it is checked against the car being a different vehicle based on the model/brand and motor name and power using Levenshtein string difference algorithm through a method in the LevenshteinService (implemented in infrastructure).
- After all this is resolved, the ad is set ready for presentation

Prerequisities to run the project on a local machine
-----------------

- Docker (only for tests)
- .NET 10
- PostgreSQL 16 with pgvector extention
- Ollama (with  gemma3:1b and embeddinggemma:latest models)

User secrets
-----------------
The project uses the following user screts:

```
{
    "ConnectionStrings": {
        "DefaultConnection": "Host=localhost;Port=XXXXXXX;Database=XXXXXXX;Username=XXXXXXX;Password=XXXXXXX",
    },
    "ollamaUri": "http://localhost:11434",
    "Resend": {
        "ApiToken": "XXXXXXX",
        "FromEmail": "XXXXXXX"
    },
    "FrontendUrl": "http://localhost:5173", // for possible frontend
    "Seeding": {
        "AdminPasswordHash": "AQAAAAEAACcQAAAAEJG7rzRCJ4U/L082iC0ZBJOBfx2PDhDpDDni6fyjYPWG07AT4qnzdtL4TKpPhDt9Jg==", // Admin?1
        "ManagerPasswordHash": "AQAAAAEAACcQAAAAEOZvU6CJRz2MKLZ1gBuj41o1Zjr+vqI/UM273q2tHlXwpjtLri/G9QvmOWtOyepCsQ==" // Manager?1
    }
}
```

Use those secrets inside of the .Api project

Running the project
-----------------

Navigate to the solution directory and run the following command:

```
dotnet run --project .\src\SimilaritySearch.Api\SimilaritySearch.Api.csproj --launch-profile https
```

or via loading the solution into an IDE of your choice

Testing
-----------------
To run the tests, make sure you have Docker installed and running on your machine. Then, navigate to the solution directory and run the following command:

```
dotnet test
```

or via an IDE

The existing test resembles a sceneario of creating 5 ads using the admin account, first an ad is created and is flagged as isRepost=false because there is no other ad in the database to check for similarity, then the second ad (which is an actual repost) is added and is successfully flagged as isRepost=true, then the third ad is added and is flagged as isRepost=false because it is not similar to the first two ads, then a fourth ad is added and is flagged as isRepost=true because its just the third ad rewritten in a different way, and finally the fifth ad is added being flagged as isRepost=false. While it is written in a similar way to the third and fourth ads, the kW number is different meaning a different car.

API Documentation
-----------------
The API documentation is available through Scalar, you can access it in the development env here:
```
https://localhost:7086/scalar/
```

Similarly, you can check the background jobs dashboard through:
```
https://localhost:7086/hangfire
```