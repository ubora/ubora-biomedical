using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DomainModels.Specifications
{
    public abstract class CompositeSpecification<TEntity> : Specification<TEntity>, IEquatable<CompositeSpecification<TEntity>>
    {
        public IEnumerable<Specification<TEntity>> Specifications { get; }

        protected CompositeSpecification(params Specification<TEntity>[] specifications)
        {
            if (specifications == null) throw new ArgumentNullException(nameof(specifications));
            
            Specifications = Reduce(specifications);
        }

        /// <summary>
        /// Reduce same kind of composite specifications to one
        /// </summary>
        /// <param name="specifications"></param>
        /// <returns>reduced specifications</returns>
        private IEnumerable<Specification<TEntity>> Reduce(IEnumerable<Specification<TEntity>> specifications)
        {
            var reducedSpecifications = new List<Specification<TEntity>>();
            foreach (var specification in specifications)
            {
                var compositeSpecification = specification as CompositeSpecification<TEntity>;

                if (compositeSpecification != null && compositeSpecification.GetType() == GetType())
                    foreach (var compInnerSpec in compositeSpecification.Specifications)
                    {
                        if (!reducedSpecifications.Contains(compInnerSpec))
                            reducedSpecifications.Add(compInnerSpec);
                    }
                else
                    if (!reducedSpecifications.Contains(specification))
                        reducedSpecifications.Add(specification);
            }
            return reducedSpecifications;
        }

        protected abstract Expression<Func<TEntity, bool>> CombineExpressions(Expression<Func<TEntity, bool>> exp1,
            Expression<Func<TEntity, bool>> exp2);

        internal sealed override Expression<Func<TEntity, bool>> ToExpression()
        {
            Expression<Func<TEntity, bool>> predicate = null;
            foreach (var specification in Specifications)
            {
                if (predicate == null)
                    predicate = specification.ToExpression();
                else
                    predicate = CombineExpressions(predicate, specification.ToExpression());
            }

            if (predicate == null) // by default will satisfy (if no arguments were specified)
                return e => true;

            return predicate;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;

            if (obj.GetType() != GetType())
                return false;

            var otherSpec = (CompositeSpecification<TEntity>)obj;

            // Assuming here that there are no duplicate specifications in composites (see "Reduce" method)
            if (Specifications.Count() != otherSpec.Specifications.Count())
                return false;

            return Specifications.All(s1 => otherSpec.Specifications.Any(s2 => s2 == s1));
        }

        public override int GetHashCode()
        {
            var hashes = Specifications.Select(s => s?.GetHashCode() ?? 0)
                .OrderBy(h => h).ToList();
            hashes.Add(GetType().GetHashCode());
            return CombineHashCodes(hashes);
        }

        public bool Equals(CompositeSpecification<TEntity> other)
        {
            return Equals((object) other);
        }

        public static bool operator ==(CompositeSpecification<TEntity> left, CompositeSpecification<TEntity> right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(CompositeSpecification<TEntity> left, CompositeSpecification<TEntity> right)
        {
            return !Equals(left, right);
        }
    }
}
