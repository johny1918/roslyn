// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis.Shared.Extensions;
using Microsoft.CodeAnalysis.Utilities;
using Roslyn.Utilities;

namespace Microsoft.CodeAnalysis.GenerateMember.GenerateParameterizedMember
{
    internal partial class AbstractGenerateParameterizedMemberService<TService, TSimpleNameSyntax, TExpressionSyntax, TInvocationExpressionSyntax>
    {
        protected class MethodSignatureInfo : SignatureInfo
        {
            private readonly IMethodSymbol _methodSymbol;

            public MethodSignatureInfo(
                SemanticDocument document,
                State state,
                IMethodSymbol methodSymbol)
                : base(document, state)
            {
                _methodSymbol = methodSymbol;
            }

            protected override ITypeSymbol DetermineReturnTypeWorker(CancellationToken cancellationToken)
            {
                if (State.IsInConditionalAccessExpression)
                {
                    return _methodSymbol.ReturnType.RemoveNullableIfPresent();
                }

                return _methodSymbol.ReturnType;
            }

            protected override IList<ITypeParameterSymbol> DetermineTypeParametersWorker(CancellationToken cancellationToken)
            {
                return _methodSymbol.TypeParameters;
            }

            protected override IList<RefKind> DetermineParameterModifiers(CancellationToken cancellationToken)
            {
                return _methodSymbol.Parameters.Select(p => p.RefKind).ToList();
            }

            protected override IList<bool> DetermineParameterOptionality(CancellationToken cancellationToken)
            {
                return _methodSymbol.Parameters.Select(p => p.IsOptional).ToList();
            }

            protected override IList<ITypeSymbol> DetermineParameterTypes(CancellationToken cancellationToken)
            {
                return _methodSymbol.Parameters.Select(p => p.Type).ToList();
            }

            protected override IList<ParameterName> DetermineParameterNames(CancellationToken cancellationToken)
            {
                return _methodSymbol.Parameters.Select(p => new ParameterName(p.Name, isFixed: true))
                                               .ToList();
            }

            protected override IList<ITypeSymbol> DetermineTypeArguments(CancellationToken cancellationToken)
            {
                return SpecializedCollections.EmptyList<ITypeSymbol>();
            }
        }
    }
}
