# jaytwo.LocalTime

[![NuGet Version](https://img.shields.io/nuget/v/jaytwo.LocalTime.svg?style=flat&logo=nuget)](https://www.nuget.org/packages/jaytwo.LocalTime)
[![NuGet Downloads](https://img.shields.io/nuget/dt/jaytwo.LocalTime.svg?style=flat)](https://www.nuget.org/packages/jaytwo.LocalTime)
[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](https://mit-license.org/)

`jaytwo.LocalTime` is a tiny, focused library for handling local times without surprises.

[View source on GitHub](https://github.com/jakegough-jaytwo/jaytwo.LocalTime)

## Features

* Convert between **local time** and **UTC** with a known time zone
* Translate times from **one time zone to another**
* Handle **DST transitions** with configurable strict/lenient rules
* Perform **clock-aware arithmetic** (add/subtract across DST changes correctly)
* Deterministic "now" injection for **testing**
* Simple **ASP.NET Core DI** integration (`ILocalTimeService`, `ILocalTimeTranslator`)
* Based on **IANA TZDB** identifiers (via NodaTime) for reliable cross-platform behavior
* Verbose DST Resolution API (detect ambiguous/skipped times, see alternatives)
* Optional truncation of "now" to a specified precision (e.g., seconds) for database compatibility

## Installation

Add the NuGet package:

```powershell
PM> Install-Package jaytwo.LocalTime
```

## Usage

* Use `ILocalTimeService` to work between a known local time zone and UTC.
* Use `ILocalTimeTranslator` to convert a local time from one zone to another.

### Examples

**Basic Usage**

1) Local time ↔ UTC

```csharp
using jaytwo.LocalTime;

var localTimeService = new LocalTimeService("America/Denver"); // throws if TZ ID is invalid
var nowLocal = localTimeService.LocalNow;                      // DateTimeOffset (local zone)
var nowUtc   = localTimeService.UtcNow;                        // DateTimeOffset (UTC)

// Conversions
var timestamp = localTimeService.GetDateTimeOffset(someLocalDateTime); // local -> DateTimeOffset
var utc = localTimeService.GetUtcDateTime(someLocalDateTime);          // local -> UTC DateTime
var local = localTimeService.GetLocalDateTimeFromUtc(someUtcDateTime); // UTC -> local DateTime (Unspecified)
```

> Need deterministic _now_ (e.g., for tests)?
> `LocalTimeService.Create("America/Denver", utcNowFactory: () => fixedUtcNow)`

**DST Handling**

Pass `throwOnAmbiguousOrSkipped: true` to throw on DST gaps/ambiguities (or set it in the constructor). Default is lenient resolution.

2) Clock-aware arithmetic (handles DST correctly)

```csharp
// Add across DST transitions
var plus1h = localTimeService.AddHours(DateTime.Parse("2025-03-09T01:00:00"), 1); // returns 2025-03-09T03:00:00-06:00

// Subtract two local wall-clock times (real elapsed time)
var elapsed = localTimeService.Subtract(
    DateTime.Parse("2025-11-02T03:00:00"),
    DateTime.Parse("2025-11-02T00:00:00")); // returns 4 hours
```

3) Zone → zone translation

```csharp
var localTimeTranslator = new LocalTimeTranslator("America/Denver", "America/Los_Angeles");

// Local DateTime in input zone → local DateTimeOffset in output zone
var losAngelesDateTimeOffset = localTimeTranslator.ToOutputDateTimeOffset(someDenverDateTime);

// Or get a local DateTime in the output zone
var losAngelesDateTime = localTimeTranslator.ToOutputDateTime(someDenverDateTime);
```

> Note: Using `nowPrecision` only affects `LocalNow` and `UtcNow`; it does not truncate time values passed to other methods.

4) Verbose DST Resolution

```csharp
// In the US, for a 'fall back' DST transition, the 1 AM hour repeats
var ambiguousLocalTime = DateTime.Parse("2025-11-02T01:30:00");

// resolve with details
var resolved = localTimeService.Resolve(ambiguousLocalTime);

// resolved.IsAmbiguous()        → true
// resolved.IsSkipped()          → false
// resolved.Matches[0]           → 2025-11-02T01:30:00-06:00
// resolved.Matches[1]           → 2025-11-02T01:30:00-07:00
// resolved.ForwardShifted       → null
// resolved.StartOfIntervalAfter → null
```

```csharp
// In the US, for a 'spring forward' DST transition, the 2 AM hour is skipped
var skippedLocalTime = DateTime.Parse("2025-03-09T02:30:00");

// resolve with details
var resolved = localTimeService.Resolve(skippedLocalTime);

// resolved.IsAmbiguous()        → false
// resolved.IsSkipped()          → true
// resolved.Matches              → (empty)
// resolved.ForwardShifted       → 2025-03-09T03:30:00-07:00
// resolved.StartOfIntervalAfter → 2025-03-09T03:00:00-07:00
```

**Quantized `Now`**

Optionally truncate "now" to a specified precision (e.g., seconds, milliseconds) for database compatibility.  Though
the `DateTime` type can represent time with sub-microsecond precision, sometimes other systems (e.g., databases) do not.

```csharp
var service = new LocalTimeService("America/Denver", nowPrecision: TimePrecision.Second);

// DateTimeOffset.Now   → 2025-01-01T12:34:56.78987654-07:00 (full sub-microsecond precision)
// service.LocalNow     → 2025-01-01T12:34:56.00000000-07:00 (truncated to second precision)
```

**Injecting with DI (typical ASP.NET Core)**

```csharp
services.AddSingleton<ILocalTimeService>(sp => new LocalTimeService("America/Denver"));
```

> __Note:__ "Local" refers to the time zone configured on the service (`TimeZoneId`).

## Notes

| Concept                                        | Behavior                                                                                                                                                                                                                            |
| ---------------------------------------------- | ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **Time zones**                                 | `TimeZoneId` expects an **IANA TZDB** identifier (e.g., `America/Denver`). Invalid IDs throw.                                                                                                                                       |
| **`DateTime.Kind`**                            | Local `DateTime` results use `Kind = Unspecified` to represent a wall-clock time without an offset (common in UI/domain models). UTC parameters should be passed with `DateTimeKind.Utc`.                                           |
| **DST handling (`throwOnAmbiguousOrSkipped`)** | `true` → throw on **skipped** (spring-forward gap) and **ambiguous** (fall-back overlap) local times. `false` (default) → resolve **leniently** (shift into the gap or prefer a standard offset per the underlying implementation). |

---

Made with &hearts; by Jake — Licensed under the [MIT License](https://mit-license.org/)
