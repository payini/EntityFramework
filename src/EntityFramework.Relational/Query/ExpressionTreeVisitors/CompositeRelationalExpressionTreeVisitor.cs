﻿// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Linq.Expressions;
using JetBrains.Annotations;
using Remotion.Linq.Parsing;

namespace Microsoft.Data.Entity.Relational.Query.ExpressionTreeVisitors
{
    public class CompositePredicateExpressionTreeVisitor : ExpressionTreeVisitor
    {
        public CompositePredicateExpressionTreeVisitor()
        {
        }

        public override Expression VisitExpression(
            [NotNull]Expression expression)
        {
            var currentExpression = expression;
            var inExpressionOptimized = 
                new EqualityPredicateInExpressionOptimizer().VisitExpression(currentExpression);

            if (inExpressionOptimized != null)
            {
                currentExpression = inExpressionOptimized;
            }

            var negationOptimized1 =
                new PredicateNegationExpressionOptimizer().VisitExpression(currentExpression);

            if (negationOptimized1 != null)
            {
                currentExpression = negationOptimized1;
            }

            var equalityExpanded = 
                new EqualityPredicateExpandingVisitor().VisitExpression(currentExpression);

            if (equalityExpanded != null)
            {
                currentExpression = equalityExpanded;
            }

            var negationOptimized2 =
                new PredicateNegationExpressionOptimizer().VisitExpression(currentExpression);

            if (negationOptimized2 != null)
            {
                currentExpression = negationOptimized2;
            }

            //var nullSemanticsExpanded =
            //    new PredicateNullSemanticsExpandingVisitor().VisitExpression(currentExpression);

            //if (nullSemanticsExpanded != null)
            //{
            //    currentExpression = nullSemanticsExpanded;
            //}

            return currentExpression;
        }
    }
}
