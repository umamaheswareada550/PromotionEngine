using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace PromotionEngine.Specifications
{
    public abstract class Specification<T>
    {
        public Specification()
        {
        }

        public abstract Expression<Func<T, T>> ToExpression();

        public T IsSatisfiedBy(T entity)
        {
            Func<T, T> predicate = ToExpression().Compile();
            return predicate(entity);
        }
    }
}
