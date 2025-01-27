# How to contribute

We love getting patches to NServiceBus from our awesome community. Here is a few guidelines that we
need contributors to follow so that we can have a chance of keeping on
top of things.

## Getting Started

* Make sure you have a [GitHub account](https://github.com/signup/free)
* [Create a new issue](https://github.com/NServiceBus/NServiceBus/issues/new), assuming one does not already exist.
  * Clearly describe the issue including steps to reproduce when it is a bug.
  * If it is a bug make sure you tell us what version you have encountered this bug on.
* Fork the repository on GitHub

## Running the Acceptance Tests
 
Follow these steps to run the acceptance tests locally:

* Add a new environment variable `Transport.UseSpecific` with the value `AzureServiceBusTransport`
* Add a new environment variable `AzureServiceBusTransport.ConnectionString` containing a connection string to your Azure storage account 
* Add a new environment variable `AzureServiceBusTransport.Topology` and set it to _ForwardingTopology_ to run tests configuring transport with `ForwardingTopology` topology. Don't setup `AzureServiceBusTransport.Topology` environment variable to run tests with `EndpointOrientedTopology` topology


## Running Unit/Integration Tests

To execute tests under `NServiceBus.AzureServiceBus.Tests`, two environment variables are required:

1. `AzureServiceBus.ConnectionString`
1. `AzureServiceBus.ConnectionString.Fallback`

Note that those should **not** point to the same namespace.

## Making Changes

* Create a feature branch from where you want to base your work.
  * This is usually the develop branch since we never do any work off our master branch. The master is always our latest stable release
  * Only target release branches if you are certain your fix must be on that
    branch.
  * To quickly create a feature branch based on develop; `git branch
    fix/develop/my_contribution` then checkout the new branch with `git
    checkout fix/develop/my_contribution`.  Please avoid working directly on the
    `develop` branch.
* Make commits of logical units.
* Check for unnecessary whitespace with `git diff --check` before committing.
* Make sure your commit messages are in the proper format.
* Make sure you have added the necessary tests for your changes.
* Run build.bat in the root to assure nothing else was accidentally broken.
* We have a resharper layer that applies our coding standards so make sure that you're "all green in reshaper"


## Submitting Changes

* Push your changes to a feature branch in your fork of the repository.
* Submit a pull request to the NServiceBus repository

# Additional Resources

* [General GitHub documentation](http://help.github.com/)
* [GitHub pull request documentation](http://help.github.com/send-pull-requests/)
