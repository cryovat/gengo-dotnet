Winterday.External.Gengo
========================

A .NET library for interfacing with the [Gengo](http://www.gengo.com)
API for translation.

* **License:** [MIT](COPYING)
* **Stability:** Experimental (partial implementation)
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

* Account
  * Get Balance
  * Get Stats
* Job
  * Approve
  * Get
  * Delete (cancel)
  * Preview
  * Get revisions
  * Get revision
  * Get feedback
  * Get comments
  * Post comment
  * Reject
  * Return for revision
* Jobs
  * Submit Jobs
  * Get recent jobs (search)
  * Get jobs by ID
* Order
 * Get order
 * Delete order
* Service
  * Get Languages
  * Get Language Pairs

Unimplemented methods
---------------------

* Jobs
  * Get job group
* Glossaries
  * Get glossaries
  * Get glossary

Todo
----

* Implement missing methods
* Refactor out some redundancy
