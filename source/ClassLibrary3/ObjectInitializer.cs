using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace ClassLibrary3
{
    public class ObjectInitializer
    {
        public Action<TObject, IDataRow> Create<TObject>()
        {
            var type = typeof(TObject);
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var arg1 = Expression.Parameter(type, "instance");
            var arg2 = Expression.Parameter(typeof(IDataRow), "dataRow");
            var variables = new List<ParameterExpression>();
            var statements = new List<Expression>();

            foreach (var property in properties)
            {
                if (false == property.CanWrite)
                {
                    continue;
                }
                
                var assign = Expression.Assign(
                    Expression.MakeMemberAccess(arg1, property),
                    CreateDataRowCall(property.PropertyType, arg2)
                );

                statements.Add(assign);
            }

            var lambda = Expression.Lambda<Action<TObject, IDataRow>>(
                Expression.Block(variables, statements),
                arg1, arg2
            );

            return lambda.Compile();
        }

        private static MethodCallExpression CreateDataRowCall(Type propertyType, ParameterExpression dataRow)
        {
            var type = dataRow.Type;
            const BindingFlags bindingAttributes = BindingFlags.Public | BindingFlags.Instance;
            MethodInfo method;

            if (typeof(int) == propertyType)
            {
                method = type.GetMethod(nameof(IDataRow.ReadInt32), bindingAttributes);
            }
            else if (typeof(long) == propertyType)
            {
                method = type.GetMethod(nameof(IDataRow.ReadInt64), bindingAttributes);
            }
            else if (typeof(string) == propertyType)
            {
                method = type.GetMethod(nameof(IDataRow.ReadString), bindingAttributes);
            }
            else
            {
                throw new Exception();
            }

            return Expression.Call(dataRow, method);
        }
    }
}