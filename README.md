Winterday.External.Gengo
========================

A .NET library for interfacing with the [Gengo](http://www.gengo.com)
API for translation with decent [unit test coverage](TESTING.md).

* **License:** [MIT](COPYING)
* **Contributors:** [See list](CONTRIBUTORS.md)
* **Stability:** Provisional
* **Dependencies:** [Newtonsoft.Json](https://github.com/JamesNK/Newtonsoft.Json/)
* **Supported platforms:** .NET 4.5 (Mono to come)
* **Gengo API version:** V2

Sample usage
------------

```csharp
// Client initialization
var client = new GengoClient(
    Settings.Default.PrivateKey,
    Settings.Default.PublicKey,
    ClientMode.Production);
        
// Submit a text for translation
var confirmation = await _client.Jobs.Submit(
    true,  // Require same translator on all jobs
    false, // Do not allow translator change
    new Job()
    {
        Slug = "Sample job",
        Body = "My hovercraft is full of eels",
        SourceLanguage = "en",
        TargetLanguage = "ja",
    });
    
// Submit a file for translation
var confirmation = await _client.Service.GetQuoteForFiles(
    new FileJob(@"c:\files\an_anthology_of_eels.txt")
    {
        Slug = "Sample job",
        SourceLanguage = "en",
        TargetLanguage = "ja",
    });

// Get reviewable jobs
var reviewable = await _client.Jobs.GetRecentJobs(
    status: TranslationStatus.Reviewable,
    afterDateTime: lastMonday,
    maxCount: 40);

```

Supported methods
-----------------

See http://developers.gengo.com/ for API overview/documentation

* [Account](http://developers.gengo.com/v2/api_methods/account/)
  * [Get Balance](http://developers.gengo.com/v2/api_methods/account/#balance-get)
  * [Get Stats](http://developers.gengo.com/v2/api_methods/account/#stats-get)
  * [Get Preferred Translators](http://developers.gengo.com/v2/api_methods/account/#preferred-translators-get)
* [Glossary](http://developers.gengo.com/v2/api_methods/glossary/)
  * [Get glossaries](http://developers.gengo.com/v2/api_methods/glossary/#glossaries-get)
  * [Get glossary](http://developers.gengo.com/v2/api_methods/glossary/#glossary-get)
* [Job](http://developers.gengo.com/v2/api_methods/job/)
  * [Approve](http://developers.gengo.com/v2/api_methods/job/#job-put)
  * [Get](http://developers.gengo.com/v2/api_methods/job/#job-get)
  * [Delete (cancel)](http://developers.gengo.com/v2/api_methods/job/#job-delete)
  * [Get revisions](http://developers.gengo.com/v2/api_methods/job/#revisions-get)
  * [Get revision](http://developers.gengo.com/v2/api_methods/job/#revision-get)
  * [Get feedback](http://developers.gengo.com/v2/api_methods/job/#feedback-get)
  * [Get comments](http://developers.gengo.com/v2/api_methods/job/#comments-get)
  * [Post comment](http://developers.gengo.com/v2/api_methods/job/#comment-post)
  * [Reject](http://developers.gengo.com/v2/api_methods/job/#job-put)
  * [Return for revision](http://developers.gengo.com/v2/api_methods/job/#job-put)
* [Jobs](http://developers.gengo.com/v2/api_methods/jobs/)
  * [Submit Jobs](http://developers.gengo.com/v2/api_methods/jobs/#jobs-post)
  * [Get recent jobs (search)](http://developers.gengo.com/v2/api_methods/jobs/#jobs-get)
  * [Get jobs by ID](http://developers.gengo.com/v2/api_methods/jobs/#jobs-by-id-get)
* [Order](http://developers.gengo.com/v2/api_methods/order/)
 * [Get order](http://developers.gengo.com/v2/api_methods/order/#order-get)
 * [Delete order](http://developers.gengo.com/v2/api_methods/order/#order-delete)
* [Service](http://developers.gengo.com/v2/api_methods/service/)
  * [Get Languages](http://developers.gengo.com/v2/api_methods/service/#languages-get)
  * [Get Language Pairs](http://developers.gengo.com/v2/api_methods/service/#language-pairs-get)
  * [Get quote](http://developers.gengo.com/v2/api_methods/service/#quote-post)
  * [Get quote based on file upload](http://developers.gengo.com/v2/api_methods/service/#quote-files-post)

Todo
----

* Refactor out some redundancy
* Inclue StyleCop in build process and get rid of any errors
