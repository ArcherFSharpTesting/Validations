<!-- GENERATED DOCUMENT DO NOT EDIT! -->
<!-- prettier-ignore-start -->
<!-- markdownlint-disable -->

<!-- Compiled with doculisp https://www.npmjs.com/package/doculisp -->

# Archer.Fletching: Test Validations for the Archer Test Framework #

1. Overview: [Philosophy of Archer.Fletching Test Validations](#philosophy-of-archerfletching-test-validations)
2. HowTo: [How to Use Archer.Fletching Test Validations](#how-to-use-archerfletching-test-validations)
3. Feature: [Should Object Validation Functions](#should-object-validation-functions)
4. Feature: [Should Result Validation Functions](#should-result-validation-functions)
5. Feature: [Should Boolean Validation Functions](#should-boolean-validation-functions)
6. Feature: [Should Other Validation Functions](#should-other-validation-functions)
7. Feature: [Should MeetStandard Validation Functions](#should-meetstandard-validation-functions)
8. Feature: [ListShould List Validation Functions](#listshould-list-validation-functions)
9. Feature: [SeqShould Sequence Validation Functions](#seqshould-sequence-validation-functions)
10. Feature: [ArrayShould Array Validation Functions](#arrayshould-array-validation-functions)
11. Feature: [Not Validation Helper](#not-validation-helper)
12. Review: [Archer.Validations](#archervalidations)

## Philosophy of Archer.Fletching Test Validations ##

Archer.Fletching test validations are designed to provide a functional, composable, and expressive approach to test assertions in F#. Unlike traditional assertion libraries that throw exceptions on failure, Archer.Fletching validations return a `TestResult` value. This enables:

- **Composability:** Test results can be combined, piped, and further processed, supporting complex validation flows.
- **Functional Style:** Validations are functions that can be used directly or in pipelines, aligning with idiomatic F# code.
- **Clarity:** Each validation is explicit about what it checks, making tests easy to read and maintain.
- **Extensibility:** Users can define custom validations and compose them with built-in ones.
- **Non-Exception Flow:** By avoiding exceptions for control flow, tests remain predictable and side-effect free.

The philosophy is to empower developers to write robust, maintainable, and expressive tests that fit naturally into functional programming workflows.

## How to Use Archer.Fletching Test Validations ##

Archer.Fletching test validations are designed to be functional, composable, and expressive. The following usage patterns and return value conventions apply to all validation helpers:

### Usage Patterns ###

- **Direct Invocation:**
  ```fsharp
  let result = Should.BeTrue true
  let result = ListShould.Contain 2 [1;2;3]
  ```

- **Pipe Notation:**
  ```fsharp
  let result = true |> Should.BeTrue
  let result = [1;2;3] |> ListShould.Contain 2
  ```

- **Custom Predicates:**
  Many validations accept F# quotations or functions for custom checks.

### Return Values ###

All validation helpers return a `TestResult` value, not an exception. This enables:
- Composability: Combine results, pipe through further checks, or aggregate outcomes.
- Predictability: No exceptions for control flow; failures are explicit values.
- Integration: Results can be used in custom test runners or reporting tools.

For more on the philosophy, see the [Philosophy](#overview) section.

## Should Object Validation Functions ##

Object validations check equality, reference, type, and custom predicates using the `Should` helper.

### Overview ###

The `Should` type provides static members for validating objects in various ways. These validations include equality, reference checks, type checks, null/default checks, and predicate-based custom checks.

---

### Object Validation Methods ###

#### Equality and Reference ####

- **BeEqualTo ( expected )**
  - Passes if the actual value is equal to `expected` (`=`).
- **NotBeEqualTo ( expected )**
  - Passes if the actual value is not equal to `expected` (`<>`).
- **BeSameAs ( expected )**
  - Passes if the actual value is the same reference as `expected`.
- **NotBeSameAs ( expected )**
  - Passes if the actual value is not the same reference as `expected`.

#### Type Checks ####

- **BeOfType<'expectedType> ( actual )**
  - Passes if `actual` is an instance of `'expectedType`.
- **NotBeTypeOf<'expectedType> ( actual )**
  - Passes if `actual` is not an instance of `'expectedType`.

#### Null and Default Checks ####

- **BeNull<'T when 'T : null> ( actual )**
  - Passes if `actual` is `null`.
- **NotBeNull<'T when 'T : null> ( actual )**
  - Passes if `actual` is not `null`.
- **BeDefaultOf<'T when 'T : equality> ( actual )**
  - Passes if `actual` is the default value for type `'T`.
- **NotBeDefaultOf<'T when 'T : equality> ( actual )**
  - Passes if `actual` is not the default value for type `'T`.

#### Predicate and Custom Checks ####

- **PassTestOf ( predicateExpression )**
  - Passes if the provided predicate expression returns `true` for the actual value.
  - Example: `Should.PassTestOf ( <@ fun x -> x > 0 @> )`
- **NotPassTestOf ( predicateExpression )**
  - Passes if the provided predicate expression returns `false` for the actual value.
- **PassAllOf ( tests )**
  - Passes if all provided test functions return a passing `TestResult` for the actual value.
  - Example: `Should.PassAllOf [ test1; test2; test3 ]`

---

### Usage Example ###

```fsharp
open Archer.Validations.Lib

// Direct invocation
let result1 = Should.BeEqualTo ( 42 ) 42
let result2 = Should.NotBeNull ( "hello" )
let result3 = Should.BeOfType<string> ( "test" )
let result4 = Should.PassTestOf ( <@ fun x -> x > 10 @> ) 15

// Using pipe notation
let result5 = 42 |> Should.BeEqualTo ( 42 )
let result6 = "hello" |> Should.NotBeNull
let result7 = "test" |> Should.BeOfType<string>
let result8 = 15 |> Should.PassTestOf ( <@ fun x -> x > 10 @> )
```

See [How to Use Fletcher Test Validations](#how-to-use-fletcher-test-validations) for usage patterns and return value details.

For more details, see the source in `Lib/ShouldType.Objects.fs`.

## Should Result Validation Functions ##

Result validations check if a value is `Ok` or `Error` using the `Should` helper.

### Overview ###

The `Should` type provides static members for validating F# `Result` values. These validations check whether a value is an `Ok` or an `Error` with the expected content.

---

### Result Validation Methods ###

- **BeOk ( expected )**
  - Passes if the actual value is `Ok expected`.
- **BeError ( expected )**
  - Passes if the actual value is `Error expected`.

---

### Usage Example ###

```fsharp
open Archer.Validations.Lib

// Direct invocation
let result1 = Should.BeOk ( 42 ) ( Ok 42 )
let result2 = Should.BeError ( "fail" ) ( Error "fail" )

// Using pipe notation
let result3 = Ok 42 |> Should.BeOk ( 42 )
let result4 = Error "fail" |> Should.BeError ( "fail" )
```

See [How to Use Fletcher Test Validations](#how-to-use-fletcher-test-validations) for usage patterns and return value details.

For more details, see the source in `Lib/ShouldType.Result.fs`.

## Should Boolean Validation Functions ##

Boolean validations check if a value is `true` or `false` using the `Should` helper.

### Overview ###

The `Should` type provides static members for validating boolean values. These validations check whether a value is `true` or `false`.

---

### Boolean Validation Methods ###

- **BeTrue ( actual )**
  - Passes if the actual value is `true`.
- **BeFalse ( actual )**
  - Passes if the actual value is `false`.

---

### Usage Example ###

```fsharp
open Archer.Validations.Lib

// Direct invocation
let result1 = Should.BeTrue ( true )
let result2 = Should.BeFalse ( false )

// Using pipe notation
let result3 = true |> Should.BeTrue
let result4 = false |> Should.BeFalse
```

See [How to Use Fletcher Test Validations](#how-to-use-fletcher-test-validations) for usage patterns and return value details.

For more details, see the source in `Lib/ShouldType.Boolean.fs`.

## Should Other Validation Functions ##

Other validations include marking tests as failed or ignored using the `Should` helper.

### Overview ###

The `Should` type provides static members for miscellaneous test outcomes, such as marking a test as failed or ignored.

---

### Other Validation Methods ###

- **Fail ( message )**
  - Marks the test as failed with the provided message.
- **BeIgnored**
  - Marks the test as ignored. Can be called with or without a message.
  - **BeIgnored ( )**: Ignores the test without a message.
  - **BeIgnored ( message )**: Ignores the test with a custom message.

---

### Usage Example ###

```fsharp
open Archer.Validations.Lib

// Mark a test as failed
let result1 = Should.Fail ( "This test should fail." )

// Ignore a test without a message
let result2 = Should.BeIgnored ( ) "any value"

// Ignore a test with a message
let result3 = Should.BeIgnored ( "This test is ignored for now." ) "any value"
```

See [How to Use Fletcher Test Validations](#how-to-use-fletcher-test-validations) for usage patterns and return value details.

For more details, see the source in `Lib/ShouldType.Other.fs`.

## Should MeetStandard Validation Functions ##

MeetStandard validations enable approval-style testing by comparing output to an approved standard (golden master).

### Overview ###

The `Should.MeetStandard` function enables approval testing by comparing a test result (usually a string) to an approved file. If the result does not match the approved standard, a reporter is used to display the difference.

---

### MeetStandard Validation Method ###

- **MeetStandard ( reporter )**
  - Returns a function that takes an `ITestInfo` and a `string` result, and checks if the result matches the approved standard using the provided reporter.
  - If the result does not match, the reporter is invoked to show the difference.

---

### Usage Example ###

```fsharp
open Archer.Validations.Lib
open ApprovalTests.Reporters

let reporter = DiffReporter() :> ApprovalTests.Core.IApprovalFailureReporter
let testInfo = // ... obtain or create an ITestInfo instance ...
let result = "output to verify"

let testResult = Should.MeetStandard ( reporter ) testInfo result
```

See [How to Use Fletcher Test Validations](#how-to-use-fletcher-test-validations) for usage patterns and return value details.

For more details, see the source in `Lib/ApprovalsSupport.fs`.

## ListShould List Validation Functions ##

List validations check containment, length, and predicates using the `ListShould` helper.

### Overview ###

The `ListShould` type provides static members for validating F# lists (`'a list`). These validations include containment, length, and predicate-based checks.

---

### List Validation Methods ###

- **Contain ( value )**
  - Passes if the list contains the specified value.
- **NotContain ( value )**
  - Passes if the list does not contain the specified value.
- **HaveAllValuesPassTestOf ( predicateExpression )**
  - Passes if all values in the list satisfy the given predicate expression.
- **HaveNoValuesPassTestOf ( predicateExpression )**
  - Passes if no values in the list satisfy the given predicate expression.
- **HaveLengthOf ( length )**
  - Passes if the list has the specified length.
- **NotHaveLengthOf ( length )**
  - Passes if the list does not have the specified length.
- **HaveAllValuesPassAllOf ( tests )**
  - Passes if all values in the list pass all provided test functions.
- **HaveAllValuesBe ( value )**
  - Passes if all values in the list are equal to the specified value.

---

### Usage Example ###

```fsharp
open Archer.Validations.Lib

let numbers = [ 1; 2; 3 ]

// Direct invocation
let result1 = ListShould.Contain ( 2 ) numbers
let result2 = ListShould.HaveLengthOf ( 3 ) numbers

// Using pipe notation
let result3 = numbers |> ListShould.NotContain ( 4 )
let result4 = numbers |> ListShould.HaveAllValuesPassTestOf ( <@ fun x -> x > 0 @> )
```

See [How to Use Fletcher Test Validations](#how-to-use-fletcher-test-validations) for usage patterns and return value details.

For more details, see the source in `Lib/ListShould.fs`.

## SeqShould Sequence Validation Functions ##

Sequence validations check containment, length, and predicates using the `SeqShould` helper.

### Overview ###

The `SeqShould` type provides static members for validating F# sequences (`seq<'a>`). These validations include containment, length, and predicate-based checks.

---

### Sequence Validation Methods ###

- **Contain ( value )**
  - Passes if the sequence contains the specified value.
- **NotContain ( value )**
  - Passes if the sequence does not contain the specified value.
- **HaveAllValuesPassTestOf ( predicateExpression )**
  - Passes if all values in the sequence satisfy the given predicate expression.
- **HaveNoValuesPassTestOf ( predicateExpression )**
  - Passes if no values in the sequence satisfy the given predicate expression.
- **HaveLengthOf ( length )**
  - Passes if the sequence has the specified length.
- **NotHaveLengthOf ( length )**
  - Passes if the sequence does not have the specified length.
- **HaveAllValuesPassAllOf ( tests )**
  - Passes if all values in the sequence pass all provided test functions.
- **HaveAllValuesBe ( value )**
  - Passes if all values in the sequence are equal to the specified value.

---

### Usage Example ###

```fsharp
open Archer.Validations.Lib

let numbers = seq { 1; 2; 3 }

// Direct invocation
let result1 = SeqShould.Contain ( 2 ) numbers
let result2 = SeqShould.HaveLengthOf ( 3 ) numbers

// Using pipe notation
let result3 = numbers |> SeqShould.NotContain ( 4 )
let result4 = numbers |> SeqShould.HaveAllValuesPassTestOf ( <@ fun x -> x > 0 @> )
```

See [How to Use Fletcher Test Validations](#how-to-use-fletcher-test-validations) for usage patterns and return value details.

For more details, see the source in `Lib/SeqShould.fs`.

## ArrayShould Array Validation Functions ##

Array validations check containment, length, and predicates using the `ArrayShould` helper.

### Overview ###

The `ArrayShould` type provides static members for validating F# arrays (`'a[]`). These validations include containment, length, and predicate-based checks.

---

### Array Validation Methods ###

- **Contain ( value )**
  - Passes if the array contains the specified value.
- **NotContain ( value )**
  - Passes if the array does not contain the specified value.
- **HaveAllValuesPassTestOf ( predicateExpression )**
  - Passes if all values in the array satisfy the given predicate expression.
- **HaveNoValuesPassTestOf ( predicateExpression )**
  - Passes if no values in the array satisfy the given predicate expression.
- **HaveLengthOf ( length )**
  - Passes if the array has the specified length.
- **NotHaveLengthOf ( length )**
  - Passes if the array does not have the specified length.
- **HaveAllValuesPassAllOf ( tests )**
  - Passes if all values in the array pass all provided test functions.
- **HaveAllValuesBe ( value )**
  - Passes if all values in the array are equal to the specified value.

---

### Usage Example ###

```fsharp
open Archer.Validations.Lib

let numbers = [| 1; 2; 3 |]

// Direct invocation
let result1 = ArrayShould.Contain ( 2 ) numbers
let result2 = ArrayShould.HaveLengthOf ( 3 ) numbers

// Using pipe notation
let result3 = numbers |> ArrayShould.NotContain ( 4 )
let result4 = numbers |> ArrayShould.HaveAllValuesPassTestOf ( <@ fun x -> x > 0 @> )
```

See [How to Use Fletcher Test Validations](#how-to-use-fletcher-test-validations) for usage patterns and return value details.

For more details, see the source in `Lib/ArrayShould.fs`.

## Not Validation Helper ##

The `Not` helper marks tests or features as "not yet implemented" in a way that integrates with the test result system.

### Overview ###

The `Not` type provides a static member for marking a test as not yet implemented. Instead of throwing an exception, it returns a `TestResult` indicating the test is ignored, which can be useful for test-driven development or feature planning.

---

### Not Validation Method ###

- **Implemented ( )**
  - Marks the test as ignored with the message "Not Yet Implemented".
  - Can be used to indicate that a test or feature is planned but not yet available.

---

### Usage Example ###

```fsharp
open Archer.Validations.Lib

let result = Not.Implemented ( )
```

See [How to Use Fletcher Test Validations](#how-to-use-fletcher-test-validations) for usage patterns and return value details.

For more details, see the source in `Lib/NotImplemented.fs`.

## Archer.Validations ##

A test verification Framework for Archer

### Complete Features ###

- Should
  - Object
    - {value} |> Should.BeEqualTo {value}
    - {value} |> Should.NotBeEqualTo {value}
    - {value} |> Should.BeSameAs {value}
    - {value} |> Should.NotBeSameAs {value}
    - {value} |> Should.BeOfType<Type>
    - {value} |> Should.NotBeOfType<Type>
    - {value} |> Should.BeNull
    - {value} |> Should.NotBeNull
    - {value} |> Should.BeDefaultOf<Type>
    - {value} |> Should.NotBeDefaultOf<Type>
    - {value} |> Should.PassTestOf {predicate}
    - {value} |> Should.NotPassTestOf {predicate}
    - {value} |> Should.PassAllOf [ {value -> TestResult} ]
  - Result
    - {result} |> Should.BeOk {value}
    - {result} |> Should.BeError {value}
  - Boolean
    - {bool} |> Should.BeTrue
    - {bool} |> Should.BeFalse
  - Approvals
    - {testInfo} |> Should.MeetStandard {reporter} {string}
  - Other
    - {string} |> Should.Fail
    - {value} |> Should.BeIgnored {string}
    - {value} |> Should.BeIgnored
- ListShould
  - {list} |> ListShould.Contain {value}
  - {list} |> ListShould.NotContain {value}
  - {list} |> ListShould.HaveAllValuesPassTestOf {predicateExpression}
  - {list} |> ListShould.HaveNoValuesPassTestOf {predicateExpression}
  - {list} |> ListShould.HaveLengthOf {integer}
  - {list} |> ListShould.NotHaveLengthOf {integer}
  - {list} |> ListShould.HaveAllValuesPassAllOf [ {value -> TestResult} ]
  - {list} |> ListShould.HaveAllValuesPassTestOf {indexedPredicateExpression}
  - {list} |> ListShould.HaveNoValuesPassTestOf {indexedPredicateExpression}
- SeqShould
  - {collection} |> SeqShould.Contain {value}
  - {collection} |> SeqShould.NotContain {value}
  - {collection} |> SeqShould.HaveAllValuesPassTestOf {predicateExpression}
  - {collection} |> SeqShould.HaveNoValuesPassTestOf {predicateExpression}
  - {collection} |> SeqShould.HaveLengthOf {integer}
  - {collection} |> SeqShould.NotHaveLengthOf {integer}
  - {collection} |> SeqShould.HaveAllValuesPassAllOf [ {value -> TestResult} ]
  - {collection} |> SeqShould.HaveAllValuesPassTestOf {indexedPredicateExpression}
  - {collection} |> SeqShould.HaveNoValuesPassTestOf {indexedPredicateExpression}
- ArrayShould
  - {array} |> ArrayShould.Contain {value}
  - {array} |> ArrayShould.NotContain {value}
  - {array} |> ArrayShould.HaveAllValuesPassTestOf {predicateExpression}
  - {array} |> ArrayShould.HaveNoValuesPassTestOf {predicateExpression}
  - {array} |> ArrayShould.HaveLengthOf {integer}
  - {array} |> ArrayShould.NotHaveLengthOf {integer}
  - {array} |> ArrayShould.HaveAllValuesPassAllOf [ {value -> TestResult} ]
  - {array} |> ArrayShould.HaveAllValuesPassTestOf {indexedPredicateExpression}
  - {array} |> ArrayShould.HaveNoValuesPassTestOf {indexedPredicateExpression}
- Not
  - Not.Implemented ()

### Feature status ###

This is a list of feature ideas. All features on this list _may_ or _may not_ end up in the final product.

#### Should ####

- Dictionary
  - [ ] {dictionary} |> Should.HaveKey {key}
  - [ ] {dictionary} |> Should.NotHaveKey {key}
  - [ ] {dictionary} |> Should.HaveValue {value}
  - [ ] {dictionary} |> Should.HavePair ({key}, {value})
  - [ ] {dictionary} |> Should.BeEmpty
  - [ ] {dictionary} |> Should.NotBeEmpty
- Object
  - [x] {value} |> Should.BeEqualTo {value}
  - [x] {value} |> Should.NotBeEqualTo {value}
  - [x] {value} |> Should.BeSameAs {value}
  - [x] {value} |> Should.NotBeSameAs {value}
  - [x] {value} |> Should.BeOfType\<Type\>
  - [x] {value} |> Should.NotBeOfType\<Type\>
  - [x] {value} |> Should.BeNull
  - [x] {value} |> Should.NotBeNull
  - [x] {value} |> Should.BeDefaultOf\<Type\>
  - [x] {value} |> Should.NotBeDefaultOf\<Type\>
  - [x] {value} |> Should.PassTestOf {predicate}
  - [x] {value} |> Should.NotPassTestOf {predicate}
  - [x] {value} |> Should.PassAllOf [ {value -> TestResult} ]
- Result
  - [x] {result} |> Should.BeOk {value}
  - [x] {result} |> Should.BeError {value}
- Functions
  - [ ] {action} |> Should.Return {value}
  - [ ] {action} |> Should.NotReturnValue {value}
  - [ ] {action} |> Should.ThrowException
  - [ ] {action} |> Should.NotThrowException
  - [ ] {action} |> Should.Call {action} |> withParameter {predicate} {initialParameter}
- Events
  - [ ] {IEvent} |> Should.Trigger |> by {action}
  - [ ] {IEvent} |> Should.NotTrigger |> by {action}
  - [ ] {IEvent} |> Should.TriggerWith {expectedArgs} |> by {action}
- String
  - [ ] {string} |> Should.Contain {string}
  - [ ] {string} |> Should.NotContain {string}
  - [ ] {string} |> Should.StartsWith {prefix}
  - [ ] {string} |> Should.EndWith {suffix}
  - [ ] {string} |> Should.HaveLengthOf {integer}
  - [ ] {string} |> Should.BeMatchedBy {regex}
  - [ ] {string} |> Should.NotBeMatchedBy {regex}
  - [ ] {string} |> Should.MatchStandard {ITestInfo} {reporter}
- Numbers
  - [ ] {number} |> Should.BeWithin ({number}, {number})
  - [ ] {number} |> Should.BeBetween ({number}, {number})
  - [ ] {number} |> Should.BeCloseTo {number} |> byDelta {number}
  - [ ] {number} |> Should.BeEqualTo {number}
  - [ ] {number} |> Should.BePositive
  - [ ] {number} |> Should.BeNegative
- Boolean
  - [x] {bool} |> Should.BeTrue
  - [x] {bool} |> Should.BeFalse
- Approvals
  - [x] {testInfo} |> Should.MeetStandard {reporter} {string}
- Other
  - [x] {string} |> Should.Fail
  - [x] {value} |> Should.BeIgnored {string}
  - [x] {value} |> Should.BeIgnored

#### ListShould ####

- [x] {list} |> ListShould.Contain {value}
- [x] {list} |> ListShould.NotContain {value}
- [ ] {list} |> ListShould.ContainAny {values}
- [ ] {list} |> ListShould.NotContainAny {values}
- [ ] {list} |> ListShould.ContainAll {values}
- [ ] {list} |> ListShould.NotContainAll {values}
- [ ] {list} |> ListShould.FindValueWith {predicateExpression}
- [ ] {list} |> ListShould.NotFindValueWith {predicateExpression}
- [x] {list} |> ListShould.HaveAllValuesPassTestOf {predicateExpression}
- [x] {list} |> ListShould.HaveNoValuesPassTestOf {predicateExpression}
- [ ] {list} |> ListShould.BeSorted
- [ ] {list} |> ListShould.NotBeSorted
- [ ] {list} |> ListShould.BeSortedBy {comparator}
- [ ] {list} |> ListShould.NotBeSortedBy {comparator}
- [ ] {list} |> ListShould.BeEmpty
- [ ] {list} |> ListShould.NotBeEmpty
- [x] {list} |> ListShould.HaveLengthOf {integer}
- [x] {list} |> ListShould.NotHaveLengthOf {integer}
- [x] {list} |> ListShould.HaveAllValuesPassAllOf [ {value -> TestResult} ]
- [ ] {list} |> ListShould.HaveAnyPassTestOf {predicateExpression}
- [ ] {list} |> ListShould.BeEmpty
- [ ] {list} |> ListShould.NotBeEmpty
- [ ] {list} |> ListShould.FindValueWith {indexedPredicateExpression}
- [ ] {list} |> ListShould.NotFindValueWith {indexedPredicateExpression}
- [x] {list} |> ListShould.HaveAllValuesPassTestOf {indexedPredicateExpression}
- [x] {list} |> ListShould.HaveNoValuesPassTestOf {indexedPredicateExpression}
- [ ] {list} |> ListShould.HaveAnyPassTestOf {indexedPredicateExpression}

#### SeqShould ####

- [x] {collection} |> SeqShould.Contain {value}
- [x] {collection} |> SeqShould.NotContain {value}
- [ ] {collection} |> SeqShould.ContainAny {values}
- [ ] {collection} |> SeqShould.NotContainAny {values}
- [ ] {collection} |> SeqShould.ContainAll {values}
- [ ] {collection} |> SeqShould.NotContainAll {values}
- [ ] {collection} |> SeqShould.FindValueWith {predicateExpression}
- [ ] {collection} |> SeqShould.NotFindValueWith {predicateExpression}
- [x] {collection} |> SeqShould.HaveAllValuesPassTestOf {predicateExpression}
- [x] {collection} |> SeqShould.HaveNoValuesPassTestOf {predicateExpression}
- [ ] {collection} |> SeqShould.BeSorted
- [ ] {collection} |> SeqShould.NotBeSorted
- [ ] {collection} |> SeqShould.BeSortedBy {comparator}
- [ ] {collection} |> SeqShould.NotBeSortedBy {comparator}
- [ ] {collection} |> SeqShould.BeEmpty
- [ ] {collection} |> SeqShould.NotBeEmpty
- [x] {collection} |> SeqShould.HaveLengthOf {integer}
- [x] {collection} |> SeqShould.NotHaveLengthOf {integer}
- [x] {collection} |> SeqShould.HaveAllValuesPassAllOf [ {value -> TestResult} ]
- [ ] {collection} |> SeqShould.HaveAnyPassTestOf {predicateExpression}
- [ ] {collection} |> SeqShould.BeEmpty
- [ ] {collection} |> SeqShould.NotBeEmpty
- [ ] {collection} |> SeqShould.FindValueWith {indexedPredicateExpression}
- [ ] {collection} |> SeqShould.NotFindValueWith {indexedPredicateExpression}
- [x] {collection} |> SeqShould.HaveAllValuesPassTestOf {indexedPredicateExpression}
- [x] {collection} |> SeqShould.HaveNoValuesPassTestOf {indexedPredicateExpression}
- [ ] {collection} |> SeqShould.HaveAnyPassTestOf {indexedPredicateExpression}

#### ArrayShould ####

- [x] {array} |> ArrayShould.Contain {value}
- [x] {array} |> ArrayShould.NotContain {value}
- [ ] {array} |> ArrayShould.ContainAny {values}
- [ ] {array} |> ArrayShould.NotContainAny {values}
- [ ] {array} |> ArrayShould.ContainAll {values}
- [ ] {array} |> ArrayShould.NotContainAll {values}
- [ ] {array} |> ArrayShould.FindValueWith {predicateExpression}
- [ ] {array} |> ArrayShould.NotFindValueWith {predicateExpression}
- [x] {array} |> ArrayShould.HaveAllValuesPassTestOf {predicateExpression}
- [x] {array} |> ArrayShould.HaveNoValuesPassTestOf {predicateExpression}
- [ ] {array} |> ArrayShould.BeSorted
- [ ] {array} |> ArrayShould.NotBeSorted
- [ ] {array} |> ArrayShould.BeSortedBy {comparator}
- [ ] {array} |> ArrayShould.NotBeSortedBy {comparator}
- [ ] {array} |> ArrayShould.BeEmpty
- [ ] {array} |> ArrayShould.NotBeEmpty
- [x] {array} |> ArrayShould.HaveLengthOf {integer}
- [x] {array} |> ArrayShould.NotHaveLengthOf {integer}
- [x] {array} |> ArrayShould.HaveAllValuesPassAllOf [ {value -> TestResult} ]
- [ ] {array} |> ArrayShould.HaveAnyPassTestOf {predicateExpression}
- [ ] {array} |> ArrayShould.BeEmpty
- [ ] {array} |> ArrayShould.NotBeEmpty
- [ ] {array} |> ArrayShould.FindValueWith {indexedPredicateExpression}
- [ ] {array} |> ArrayShould.NotFindValueWith {indexedPredicateExpression}
- [x] {array} |> ArrayShould.HaveAllValuesPassTestOf {indexedPredicateExpression}
- [x] {array} |> ArrayShould.HaveNoValuesPassTestOf {indexedPredicateExpression}
- [ ] {array} |> ArrayShould.HaveAnyPassTestOf {indexedPredicateExpression}

#### Not ####

- [x] Not.Implemented ()

<!-- markdownlint-restore -->
<!-- prettier-ignore-end -->
<!-- GENERATED DOCUMENT DO NOT EDIT! -->