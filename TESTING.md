Testing
=======

For tests to be able to run, API public and private keys need to be entered
into the [TestKeys.cs](Winterday.External.Gengo.Tests/TestKeys.cs) file.

The library comes with a unit test for the general case of each API
method, but some will be inconclusive, while others will occasionally fail.

Gengo does certain kinds of processing asynchronously after submission. This
means that for a short time, the "next logical API call" will not return the
expected value.

In these cases, the affected unit tests have Thread.Sleep()'s scatted around,
but may still fail nondeterministically. Repeat offenders include:

 * TestCreateGetAndDeleteOrder
 * TestGetJobGroup

Some tests require a job to be set in the reviewable status in the Gengo
[Sandbox](https://sandbox.gengo.com/dashboard/). There is currently now way
of doing this programtically. These are specifically:

 * TestApproveJob
 * TestGetPreview
 * TestReturnForSubmission
 
There is no test for the method for [rejecting](http://developers.gengo.com/v2/job/#job-put)
a job, as this requires solving a captcha.
