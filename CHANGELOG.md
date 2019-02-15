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
