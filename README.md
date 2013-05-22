Winterday.External.Gengo
========================

A .NET library for interfacing with the [Gengo](http://www.gengo.com)
API for translation.

* **License:** [MIT](COPYING)
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

* [Account](http://developers.gengo.com/v2/account/)
  * [Get Balance](http://developers.gengo.com/v2/account/#balance-get)
  * [Get Stats](http://developers.gengo.com/v2/account/#stats-get)
* [Job](http://developers.gengo.com/v2/job/)
  * [Approve](http://developers.gengo.com/v2/job/#job-put)
  * [Get](http://developers.gengo.com/v2/job/#job-get)
  * [Delete (cancel)](http://developers.gengo.com/v2/job/#job-delete)
  * [Preview](http://developers.gengo.com/v2/job/#job-put)
  * [Get revisions](http://developers.gengo.com/v2/job/#revisions-get)
  * [Get revision](http://developers.gengo.com/v2/job/#revision-get)
  * [Get feedback](http://developers.gengo.com/v2/job/#feedback-get)
  * [Get comments](http://developers.gengo.com/v2/job/#comment-post)
  * [Post comment](http://developers.gengo.com/v2/job/#comments-get)
  * [Reject](http://developers.gengo.com/v2/job/#job-put)
  * [Return for revision](http://developers.gengo.com/v2/job/#job-put)
* [Jobs](http://developers.gengo.com/v2/jobs/)
  * [Submit Jobs](http://developers.gengo.com/v2/jobs/#jobs-post)
  * [Get job group](http://developers.gengo.com/v2/jobs/#job-group-get)
  * [Get recent jobs (search)](http://developers.gengo.com/v2/jobs/#jobs-get)
  * [Get jobs by ID](http://developers.gengo.com/v2/jobs/#jobs-by-id-get)
* [Order](http://developers.gengo.com/v2/order/)
 * [Get order](http://developers.gengo.com/v2/order/#order-get)
 * [Delete order](http://developers.gengo.com/v2/order/#order-delete)
* [Service](http://developers.gengo.com/v2/service/)
  * [Get Languages](http://developers.gengo.com/v2/service/#languages-get)
  * [Get Language Pairs](http://developers.gengo.com/v2/service/#language-pairs-get)
  * [Get quote](http://developers.gengo.com/v2/service/#quote-post)
  * [Get quote based on file upload](http://developers.gengo.com/v2/service/#quote-files-post)

Unimplemented methods
---------------------

* [Glossary](http://developers.gengo.com/v2/glossary/)
  * [Get glossaries](http://developers.gengo.com/v2/glossary/#glossaries-get)
  * [Get glossary](http://developers.gengo.com/v2/glossary/#glossary-get)

Todo
----

* Implement missing methods
* Refactor out some redundancy
