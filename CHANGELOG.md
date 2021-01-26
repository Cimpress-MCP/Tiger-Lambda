### What's new in 4.0.1 (Released 2021-01-26)

* The library's compatibility has been greatly widened.

### What's new in 4.0.0 (Released 2020-09-XX)

* Handlers now accept a cancellation token.
  * The cancellation tokens are synthesized by the library; they will be canceled when approximately 500ms remain in the Function execution.
  * This value is not yet configurable.
* Special types have been added for handling SQS events serialized to JSON.

### What's new in 3.0.0 (Released 2020-07-07)

* The library has been updated for .NET Core 3.1.
* System settings are now set via environment variables with the standard "DOTNET_" prefix, rather than "LAMBDA_".
* (Dependency) The secret configuration now accepts a collection of secret IDs (probably ARNs) from which to read.
  * It no longer accepts a "base ID".
* (Administrivia) The project's code of conduct has been updated.
* (Administrivia) The project main branch has been renamed `main`.

### What's new in 2.0.3 (Released 2019-05-11)

* Secrets Manager support now comes from an external library, Tiger.Secrets.

### What's new in 2.0.2 (Released 2019-02-15)

* The AWS Request ID is added to the top level logging scope, if scopes are enabled.

### What's new in 2.0.1 (Released 2019-02-08)

* An alternative key delimiter for Secrets Manager secrets has been introduced.
  * If a secret needs to be referenced in a CloudFormation dynamic reference, a double-underscore (__, or "dunder") may be used in lieu of colon (:).

### What's new in 2.0.0 (Released 2018-12-11)

* AWS Secrets Manager support has been integrated. See `LambdaHost.CreateSecretsBuilder()`.
* Special support for AWS Step Functions has been upstreamed. For async exception support, investigate using the `UNWRAP_AGGREGATE_EXCEPTIONS` environment variable.

### What's new in 1.0.1 (Released 2018-07-25)

* The collapsing of appsettings files has been fixed.

### What's new in 1.0.0 (Released 2018-07-13)

* Everything is new!
