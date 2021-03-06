Out Variable Declarations
=========================

The *out variable declaration* feature enables a variable to be declared at the location that it is being passed as an `out` argument.

```antlr
argument_value
    : 'out' type identifier
    | ...
    ;
```

A variable declared this way is called an *out variable*. 
You may use the contextual keyword `var` for the variable's type.
The scope will be the same as for a *pattern-variable* introduced via pattern-matching.
Out variables are disallowed within constructor initializers.

According to Language Specification (section 7.6.7 Element access)
The argument-list of an element-access is not allowed to contain ref or out arguments.
However, due to backward compatibility, compiler overlooks this restriction during parsing
and even ignores out/ref modifiers in element access during binding.
We will enforce that language rule for out variables declarations at the syntax level.

Within the scope of a local variable introduced by a local-variable-declaration, 
it is a compile-time error to refer to that local variable in a textual position 
that precedes its declaration. 

It is also an error to reference implicitly-typed (�8.5.1) out variable in the same argument list that immediately 
contains its declaration. Technically, we could support some restricted scenarios (when it is used as another 
argument in the same list), but their utility is very questionable. Otherwise, we are looking into getting into 
a cycle trying to infer its type. 

For the purposes of overload resolution (see sections 7.5.3.2 Better function member and 7.5.3.3 Better conversion from expression),
neither conversion is considered better when corresponding argument is an implicitly-typed out variable declaration.
Once overload resolution succeeds, the type of implicitly-typed out variable is set to be equal to the type of the 
corresponding parameter in the signature of the best candidate.

> **Note**: There is a discussion thread for this feature at https://github.com/dotnet/roslyn/issues/6183



**ArgumentSyntax node is extended as follows to accommodate for the new syntax:**

- ```Expression``` field is replaced with the following field
```
    <Field Name="ExpressionOrDeclaration" Type="CSharpSyntaxNode" >
      <PropertyComment>
        <summary>
          ExpressionSyntax node representing the argument value, 
          or VariableDeclarationSyntax representing out local declaration.
        </summary>
      </PropertyComment>
    </Field>
```
The Expression property in the public API is preserved.
VariableDeclarationSyntax is expected to contain single VariableDeclarator without initializer or arguments.


**Open issues and TODOs:**
    Tracked at https://github.com/dotnet/roslyn/issues/11566.
