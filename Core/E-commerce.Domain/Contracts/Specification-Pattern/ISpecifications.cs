using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace E_commerce.Domain.Contracts.Specification_Pattern
{
    // why where TEntity : IEntity<TKey> ,
    // because we want to make sure that the entity has a key and we can use it in the specifications
    public interface ISpecifications<TEntity , TKey> where TEntity : IEntity<TKey>
    {
        // this is the where clause in the specifications return true or false
        Expression<Func<TEntity, bool>> Criteria { get; } 
        
        // this is the include clause in the specifications to include related entities return a list of expressions like x => x.Category
        List<Expression<Func<TEntity, object>>> Includes { get; }

        // this is the order by clause in the specifications to order the results
        // return a list of OrderExpressionInfo which contains the expression
        // and the order type (ascending or descending)
        List<OrderExpressionInfo<TEntity>> OrderExpressionInfo { get; }

        // Pagenation properties
        int Take { get; } // how many records to take
        int Skip { get; } // how many records to skip
        bool IsPagenationEnabled { get; } // to enable or disable pagenation
    }
}
