<!-- (dl
(section-meta
    (title How to Use Archer.Fletching Test Validations)
)
) -->

Archer.Fletching test validations are designed to be functional, composable, and expressive. The following usage patterns and return value conventions apply to all validation helpers:

<!-- (dl (# Usage Patterns)) -->

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

<!-- (dl (# Return Values)) -->

All validation helpers return a `TestResult` value, not an exception. This enables:
- Composability: Combine results, pipe through further checks, or aggregate outcomes.
- Predictability: No exceptions for control flow; failures are explicit values.
- Integration: Results can be used in custom test runners or reporting tools.

For more on the philosophy, see the [Philosophy](#overview) section.
