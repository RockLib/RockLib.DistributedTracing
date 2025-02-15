# RockLib.DistributedTracing.AspNetCore Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## 4.0.0 - 2025-01-23

#### Changed
- Removed .NET 6 as a target framework.
- Updated the following package references:
	- Microsoft.AspNetCore.Http.Abstractions from 2.2.0 to 2.3.0
	- Microsoft.Extensions.Options from 8.0.0 to 9.0.0
	- OpenTelemetry from 1.7.0 to 1.11.1
	- System.Text.Encodings.Web from 8.0.0 to 9.0.0

## 3.0.0 - 2024-02-15

#### Changed
- Finalized 3.0.0 version.

## 3.0.0-alpha.1 - 2024-01-04

#### Changed
- Supported targets: net6.0, net8.0, and net48 (netcoreapp3.1 was removed).

## 2.1.0 - 2022-10-19

#### Added
- TraceId and SpanId retrieved from Open Telemetry and made available in the CorrelationIdAccessor.
- OpenTelemetry dependency 1.3.1

#### Changed
- TraceId value is now used as CorrelationId, when CorelationId is not present, before creating a new value.

## 2.1.0-alpha.1 - 2022-10-18

#### Added
- TraceId and SpanId retrieved from Open Telemetry and made available in the CorrelationIdAccessor.
- OpenTelemetry dependency 1.3.1

#### Changed
- TraceId value is now used as CorrelationId, when CorelationId is not present, before creating a new value.

## 2.0.0 - 2022-02-08

#### Added
- Added `.editorconfig` and `Directory.Build.props` files to ensure consistency.

#### Changed
- Supported targets: net6.0, netcoreapp3.1, and net48.
- As the package now uses nullable reference types, some method parameters now specify if they can accept nullable values.

## 1.0.3 - 2021-08-12

#### Changed

- Changes "Quicken Loans" to "Rocket Mortgage".

## 1.0.2 - 2021-05-06

#### Added

- Adds SourceLink to nuget package.

----

**Note:** Release notes in the above format are not available for earlier versions of
RockLib.DistributedTracing.AspNetCore. What follows below are the original release notes.

----

## 1.0.1

Updates net5.0 target to use a framework reference to Microsoft.AspNetCore.App
instead of package references to Microsoft.AspNetCore.Http.Abstractions and
Microsoft.Extensions.Options.

## 1.0.0

Initial release.
