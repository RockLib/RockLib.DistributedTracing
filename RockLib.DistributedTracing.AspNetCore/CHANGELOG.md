# RockLib.DistributedTracing.AspNetCore Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

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
