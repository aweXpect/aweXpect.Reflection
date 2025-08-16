# aweXpect.Reflection vs FluentAssertions: Feature Comparison

This document provides a comprehensive comparison between aweXpect.Reflection and FluentAssertions for reflection-based testing scenarios. Both libraries offer powerful capabilities for asserting against reflection types, but with different syntax patterns and feature sets.

## Overview

**aweXpect.Reflection** is designed as an extension to the aweXpect testing framework, providing expectations for reflection types with async/await support and rich filtering capabilities.

**FluentAssertions** is a popular general-purpose assertion library that includes extensive reflection testing features alongside other assertion types.

## Feature Comparison Matrix

| Feature Category | aweXpect.Reflection | FluentAssertions | Notes |
|------------------|---------------------|------------------|-------|
| **Assembly Testing** | ✅ | ✅ | Both support assembly-level assertions |
| **Type Testing** | ✅ | ✅ | Both support comprehensive type assertions |
| **Method Testing** | ✅ | ✅ | Both support method-level assertions |
| **Property Testing** | ✅ | ✅ | Both support property assertions |
| **Field Testing** | ✅ | ✅ | Both support field assertions |
| **Event Testing** | ✅ | ✅ | Both support event assertions |
| **Constructor Testing** | ✅ | ✅ | Both support constructor assertions |
| **Attribute Testing** | ✅ | ✅ | Both support attribute presence/value testing |
| **Collection Filtering** | ✅ | ⚠️ | aweXpect has more advanced filtering via `In` |
| **Async Support** | ✅ | ❌ | aweXpect is fully async, FluentAssertions is sync |
| **String Matching Options** | ✅ | ✅ | Both support prefix, suffix, regex, wildcards |
| **Dependency Testing** | ✅ | ❌ | aweXpect has assembly dependency checks |
| **Access Modifier Testing** | ✅ | ✅ | Both support public/private/protected/internal |
| **Type Kind Testing** | ✅ | ✅ | Both support class/interface/enum/abstract/etc |

## Detailed Feature Breakdown

### 1. Assembly Assertions

#### aweXpect.Reflection
```csharp
// Single assembly
await Expect.That(assembly).HasName("MyAssembly");
await Expect.That(assembly).HasADependencyOn("System.Core");
await Expect.That(assembly).HasNoDependencyOn("UnwantedDependency");

// Multiple assemblies
Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
await Expect.That(assemblies).HaveName("System").AsPrefix();
```

#### FluentAssertions
```csharp
// Single assembly
assembly.Should().HaveAssemblyName("MyAssembly");

// Multiple assemblies - requires more manual work
Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
assemblies.Should().OnlyContain(a => a.FullName.StartsWith("System"));
```

**Winner: aweXpect.Reflection** - More specific dependency testing features.

### 2. Type Assertions

#### aweXpect.Reflection
```csharp
// Single type
await Expect.That(typeof(MyClass)).IsAClass();
await Expect.That(typeof(MyInterface)).IsAnInterface();
await Expect.That(typeof(MyEnum)).IsAnEnum();
await Expect.That(typeof(AbstractClass)).IsAbstract();
await Expect.That(typeof(MyClass)).HasNamespace("MyNamespace");
await Expect.That(typeof(MyClass)).Has<MyAttribute>();

// Multiple types with advanced filtering
await Expect.That(In.AllLoadedAssemblies()
    .Types().WhichAreClasses().WhichArePublic())
    .HaveNamespace("MyNamespace").AsPrefix();
```

#### FluentAssertions
```csharp
// Single type
typeof(MyClass).Should().BeClass();
typeof(MyInterface).Should().BeInterface();
typeof(MyEnum).Should().BeEnum();
typeof(AbstractClass).Should().BeAbstract();
typeof(MyClass).Should().BeInNamespace("MyNamespace");
typeof(MyClass).Should().BeDecoratedWith<MyAttribute>();

// Multiple types
var types = Assembly.GetExecutingAssembly().GetTypes();
types.Should().OnlyContain(t => t.IsClass && t.IsPublic);
```

**Winner: Tie** - Both provide comprehensive type testing, aweXpect has better collection filtering.

### 3. Method Assertions

#### aweXpect.Reflection
```csharp
// Single method
MethodInfo method = typeof(MyClass).GetMethod("MyMethod");
await Expect.That(method).IsPublic();
await Expect.That(method).HasName("MyMethod");
await Expect.That(method).Has<ObsoleteAttribute>();

// Multiple methods with filtering
await Expect.That(In.AssemblyContaining<MyClass>()
    .Methods().WhichArePublic().With<TestAttribute>())
    .HaveName("Test").AsPrefix();
```

#### FluentAssertions
```csharp
// Single method
MethodInfo method = typeof(MyClass).GetMethod("MyMethod");
method.Should().BePublic();
method.Should().HaveName("MyMethod");
method.Should().BeDecoratedWith<ObsoleteAttribute>();

// Multiple methods
var methods = typeof(MyClass).GetMethods();
methods.Should().OnlyContain(m => m.IsPublic);
```

**Winner: aweXpect.Reflection** - Superior filtering and collection handling.

### 4. Property Assertions

#### aweXpect.Reflection
```csharp
// Single property
PropertyInfo property = typeof(MyClass).GetProperty("MyProperty");
await Expect.That(property).IsPublic();
await Expect.That(property).HasName("MyProperty");

// Multiple properties
await Expect.That(In.AssemblyContaining<MyClass>()
    .Properties().WhichArePublic())
    .HaveName("Id").AsSuffix();
```

#### FluentAssertions
```csharp
// Single property
PropertyInfo property = typeof(MyClass).GetProperty("MyProperty");
property.Should().BePublic();
property.Should().HaveName("MyProperty");

// Multiple properties
var properties = typeof(MyClass).GetProperties();
properties.Should().OnlyContain(p => p.GetGetMethod().IsPublic);
```

**Winner: aweXpect.Reflection** - Better collection support and cleaner syntax.

### 5. Advanced Filtering and Collection Operations

#### aweXpect.Reflection
```csharp
// Complex filtering scenarios
await Expect.That(In.AllLoadedAssemblies()
    .Types().WhichAreClasses().WhichArePublic()
    .Methods().WhichArePublic().With<TestAttribute>()
    .DeclaringTypes())
    .HaveName("Tests").AsSuffix();

// Filter by return types
await Expect.That(In.AssemblyContaining<MyClass>()
    .Methods().WhichReturn<string>().WhichArePublic())
    .HaveName("Get").AsPrefix();
```

#### FluentAssertions
```csharp
// Requires more manual LINQ operations
var testClasses = Assembly.GetExecutingAssembly()
    .GetTypes()
    .Where(t => t.IsClass && t.IsPublic)
    .Where(t => t.GetMethods()
        .Any(m => m.GetCustomAttribute<TestAttribute>() != null));
    
testClasses.Should().OnlyContain(t => t.Name.EndsWith("Tests"));

// Return type filtering requires manual work
var stringMethods = typeof(MyClass)
    .GetMethods()
    .Where(m => m.ReturnType == typeof(string) && m.IsPublic);
stringMethods.Should().OnlyContain(m => m.Name.StartsWith("Get"));
```

**Winner: aweXpect.Reflection** - Significantly more powerful and intuitive filtering.

## Unique Features

### aweXpect.Reflection Exclusive Features

1. **Assembly Dependency Testing**
   ```csharp
   await Expect.That(assembly).HasADependencyOn("RequiredLibrary");
   await Expect.That(assembly).HasNoDependencyOn("ForbiddenLibrary");
   ```

2. **Advanced Collection Filtering with `In` Helper**
   ```csharp
   In.AllLoadedAssemblies()
     .Types().WhichAreClasses()
     .Methods().With<TestAttribute>()
     .DeclaringTypes();
   ```

3. **Full Async/Await Support**
   ```csharp
   await Expect.That(type).IsAClass();
   ```

4. **Declarative Type Navigation**
   ```csharp
   In.AssemblyContaining<MyClass>()
     .Methods().DeclaringTypes()
     .Properties().DeclaringTypes();
   ```

### FluentAssertions Exclusive Features

1. **Broader Ecosystem Integration**
   - Works with any testing framework
   - Part of larger assertion library
   - More community resources and examples

2. **Immediate Execution (No Async Required)**
   ```csharp
   typeof(MyClass).Should().BeClass(); // Executes immediately
   ```

3. **More Mature and Established**
   - Longer development history
   - More comprehensive documentation
   - Larger user base

## Syntax Comparison Examples

### Type Testing
```csharp
// aweXpect.Reflection
await Expect.That(typeof(MyClass)).IsAClass();
await Expect.That(typeof(MyClass)).IsAbstract();
await Expect.That(typeof(MyClass)).HasNamespace("MyApp.Domain");

// FluentAssertions  
typeof(MyClass).Should().BeClass();
typeof(MyClass).Should().BeAbstract();
typeof(MyClass).Should().BeInNamespace("MyApp.Domain");
```

### Method Testing
```csharp
// aweXpect.Reflection
await Expect.That(method).IsPublic();
await Expect.That(method).HasName("Execute");
await Expect.That(method).Has<ObsoleteAttribute>();

// FluentAssertions
method.Should().BePublic();
method.Should().HaveName("Execute");
method.Should().BeDecoratedWith<ObsoleteAttribute>();
```

### Collection Testing
```csharp
// aweXpect.Reflection
await Expect.That(In.AssemblyContaining<MyClass>()
    .Types().WhichAreClasses())
    .HaveName("Service").AsSuffix();

// FluentAssertions
var types = Assembly.GetAssembly(typeof(MyClass)).GetTypes();
types.Where(t => t.IsClass)
     .Should().OnlyContain(t => t.Name.EndsWith("Service"));
```

## Migration Considerations

### From FluentAssertions to aweXpect.Reflection

**Advantages:**
- More powerful collection filtering
- Cleaner syntax for complex reflection scenarios  
- Assembly dependency testing
- Better integration with async test methods

**Considerations:**
- Requires adopting aweXpect testing framework
- Learning new API surface
- Async/await pattern throughout

### From aweXpect.Reflection to FluentAssertions

**Advantages:**
- Broader ecosystem and community
- Works with any testing framework
- More documentation and examples available
- No async requirement

**Considerations:**
- Loss of advanced filtering capabilities
- More verbose syntax for complex scenarios
- Manual implementation needed for dependency testing

## Conclusion

Both libraries excel in reflection testing but serve different needs:

**Choose aweXpect.Reflection if:**
- You're using the aweXpect testing framework
- You need advanced collection filtering and navigation
- Assembly dependency testing is important
- You prefer async/await patterns
- Complex reflection queries are common in your tests

**Choose FluentAssertions if:**
- You're using a different testing framework (xUnit, NUnit, MSTest)
- You need a mature, widely-adopted library
- Simple reflection assertions are sufficient
- You prefer synchronous execution
- You want the broader FluentAssertions ecosystem

For teams already using aweXpect, aweXpect.Reflection provides superior filtering and collection operations. For teams using other frameworks, FluentAssertions offers broader compatibility and ecosystem support.