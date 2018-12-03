using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Model.Utility
{

    public class LambdaQueryCondition
    {
        public static Expression<Func<T, bool>> GetFilterExpression<T>(List<FilterModel> filterConditionList)
        {
            Expression<Func<T, bool>> condition = null;
            try
            {
                if (filterConditionList != null && filterConditionList.Count > 0)
                {
                    foreach (FilterModel filterCondition in filterConditionList)
                    {
                        Expression<Func<T, bool>> tempCondition = CreateLambda<T>(filterCondition);
                        if (condition == null)
                        {
                            condition = tempCondition;
                        }
                        else
                        {
                            if ("AND".Equals(filterCondition.logic))
                            {
                                condition = condition.And(tempCondition);
                            }
                            else
                            {
                                condition = condition.Or(tempCondition);
                            }
                        }
                    }
                }
            }
            catch (Exception )
            {
                throw ;
            }
            return condition;
        }

        public static Expression<Func<T, bool>> CreateLambda<T>(FilterModel filterCondition)
        {
            var parameter = Expression.Parameter(typeof(T), "p");//创建参数i
            var constant = Expression.Constant(filterCondition.value);//创建常数
            MemberExpression member = Expression.PropertyOrField(parameter, filterCondition.key);
            if ("=".Equals(filterCondition.action))
            {
                return Expression.Lambda<Func<T, bool>>(Expression.Equal(member, constant), parameter);
            }
            else if ("!=".Equals(filterCondition.action))
            {
                return Expression.Lambda<Func<T, bool>>(Expression.NotEqual(member, constant), parameter);
            }
            else if (">".Equals(filterCondition.action))
            {
                return Expression.Lambda<Func<T, bool>>(Expression.GreaterThan(member, constant), parameter);
            }
            else if ("<".Equals(filterCondition.action))
            {
                return Expression.Lambda<Func<T, bool>>(Expression.LessThan(member, constant), parameter);
            }
            else if (">=".Equals(filterCondition.action))
            {
                return Expression.Lambda<Func<T, bool>>(Expression.GreaterThanOrEqual(member, constant), parameter);
            }
            else if ("<=".Equals(filterCondition.action))
            {
                return Expression.Lambda<Func<T, bool>>(Expression.LessThanOrEqual(member, constant), parameter);
            }
            else if ("in".Equals(filterCondition.action) && "1".Equals(filterCondition.dataType))
            {
                return GetExpressionWithMethod<T>("Contains", filterCondition);
            }
            else if ("out".Equals(filterCondition.action) && "1".Equals(filterCondition.dataType))
            {
                return GetExpressionWithoutMethod<T>("Contains", filterCondition);
            }
            else
            {
                return null;
            }
        }

        public static Expression<Func<T, bool>> GetExpressionWithMethod<T>(string methodName, FilterModel filterCondition)
        {
            ParameterExpression parameterExpression = Expression.Parameter(typeof(T), "p");
            MethodCallExpression methodExpression = GetMethodExpression(methodName, filterCondition.key, filterCondition.value, parameterExpression);
            return Expression.Lambda<Func<T, bool>>(methodExpression, parameterExpression);
        }

        public static Expression<Func<T, bool>> GetExpressionWithoutMethod<T>(string methodName, FilterModel filterCondition)
        {
            ParameterExpression parameterExpression = Expression.Parameter(typeof(T), "p");
            MethodCallExpression methodExpression = GetMethodExpression(methodName, filterCondition.key, filterCondition.value, parameterExpression);
            var notMethodExpression = Expression.Not(methodExpression);
            return Expression.Lambda<Func<T, bool>>(notMethodExpression, parameterExpression);
        }

        /// <summary>
        /// 生成类似于p=>p.values.Contains("xxx");的lambda表达式
        /// parameterExpression标识p，propertyName表示values，propertyValue表示"xxx",methodName表示Contains
        /// 仅处理p的属性类型为string这种情况
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="propertyName"></param>
        /// <param name="propertyValue"></param>
        /// <param name="parameterExpression"></param>
        /// <returns></returns>
        private static MethodCallExpression GetMethodExpression(string methodName, string propertyName, string propertyValue, ParameterExpression parameterExpression)
        {
            var propertyExpression = Expression.Property(parameterExpression, propertyName);
            MethodInfo method = typeof(string).GetMethod(methodName, new[] { typeof(string) });
            var someValue = Expression.Constant(propertyValue, typeof(string));
            return Expression.Call(propertyExpression, method, someValue);
        }

    }

    public static class LinqBuilder
    {
        /// <summary>
        /// 默认True条件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Expression<Func<T, bool>> True<T>() { return f => true; }

        /// <summary>
        /// 默认False条件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Expression<Func<T, bool>> False<T>() { return f => false; }

        /// <summary>
        /// 拼接 OR 条件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exp"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> exp, Expression<Func<T, bool>> condition)
        {
            var inv = Expression.Invoke(condition, exp.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>(Expression.Or(exp.Body, inv), exp.Parameters);
        }

        /// <summary>
        /// 拼接And条件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exp"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> exp, Expression<Func<T, bool>> condition)
        {
            var inv = Expression.Invoke(condition, exp.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>(Expression.And(exp.Body, inv), exp.Parameters);
        }
    }
}
