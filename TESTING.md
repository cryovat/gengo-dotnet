Testing
=======

For tests to be able to run, API public and private keys need to be entered
into the [TestKeys.cs](Winterday.External.Gengo.Tests/TestKeys.cs) file.

The library comes with a unit test for the general case of each API
method, but some will initially be inconclusive, while others will seemingly
fail at random. Why this happens is described as follows:

Tests requiring manual action
-----------------------------

Some tests require a job to be set to the 'Reviewable' state in the Gengo
[Sandbox](https://sandbox.gengo.com/dashboard/) as there is currently no way
of doing this programmatically. These tests are specifically:

 * TestApproveJob
 * TestGetPreview
 * TestReturnForSubmission

Flimsy tests
------------

Job submission and certain other methods are processed asynchronously by the
Gengo servers. This means that there is a non-deterministic delay before methods
that query and manipulate jobs/orders will behave as expected.

In these cases, the affected unit tests have 
[Thread.Sleep](http://msdn.microsoft.com/en-us/library/system.threading.thread.sleep.aspx)s
with pragmatic delays scatted around, but may still fail randomly
based on current server load. Repeat offenders include:

 * TestCreateGetAndDeleteOrder
 * TestGetJobGroup

Methods not covered by tests
----------------------------
 
There is no test for the method for [rejecting](http://developers.gengo.com/v2/job/#job-put)
a job, as this requires solving a captcha.
